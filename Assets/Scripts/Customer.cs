using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public ShiftManager ShiftManager;
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
    public GameObject doll;
    public Inventory inventory;

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
        uiShower=GameObject.Find("Combiner").GetComponent<UIShower>();
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

        if (over && spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
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
                ShiftManager.customersServed++;
                return;
            }

            if (Speech != null)
                Speech.text = reaction[curentText];
        }
    }

    public void GetDolled()
    {
        if (gotDolled)
            return;

        if (inventory == null)
        {
            return;
        }

        if (inventory.moneyCounter == null)
        {
            return;
        }

        DollEvaluationResult result = DollEvaluator.Evaluate(
            doll,
            colorRequirements,
            meshRequirements,
            requiredPercent);

        if (!result.IsValid)
        {
            return;
        }

        int reward = result.IsGood
            ? (int)(result.Payment * 1.1f)
            : result.Payment;

        long updatedMoney = (long)inventory.money + reward;
        if (updatedMoney > int.MaxValue || updatedMoney < int.MinValue)
        {
            return;
        }

        if (doll == null)
        {
            return;
        }

        DollAssembly assembly = doll.GetComponent<DollAssembly>();
        if (assembly == null)
            assembly = doll.GetComponentInChildren<DollAssembly>(true);
            
        if (assembly == null)
        {
            return;
        }

        if (!assembly.TryCompleteOrder(inventory, out string assemblyError))
        {
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
        }

    }

    string GetLine(string[] lines, int index)
    {
        if (lines == null || index < 0 || index >= lines.Length)
            return "";

        return lines[index];
    }
}
