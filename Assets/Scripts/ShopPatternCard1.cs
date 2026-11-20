using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class ShopPatternCard : MonoBehaviour, IPointerClickHandler
{
    public GameObject countObj;
    public string type;
    public Mesh mesh;
    public Inventory inventory;
    float doubleClickTimer=0;
    public Sprite preview;
    public Sprite icon;
    public GameObject previewObj;
    public string name;
    public int price;
    void Start()
    {
        foreach (var item in inventory.cart)
        {
            Debug.Log("aa");
            if (item.name == name)
            {
                Debug.Log("found");
                countObj.SetActive(true);
                countObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.ammount.ToString();
                
            }
            else
            {
                countObj.SetActive(false);
                Debug.Log("not found");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doubleClickTimer > -10)
        {
            doubleClickTimer -= Time.deltaTime;
        }
        
    }
    public void open()
    {
        if (doubleClickTimer > 0f)
        {
            countObj.SetActive(true);
            bool isInCart = false;
            foreach (var item in inventory.cart)
            {
                if (item.name == name)
                {
                    item.ammount = 1;
                    isInCart = true;
                    countObj.SetActive(true);
                    countObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.ammount.ToString();
                }
            }
            if (!isInCart)
            {
                countObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text="1";
                inventory.cart.Add(new Inventory.ShopPart
                {
                    icon = icon,
                    name = name,
                    price = price,
                    mesh = mesh,
                    ammount = 1,
                    type=type,
                
                });
            }
        }
        doubleClickTimer = 0.25f;
        previewObj.GetComponent<UnityEngine.UI.Image>().sprite = preview;
        previewObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        previewObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = price.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            foreach (var item in inventory.cart)
            {
                if (item.name == name)
                {
                    item.ammount--;
                    if (item.ammount <= 0)
                    {

                        item.ammount = 0;
                        inventory.cart.Remove(item);
                        countObj.SetActive(false);
                    }

                    countObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.ammount.ToString();
                    return;
                }
            }
        }
           
    }
}
