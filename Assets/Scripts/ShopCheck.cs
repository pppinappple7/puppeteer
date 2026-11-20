using TMPro;
using UnityEngine;

public class ShopCheck : MonoBehaviour
{
    public int total;
    [SerializeField]Inventory inventory;
    [SerializeField] TextMeshProUGUI listText;
    [SerializeField] TextMeshProUGUI totalText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        total = 0;
        listText.text = "";
        foreach(var  item in inventory.cart)
        {
            listText.text += item.name + " x" + item.ammount + " - " + (item.price * item.ammount).ToString() + "\n";
            total += item.ammount * item.price;
        }
        totalText.text=total.ToString();
    }
}
