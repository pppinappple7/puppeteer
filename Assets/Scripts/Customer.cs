
using TMPro;
using UnityEngine;


public class Customer : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    int payment;
    bool over=false;
    bool good;
    bool gotDolled=false;
    SpriteRenderer spriteRenderer;
    UIShower selfUI;
    [SerializeField] UIShower uiShower;
    [SerializeField] float letterPeriod = 0.1f;
    float speechTimer;
    [SerializeField] TextMeshProUGUI Speech;
    int curentText;
    public bool shown;
    bool taken;
    [SerializeField] string[] order;
    [SerializeField] string[] gratitude;
    [SerializeField] string[] neutral;
    void Start()
    {
        
        shown=true; taken=false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(100, 100, 100, 0);
        selfUI = GetComponent<UIShower>();
        Speech.maxVisibleCharacters = 0;
        Speech.text = order[curentText];
    }

    // Update is called once per frame
    void Update()
    {
        Color color = spriteRenderer.color;
        if (shown)
        {
            

            color.a = Mathf.MoveTowards(
                color.a,
                shown ? 1f : 0f,
                5f * Time.deltaTime
            );
            selfUI.shown = true;
            speechTimer += Time.deltaTime;
            if (speechTimer > letterPeriod)
            {
                Speech.maxVisibleCharacters += 1;
                speechTimer = 0;
            }
        }
        else
        {
            Speech.text = "";
            curentText = 0;
            selfUI.shown = false;
            color.a = Mathf.MoveTowards(
            color.a,
            shown ? 1f : 0f,
            5f * Time.deltaTime
            );
        }
        spriteRenderer.color = color;
    }
    private void LateUpdate()
    {
        if ((!taken||gotDolled)&& shown)
        {
            uiShower.shown = false;
        }
        if (over)
        {
            shown = false;
            if (spriteRenderer.color.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
    public void Next()
    {
        curentText++;
        Speech.maxVisibleCharacters = 0;
        if (!taken)
        {
            
            if (curentText > order.Length - 1)
            {
                taken = true;
            }
            if (!taken)
            {
                Speech.text = order[curentText];
            }
            else
            {
                shown = false;
                Speech.text = "";
            }
        }
        else if (gotDolled)
        {
            if (good)
            {
                Speech.text = gratitude[curentText];
                if (curentText > gratitude.Length - 1)
                {
                    over=true;
                }
            }
            else
            {
                Speech.text = neutral[curentText];
                if (curentText > neutral.Length - 1)
                {
                    over = true;
                }
            }
        }

    }
    public void GetDolled(GameObject doll)
    {
        gotDolled = true;
        good = true;
        inventory.money += payment;
        inventory.moneyCounter.transform.localScale *= 2;
    }
}
