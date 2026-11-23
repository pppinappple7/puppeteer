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
            Debug.LogWarning("SewPatternCard: не назначена выкройка или SewResulter.", this);
            return;
        }

        sewResulter.SelectPattern(sewingPart);
    }
}
