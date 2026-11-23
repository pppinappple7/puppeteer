using UnityEngine;
using UnityEngine.UI;

public class ClothesCard : MonoBehaviour
{
    public int price;
    public string type;
    public Mesh mesh1;
    public Mesh mesh2;
    public Color color1;
    public Color color2;
    public CombineSetUp combineSetUp;
    public Inventory inventory;
    public Sprite icon;

    private Inventory.Clothes clothesItem;
    private CanvasGroup canvasGroup;
    private float availableAlpha = 1f;
    private bool available = true;

    public void Configure(Inventory.Clothes clothes, CombineSetUp setup)
    {
        clothesItem = clothes;
        combineSetUp = setup;

        if (clothes != null)
        {
            price = clothes.price;
            type = clothes.type;
            mesh1 = clothes.mesh1;
            mesh2 = clothes.mesh2;
            color1 = clothes.color1;
            color2 = clothes.color2;
            icon = clothes.icon;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        availableAlpha = canvasGroup.alpha;
    }

    public void SetAvailable(bool value)
    {
        available = value;

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
        canvasGroup.alpha = value ? availableAlpha : availableAlpha * 0.45f;

        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
            button.interactable = value;
    }

    public void Select()
    {
        if (!available || clothesItem == null || combineSetUp == null)
            return;

        combineSetUp.TryInstallClothes(clothesItem);
    }
}
