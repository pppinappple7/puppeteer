using System.Collections.Generic;
using UnityEngine;

public enum ShopProductType
{
    Cloth,
    Pattern,
    Clothes
}

[System.Serializable]
public class ShopProductData
{
    [SerializeField] string productName;
    [SerializeField] ShopProductType productType;
    [SerializeField] int price;
    [SerializeField] Sprite icon;
    [SerializeField] Sprite preview;

    [Header("Cloth")]
    [SerializeField] Color color = Color.white;

    [Header("Pattern and clothes")]
    [SerializeField] string type;
    [SerializeField] Mesh mesh;
    [SerializeField] Mesh secondMesh;

    public string ProductName => productName;
    public ShopProductType ProductType => productType;
    public int Price => price;
    public Sprite Icon => icon;
    public Sprite Preview => preview;
    public Color Color => color;
    public string Type => type;
    public Mesh Mesh => mesh;
    public Mesh SecondMesh => secondMesh;

    public bool TryValidate(out string error)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            error = "Product name is empty.";
            return false;
        }

        if (price < 0)
        {
            error = "Product price cannot be negative.";
            return false;
        }

        if (productType == ShopProductType.Pattern)
        {
            if (string.IsNullOrWhiteSpace(type) || mesh == null)
            {
                error = "A pattern requires a type and a mesh.";
                return false;
            }
        }
        else if (productType == ShopProductType.Clothes)
        {
            if (string.IsNullOrWhiteSpace(type) || mesh == null || secondMesh == null)
            {
                error = "Clothes require a type and two meshes.";
                return false;
            }
        }
        else if (productType != ShopProductType.Cloth)
        {
            error = "Product type is not supported.";
            return false;
        }

        error = null;
        return true;
    }
}

[CreateAssetMenu(fileName = "ShopCatalog", menuName = "Doll Game/Shop Catalog")]
public class ShopCatalog : ScriptableObject
{
    [SerializeField] List<ShopProductData> items = new List<ShopProductData>();

    public IReadOnlyList<ShopProductData> Items => items;
}
