using UnityEngine;

public class SewClothesCard : MonoBehaviour
{
    public int price;
    public SewResulter sewResulter;
    public string type;
    public Mesh mesh1;
    public Mesh mesh2;
    public Inventory inventory;
    public Sprite icon;
    public new string name;

    public Inventory.SewingClothes sewingClothes;

    public void Selected()
    {
        if (sewResulter == null || sewingClothes == null)
        {
            return;
        }

        sewResulter.SelectClothes(sewingClothes);
    }
}
