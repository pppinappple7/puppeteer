using UnityEngine;
using UnityEngine.EventSystems;

public class ShopPatternCard : MonoBehaviour, IPointerClickHandler
{
    public GameObject countObj;
    public string type;
    public Mesh mesh;
    public Inventory inventory;
    float doubleClickTimer;
    public Sprite preview;
    public Sprite icon;
    public GameObject previewObj;
    public string itemId;
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
        int itemPrice, string itemType, Mesh itemMesh, Sprite itemIcon, Sprite itemPreview, string targetItemId = null)
    {
        inventory = targetInventory;
        previewObj = targetPreview;
        itemId = targetItemId;
        name = itemName;
        price = itemPrice;
        type = itemType;
        mesh = itemMesh;
        icon = itemIcon;
        preview = itemPreview;
    }

    public void open()
    {
        if (doubleClickTimer > 0f)
        {
            ShopCartUtility.AddPattern(inventory, name, price, icon, type, mesh, itemId);
            RefreshCount();
        }

        doubleClickTimer = 0.25f;
        ShopCartUtility.ShowPreview(previewObj, preview, name, price);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        int amount = ShopCartUtility.RemoveOne(inventory, name, itemId);
        ShopCartUtility.SetCount(countObj, amount);
    }

    void RefreshCount()
    {
        ShopCartUtility.SetCount(countObj, ShopCartUtility.GetAmount(inventory, name, itemId));
    }
}
