using UnityEngine;
using UnityEngine.UI;

public class PartCard : MonoBehaviour
{
    public int price;
    public string type;
    public Mesh mesh;
    public Color color;
    public CombineSetUp combineSetUp;
    public Inventory inventory;
    public Sprite icon;

    private Inventory.Part partItem;
    private CanvasGroup canvasGroup;
    private float availableAlpha = 1f;
    private bool available = true;

    public void Configure(Inventory.Part part, CombineSetUp setup)
    {
        partItem = part;
        combineSetUp = setup;

        if (part != null)
        {
            price = part.price;
            type = part.type;
            mesh = part.mesh;
            color = part.color;
            icon = part.icon;
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
        if (!available || partItem == null || combineSetUp == null)
            return;

        combineSetUp.TryInstallPart(partItem);
    }
}
