using System.Collections.Generic;
using UnityEngine;

public enum ShopProductType
{
    Cloth,
    Pattern,
    Clothes
}

public class ShopProductData
{
    public string ItemId { get; }
    public string ProductName { get; }
    public ShopProductType ProductType { get; }
    public int Price { get; }
    public Sprite Icon { get; }
    public Sprite Preview { get; }
    public Color Color { get; }
    public string Type { get; }
    public Mesh Mesh { get; }
    public Mesh SecondMesh { get; }

    public ShopProductData(ShopSetUp.ClothInfo info)
    {
        ItemId = info.itemId;
        ProductName = info.name;
        ProductType = ShopProductType.Cloth;
        Price = info.price;
        Icon = info.icon;
        Preview = info.preview;
        Color = info.color;
    }

    public ShopProductData(ShopSetUp.PatternInfo info)
    {
        ItemId = info.itemId;
        ProductName = info.name;
        ProductType = ShopProductType.Pattern;
        Price = info.price;
        Icon = info.icon;
        Preview = info.preview;
        Type = info.type;
        Mesh = info.mesh;
    }

    public ShopProductData(ShopSetUp.ClothesInfo info)
    {
        ItemId = info.itemId;
        ProductName = info.name;
        ProductType = ShopProductType.Clothes;
        Price = info.price;
        Icon = info.icon;
        Preview = info.preview;
        Type = info.type;
        Mesh = info.mesh1;
        SecondMesh = info.mesh2;
    }

    public bool TryValidate(out string error)
    {
        if (string.IsNullOrWhiteSpace(ProductName) || Price < 0)
        {
            error = "Invalid product data.";
            return false;
        }

        if (ProductType == ShopProductType.Pattern)
        {
            if (string.IsNullOrWhiteSpace(Type) || Mesh == null)
            {
                error = "Pattern data is incomplete.";
                return false;
            }
        }
        else if (ProductType == ShopProductType.Clothes)
        {
            if (string.IsNullOrWhiteSpace(Type) || Mesh == null || SecondMesh == null)
            {
                error = "Clothes data is incomplete.";
                return false;
            }
        }
        else if (ProductType != ShopProductType.Cloth)
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
    [SerializeField] List<ShopSetUp.ClothInfo> clothInfos = new List<ShopSetUp.ClothInfo>();
    [SerializeField] List<ShopSetUp.PatternInfo> patternInfos = new List<ShopSetUp.PatternInfo>();
    [SerializeField] List<ShopSetUp.ClothesInfo> clothesInfos = new List<ShopSetUp.ClothesInfo>();

    public IReadOnlyList<ShopSetUp.ClothInfo> ClothInfos => clothInfos;
    public IReadOnlyList<ShopSetUp.PatternInfo> PatternInfos => patternInfos;
    public IReadOnlyList<ShopSetUp.ClothesInfo> ClothesInfos => clothesInfos;

    public bool TryGetItem(string itemId, out ShopProductData result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(itemId))
            return false;

        ShopProductData found = null;

        if (clothInfos != null)
        {
            foreach (ShopSetUp.ClothInfo info in clothInfos)
            {
                if (info == null || info.itemId != itemId)
                    continue;

                if (found != null)
                    return false;

                found = new ShopProductData(info);
            }
        }

        if (patternInfos != null)
        {
            foreach (ShopSetUp.PatternInfo info in patternInfos)
            {
                if (info == null || info.itemId != itemId)
                    continue;

                if (found != null)
                    return false;

                found = new ShopProductData(info);
            }
        }

        if (clothesInfos != null)
        {
            foreach (ShopSetUp.ClothesInfo info in clothesInfos)
            {
                if (info == null || info.itemId != itemId)
                    continue;

                if (found != null)
                    return false;

                found = new ShopProductData(info);
            }
        }

        result = found;
        return result != null;
    }

    public bool TryValidateIds(out string error)
    {
        error = null;
        HashSet<string> ids = new HashSet<string>();

        if (clothInfos == null || patternInfos == null || clothesInfos == null)
        {
            error = "One or more catalog lists are missing.";
            return false;
        }

        foreach (ShopSetUp.ClothInfo info in clothInfos)
        {
            if (info == null)
            {
                error = "Catalog contains an empty cloth entry.";
                return false;
            }

            if (!TryAddId(info.itemId, ids, out error))
                return false;
        }

        foreach (ShopSetUp.PatternInfo info in patternInfos)
        {
            if (info == null)
            {
                error = "Catalog contains an empty pattern entry.";
                return false;
            }

            if (!TryAddId(info.itemId, ids, out error))
                return false;
        }

        foreach (ShopSetUp.ClothesInfo info in clothesInfos)
        {
            if (info == null)
            {
                error = "Catalog contains an empty clothes entry.";
                return false;
            }

            if (!TryAddId(info.itemId, ids, out error))
                return false;
        }

        return true;
    }

    static bool TryAddId(string itemId, HashSet<string> ids, out string error)
    {
        if (string.IsNullOrWhiteSpace(itemId))
        {
            error = "Every catalog item needs an itemId.";
            return false;
        }

        if (!ids.Add(itemId))
        {
            error = "Duplicate itemId: " + itemId;
            return false;
        }

        error = null;
        return true;
    }
}
