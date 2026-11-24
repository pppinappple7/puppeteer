using UnityEngine;

public class SewPatternCard : MonoBehaviour
{
    public int price;
    public SewResulter sewResulter;
    public string type;
    public Mesh mesh;
    public Inventory inventory;
    public Sprite icon;
    public new string name;

    public Inventory.SewingPart sewingPart;

    public void Selected()
    {
        if (sewResulter == null || sewingPart == null)
        {
            return;
        }

        sewResulter.SelectPattern(sewingPart);
    }
}
