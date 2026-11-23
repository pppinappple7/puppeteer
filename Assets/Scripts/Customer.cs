using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float requiredPercent;

    [System.Serializable]
    public class Requirement
    {
        public string objName;
        public bool clothes;
    }

    [System.Serializable]
    public class ColorRequirement : Requirement
    {
        public Color color;
    }

    [System.Serializable]
    public class MeshRequirement : Requirement
    {
        public Mesh mesh;
    }

    [SerializeField] ColorRequirement[] colorRequirements;
    [SerializeField] MeshRequirement[] meshRequirements;
    [SerializeField] GameObject doll;
    [SerializeField] Inventory inventory;

    public int payment;
    public int goods;

    bool over;
    bool good;
    bool gotDolled;
    bool taken;

    SpriteRenderer spriteRenderer;
    UIShower selfUI;

    [SerializeField] UIShower uiShower;
    [SerializeField] float letterPeriod = 0.1f;
    float speechTimer;
    [SerializeField] TextMeshProUGUI Speech;

    public int curentText;
    public bool shown;

    [SerializeField] string[] order;
    [SerializeField] string[] gratitude;
    [SerializeField] string[] neutral;

    void Start()
    {
        shown = true;
        taken = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        selfUI = GetComponent<UIShower>();

        if (spriteRenderer != null)
            spriteRenderer.color = new Color(100f, 100f, 100f, 0f);

        if (Speech != null)
        {
            Speech.maxVisibleCharacters = 0;
            Speech.text = GetLine(order, 0);
        }
        else
        {
            Debug.LogWarning("Customer has no speech text assigned.", this);
        }

        if (selfUI == null)
            Debug.LogWarning("Customer has no UIShower component.", this);
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;

            if (shown)
            {
                color.a = Mathf.MoveTowards(color.a, 1f, 5f * Time.deltaTime);

                if (selfUI != null)
                    selfUI.shown = true;

                speechTimer += Time.deltaTime;
                if (Speech != null && letterPeriod > 0f && speechTimer > letterPeriod)
                {
                    Speech.maxVisibleCharacters++;
                    speechTimer = 0f;
                }
            }
            else
            {
                if (Speech != null)
                    Speech.text = "";

                curentText = 0;

                if (selfUI != null)
                    selfUI.shown = false;

                color.a = Mathf.MoveTowards(color.a, 0f, 5f * Time.deltaTime);
            }

            spriteRenderer.color = color;
        }

        if ((!taken || gotDolled) && shown && uiShower != null)
            uiShower.shown = false;

        if (over)
        {
            shown = false;
            if (spriteRenderer == null || spriteRenderer.color.a <= 0f)
                Destroy(gameObject);
        }

        if (doll != null)
            doll.SetActive(uiShower != null && uiShower.shown);
    }

    public void Next()
    {
        curentText++;

        if (Speech != null)
            Speech.maxVisibleCharacters = 0;

        if (!taken)
        {
            if (order == null || curentText >= order.Length)
            {
                taken = true;
                shown = false;
                if (Speech != null)
                    Speech.text = "";
                return;
            }

            if (Speech != null)
                Speech.text = order[curentText];
        }
        else if (gotDolled)
        {
            shown = true;
            string[] reaction = good ? gratitude : neutral;

            if (reaction == null || curentText >= reaction.Length)
            {
                over = true;
                shown = false;
                return;
            }

            if (Speech != null)
                Speech.text = reaction[curentText];
        }
    }

    public void GetDolled(GameObject dollToEvaluate)
    {
        if (gotDolled)
            return;

        if (inventory == null)
        {
            Debug.LogWarning("Customer cannot complete the order: Inventory is not assigned.", this);
            return;
        }

        if (inventory.moneyCounter == null)
        {
            Debug.LogWarning("Customer cannot complete the order: moneyCounter is not assigned.", this);
            return;
        }

        DollEvaluationResult result = DollEvaluator.Evaluate(
            dollToEvaluate,
            colorRequirements,
            meshRequirements,
            requiredPercent);

        if (!result.IsValid)
        {
            Debug.LogWarning("Customer could not evaluate the doll: " + result.Error, this);
            return;
        }

        int reward = result.IsGood
            ? (int)(result.Payment * 1.1f)
            : result.Payment;

        long updatedMoney = (long)inventory.money + reward;
        if (updatedMoney > int.MaxValue || updatedMoney < int.MinValue)
        {
            Debug.LogWarning("Customer cannot complete the order: money value would overflow.", this);
            return;
        }

        if (dollToEvaluate == null)
        {
            Debug.LogWarning("Customer cannot complete the order: doll reference is missing.", this);
            return;
        }

        DollAssembly assembly = dollToEvaluate.GetComponent<DollAssembly>();
        if (assembly == null)
            assembly = dollToEvaluate.GetComponentInChildren<DollAssembly>(true);

        if (assembly == null)
        {
            Debug.LogWarning("Customer cannot complete the order: DollAssembly is missing from the doll.", this);
            return;
        }

        if (!assembly.TryCompleteOrder(inventory, out string assemblyError))
        {
            Debug.LogWarning("Customer could not consume the installed items: " + assemblyError, this);
            return;
        }

        payment = result.Payment;
        goods = result.MatchedRequirements;
        good = result.IsGood;

        inventory.money = (int)updatedMoney;
        inventory.moneyCounter.transform.localScale *= 2f;

        gotDolled = true;
        curentText = 0;
        shown = true;

        string[] reaction = good ? gratitude : neutral;
        if (reaction != null && reaction.Length > 0)
        {
            if (Speech != null)
            {
                Speech.maxVisibleCharacters = 0;
                Speech.text = reaction[0];
            }
        }
        else
        {
            over = true;
            shown = false;
            Debug.LogWarning("Customer has no reaction dialogue configured.", this);
        }
    }

    string GetLine(string[] lines, int index)
    {
        if (lines == null || index < 0 || index >= lines.Length)
            return "";

        return lines[index];
    }
}
