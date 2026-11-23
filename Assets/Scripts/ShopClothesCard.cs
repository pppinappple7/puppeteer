using UnityEngine;
using UnityEngine.EventSystems;

public class ShopClothesCard : MonoBehaviour, IPointerClickHandler
{
    public string name;
    public GameObject countObj;
    public string type;
    public Mesh mesh1;
    public Mesh mesh2;
    public Inventory inventory;
    float doubleClickTimer;
    public Sprite preview;
    public Sprite icon;
    public GameObject previewObj;
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
        int itemPrice, string itemType, Mesh firstMesh, Mesh secondMesh, Sprite itemIcon, Sprite itemPreview)
    {
        inventory = targetInventory;
        previewObj = targetPreview;
        name = itemName;
        price = itemPrice;
        type = itemType;
        mesh1 = firstMesh;
        mesh2 = secondMesh;
        icon = itemIcon;
        preview = itemPreview;
    }

    public void open()
    {
        if (doubleClickTimer > 0f)
        {
            ShopCartUtility.AddClothes(inventory, name, price, icon, type, mesh1, mesh2);
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
