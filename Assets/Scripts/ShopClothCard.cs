using UnityEngine;
using UnityEngine.EventSystems;

public class ShopClothCard : MonoBehaviour, IPointerClickHandler
{
    public GameObject countObj;
    public Color color;
    public Inventory inventory;
    float doubleClickTimer;
    public Sprite preview;
    public Sprite icon;
    public GameObject previewObj;
    public string name;
    public int price;

    void Start()
    {
        RefreshCount();
    }

    void Update()
    {
        if (doubleClickTimer > -10f)
            doubleClickTimer -= Time.deltaTime;
    }

    public void Configure(Inventory targetInventory, GameObject targetPreview, string itemName,
        int itemPrice, Color itemColor, Sprite itemIcon, Sprite itemPreview)
    {
        inventory = targetInventory;
        previewObj = targetPreview;
        name = itemName;
        price = itemPrice;
        color = itemColor;
        icon = itemIcon;
        preview = itemPreview;
    }

    public void open()
    {
        if (doubleClickTimer > 0f)
        {
            ShopCartUtility.AddCloth(inventory, name, price, icon, color);
            RefreshCount();
        }

        doubleClickTimer = 0.25f;
        ShopCartUtility.ShowPreview(previewObj, preview, name, price);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        int amount = ShopCartUtility.RemoveOne(inventory, name);
        ShopCartUtility.SetCount(countObj, amount);
    }

    void RefreshCount()
    {
        ShopCartUtility.SetCount(countObj, ShopCartUtility.GetAmount(inventory, name));
    }
}
