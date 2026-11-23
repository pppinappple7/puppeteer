using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSetUp : MonoBehaviour
{
    [Header("Card prefabs and containers")]
    [SerializeField] GameObject clothCardPrefub;
    [SerializeField] Transform clothContentBox;
    [SerializeField] GameObject patternCardPrefub;
    [SerializeField] Transform patternContentBox;
    [SerializeField] GameObject clothesCardPrefub;
    [SerializeField] Transform clothesContentBox;
    [SerializeField] GameObject previewObj;
    [SerializeField] Inventory inventory;

    [Header("Additional data catalog")]
    [SerializeField] ShopCatalog catalog;

    [System.Serializable]
    public class ClothInfo
    {
        [Header("Legacy product data")]
        public string name;
        public Color color;
        public int price;
        public Sprite icon;
        public Sprite preview;
    }

    [System.Serializable]
    public class PatternInfo
    {
        public string type;
        public string name;
        public Mesh mesh;
        public int price;
        public Sprite icon;
        public Sprite preview;
    }

    [System.Serializable]
    public class ClothesInfo
    {
        public string type;
        public string name;
        public Mesh mesh1;
        public Mesh mesh2;
        public int price;
        public Sprite icon;
        public Sprite preview;
    }

    [SerializeField] ClothInfo[] clothInfos;
    [SerializeField] PatternInfo[] patternInfos;
    [SerializeField] ClothesInfo[] clothesInfos;

    readonly List<GameObject> spawnedCards = new List<GameObject>();

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        ClearSpawnedCards();

        int clothIndex = 0;
        int patternIndex = 0;
        int clothesIndex = 0;

        if (clothInfos != null)
        {
            foreach (ClothInfo info in clothInfos)
            {
                if (info == null || string.IsNullOrWhiteSpace(info.name) || info.price < 0)
                {
                    Debug.LogWarning("ShopSetUp: skipped invalid legacy cloth entry.", this);
                    continue;
                }

                CreateClothCard(info.name, info.price, info.color, info.icon, info.preview, clothIndex++);
            }
        }

        if (patternInfos != null)
        {
            foreach (PatternInfo info in patternInfos)
            {
                if (info == null || string.IsNullOrWhiteSpace(info.name) ||
                    string.IsNullOrWhiteSpace(info.type) || info.mesh == null || info.price < 0)
                {
                    Debug.LogWarning("ShopSetUp: skipped invalid legacy pattern entry.", this);
                    continue;
                }

                CreatePatternCard(info.name, info.price, info.type, info.mesh, info.icon, info.preview, patternIndex++);
            }
        }

        if (clothesInfos != null)
        {
            foreach (ClothesInfo info in clothesInfos)
            {
                if (info == null || string.IsNullOrWhiteSpace(info.name) ||
                    string.IsNullOrWhiteSpace(info.type) || info.mesh1 == null || info.mesh2 == null || info.price < 0)
                {
                    Debug.LogWarning("ShopSetUp: skipped invalid legacy clothes entry.", this);
                    continue;
                }

                CreateClothesCard(info.name, info.price, info.type, info.mesh1, info.mesh2,
                    info.icon, info.preview, clothesIndex++);
            }
        }

        if (catalog == null || catalog.Items == null)
            return;

        foreach (ShopProductData item in catalog.Items)
        {
            if (item == null)
            {
                Debug.LogWarning("ShopSetUp: skipped an empty catalog entry.", this);
                continue;
            }

            if (!item.TryValidate(out string error))
            {
                Debug.LogWarning("ShopSetUp: skipped catalog item. " + error, this);
                continue;
            }

            switch (item.ProductType)
            {
                case ShopProductType.Cloth:
                    CreateClothCard(item.ProductName, item.Price, item.Color, item.Icon, item.Preview, clothIndex++);
                    break;

                case ShopProductType.Pattern:
                    CreatePatternCard(item.ProductName, item.Price, item.Type, item.Mesh,
                        item.Icon, item.Preview, patternIndex++);
                    break;

                case ShopProductType.Clothes:
                    CreateClothesCard(item.ProductName, item.Price, item.Type, item.Mesh,
                        item.SecondMesh, item.Icon, item.Preview, clothesIndex++);
                    break;
            }
        }
    }

    void CreateClothCard(string itemName, int itemPrice, Color itemColor,
        Sprite itemIcon, Sprite itemPreview, int index)
    {
        if (!CanCreate(clothCardPrefub, clothContentBox, "cloth"))
            return;

        GameObject cardObject = Instantiate(clothCardPrefub, clothContentBox);
        spawnedCards.Add(cardObject);
        cardObject.transform.SetAsFirstSibling();
        SetCardPosition(cardObject, index);

        ShopClothCard card = cardObject.GetComponent<ShopClothCard>();
        if (card == null)
        {
            Debug.LogError("ShopSetUp: cloth prefab has no ShopClothCard component.", cardObject);
            Destroy(cardObject);
            return;
        }

        card.Configure(inventory, previewObj, itemName, itemPrice, itemColor, itemIcon, itemPreview);
        SetCardIcon(cardObject, itemIcon);
    }

    void CreatePatternCard(string itemName, int itemPrice, string itemType, Mesh itemMesh,
        Sprite itemIcon, Sprite itemPreview, int index)
    {
        if (!CanCreate(patternCardPrefub, patternContentBox, "pattern"))
            return;

        GameObject cardObject = Instantiate(patternCardPrefub, patternContentBox);
        spawnedCards.Add(cardObject);
        cardObject.transform.SetAsFirstSibling();
        SetCardPosition(cardObject, index);

        ShopPatternCard card = cardObject.GetComponent<ShopPatternCard>();
        if (card == null)
        {
            Debug.LogError("ShopSetUp: pattern prefab has no ShopPatternCard component.", cardObject);
            Destroy(cardObject);
            return;
        }

        card.Configure(inventory, previewObj, itemName, itemPrice, itemType, itemMesh, itemIcon, itemPreview);
        SetCardIcon(cardObject, itemIcon);
    }

    void CreateClothesCard(string itemName, int itemPrice, string itemType, Mesh firstMesh,
        Mesh secondMesh, Sprite itemIcon, Sprite itemPreview, int index)
    {
        if (!CanCreate(clothesCardPrefub, clothesContentBox, "clothes"))
            return;

        GameObject cardObject = Instantiate(clothesCardPrefub, clothesContentBox);
        spawnedCards.Add(cardObject);
        cardObject.transform.SetAsFirstSibling();
        SetCardPosition(cardObject, index);

        ShopClothesCard card = cardObject.GetComponent<ShopClothesCard>();
        if (card == null)
        {
            Debug.LogError("ShopSetUp: clothes prefab has no ShopClothesCard component.", cardObject);
            Destroy(cardObject);
            return;
        }

        card.Configure(inventory, previewObj, itemName, itemPrice, itemType,
            firstMesh, secondMesh, itemIcon, itemPreview);
        SetCardIcon(cardObject, itemIcon);
    }

    bool CanCreate(GameObject prefab, Transform content, string itemType)
    {
        if (prefab != null && content != null && inventory != null)
            return true;

        Debug.LogError("ShopSetUp: missing prefab, content container or Inventory for " + itemType + " cards.", this);
        return false;
    }

    static void SetCardIcon(GameObject cardObject, Sprite icon)
    {
        if (cardObject.transform.childCount == 0)
            return;

        Image image = cardObject.transform.GetChild(0).GetComponent<Image>();
        if (image != null)
            image.sprite = icon;
    }

    static void SetCardPosition(GameObject card, int index)
    {
        int x = index % 3;
        int y = index / 3;
        card.transform.localPosition = new Vector2(75f - 37.22f + x * 75f, -50f - 75f * y);
    }

    void ClearSpawnedCards()
    {
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            if (spawnedCards[i] != null)
                Destroy(spawnedCards[i]);
        }

        spawnedCards.Clear();
    }
}
