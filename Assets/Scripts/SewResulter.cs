using UnityEngine;
using UnityEngine.Serialization;

public class SewResulter : MonoBehaviour
{
    public int price;
    public int price1;
    public int price2;
    public Sprite icon;
    public string type;

    public MeshRenderer renderer1;
    public MeshRenderer renderer2;
    public MeshFilter meshFilter1;
    public MeshFilter meshFilter2;

    public bool clothes = false;

    [SerializeField] private Inventory inventory;
    [FormerlySerializedAs("SewSetUp")]
    [SerializeField] private SewSetUp sewSetUp;
    [SerializeField] private GameObject secondColorPanel;
    [SerializeField] private GameObject model1;
    [SerializeField] private GameObject model2;

    private Inventory.SewingCloth selectedCloth1;
    private Inventory.SewingCloth selectedCloth2;
    private Inventory.SewingPart selectedPartPattern;
    private Inventory.SewingClothes selectedClothesPattern;

    private Color initialColor1;
    private Color initialColor2;

    private void Awake()
    {
        if (model1 != null)
        {
            if (renderer1 == null)
                renderer1 = model1.GetComponent<MeshRenderer>();

            if (meshFilter1 == null)
                meshFilter1 = model1.GetComponent<MeshFilter>();
        }

        if (model2 != null)
        {
            if (renderer2 == null)
                renderer2 = model2.GetComponent<MeshRenderer>();

            if (meshFilter2 == null)
                meshFilter2 = model2.GetComponent<MeshFilter>();
        }

        if (renderer1 != null)
            initialColor1 = renderer1.material.color;

        if (renderer2 != null)
            initialColor2 = renderer2.material.color;

        if (meshFilter1 != null)
            meshFilter1.mesh = null;

        if (meshFilter2 != null)
            meshFilter2.mesh = null;

        SetClothesMode(false);
    }

    public void SelectCloth(Inventory.SewingCloth cloth, bool accent)
    {
        if (cloth == null)
            return;

        if (accent)
        {
            selectedCloth2 = cloth;
            price2 = cloth.price;

            if (renderer2 != null)
                renderer2.material.color = cloth.color;
        }
        else
        {
            selectedCloth1 = cloth;
            price1 = cloth.price;

            if (renderer1 != null)
                renderer1.material.color = cloth.color;
        }
    }

    public void SelectPattern(Inventory.SewingPart pattern)
    {
        if (pattern == null)
            return;

        selectedPartPattern = pattern;
        selectedClothesPattern = null;
        selectedCloth2 = null;

        clothes = false;
        price = pattern.price;
        price2 = 0;
        type = pattern.type;
        icon = pattern.icon;

        if (meshFilter1 != null)
            meshFilter1.mesh = pattern.mesh;

        if (meshFilter2 != null)
            meshFilter2.mesh = null;

        SetClothesMode(false);
    }

    public void SelectClothes(Inventory.SewingClothes pattern)
    {
        if (pattern == null)
            return;

        selectedClothesPattern = pattern;
        selectedPartPattern = null;

        clothes = true;
        price = pattern.price;
        type = pattern.type;
        icon = pattern.icon;

        if (meshFilter1 != null)
            meshFilter1.mesh = pattern.mesh1;

        if (meshFilter2 != null)
            meshFilter2.mesh = pattern.mesh2;

        SetClothesMode(true);
    }

    public void Sew()
    {
        bool success;
        string error;

        if (clothes)
        {
            success = SewingService.TrySewClothes(
                inventory,
                selectedCloth1,
                selectedCloth2,
                selectedClothesPattern,
                out error
            );
        }
        else
        {
            success = SewingService.TrySewPart(
                inventory,
                selectedCloth1,
                selectedPartPattern,
                out error
            );
        }

        if (!success)
        {
            Debug.LogWarning("Не удалось выполнить пошив: " + error, this);
            return;
        }

        ResetSelection();

        if (sewSetUp != null)
            sewSetUp.Refresh();
    }

    private void ResetSelection()
    {
        selectedCloth1 = null;
        selectedCloth2 = null;
        selectedPartPattern = null;
        selectedClothesPattern = null;

        price = 0;
        price1 = 0;
        price2 = 0;
        icon = null;
        type = null;
        clothes = false;

        if (meshFilter1 != null)
            meshFilter1.mesh = null;

        if (meshFilter2 != null)
            meshFilter2.mesh = null;

        if (renderer1 != null)
            renderer1.material.color = initialColor1;

        if (renderer2 != null)
            renderer2.material.color = initialColor2;

        SetClothesMode(false);
    }

    private void SetClothesMode(bool enabled)
    {
        if (secondColorPanel != null)
            secondColorPanel.SetActive(enabled);

        if (model2 != null)
            model2.SetActive(enabled);
    }
}
