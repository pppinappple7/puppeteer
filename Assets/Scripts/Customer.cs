
using TMPro;
using UnityEngine;
using static Customer;


public class Customer : MonoBehaviour
{
    [SerializeField] float requiredPercent;
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
    bool over=false;
    bool good;
    bool gotDolled=false;
    SpriteRenderer spriteRenderer;
    UIShower selfUI;
    [SerializeField] UIShower uiShower;
    [SerializeField] float letterPeriod = 0.1f;
    float speechTimer;
    [SerializeField] TextMeshProUGUI Speech;
    public int curentText;
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
        
        if ((!taken || gotDolled) && shown)
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
        doll.SetActive(uiShower.shown);
    }
    private void LateUpdate()
    {
        
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
            shown=true;
            if (good)
            {
                if (curentText > gratitude.Length - 1)
                {
                    over = true;
                    shown = false;
                    return;
                }
                Speech.text = gratitude[curentText];

            }
            else
            {
                if (curentText > gratitude.Length - 1)
                {
                    over = true;
                    shown = false;
                    return;
                }
                Speech.text = neutral[curentText];
            }
        }

    }
    public void GetDolled(GameObject doll)
    {
        good = false;
        payment = 0;
        goods = 0;
        foreach(PartPicker partPicker in doll.GetComponentsInChildren<PartPicker>())
        {
            payment += (int)(partPicker.price * 0.5f);
        }
        foreach (Pricer price in doll.GetComponentsInChildren<Pricer>())
        {
            payment += (int)(price.price * 0.5f);
        }
        gotDolled = true;
        
        foreach (ColorRequirement requirement in colorRequirements)
        {
            
            if (doll.transform.Find(requirement.objName).GetComponent<MeshRenderer>().material.color == requirement.color)
            {

                goods++;
            }

        }
        foreach(MeshRequirement requirement1 in meshRequirements)
        {
            if (doll.transform.Find(requirement1.objName).GetComponent<MeshFilter>().mesh == requirement1.mesh)
            {
                goods++;
            }
        }
        if(goods/(colorRequirements.Length+meshRequirements.Length)>=requiredPercent)
        {
            good = true;
        }
        curentText = 0;
        if (good)
        {
            Speech.text = gratitude[curentText];
            inventory.money += (int)(payment * 1.1f);
        }
        else
        {
            Speech.text = neutral[curentText];
            inventory.money += (int)(payment * 1f);
        }

            
        inventory.moneyCounter.transform.localScale *= 2;
        gotDolled = true;
        shown = true;
    }
    
}
