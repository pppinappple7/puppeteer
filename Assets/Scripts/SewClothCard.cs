using TMPro;
using UnityEngine;

public class SewClothCard : MonoBehaviour
{
    public bool accent;
    public SewResulter sewResulter;

    public int price;
    public MeshRenderer targetMesh;
    public Color color;
    public Inventory inventory;
    public Sprite icon;
    public new string name;

    public Inventory.SewingCloth sewingCloth;
    public int ammount;

    [SerializeField] private TextMeshProUGUI ammountText;

    public void Select()
    {
        if (sewResulter == null || sewingCloth == null)
        {
            return;
        }

        sewResulter.SelectCloth(sewingCloth, accent);
    }

    public void UpdateAmountText()
    {
        if (ammountText != null)
            ammountText.text = ammount.ToString();
    }
}
