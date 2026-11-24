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

    [Header("Product catalog")]
    [SerializeField] ShopCatalog catalog;

    [System.Serializable]
    public class ClothInfo
    {
        public string itemId;
        public string name;
        public Color color;
        public int price;
        public Sprite icon;
        public Sprite preview;
    }

    [System.Serializable]
    public class PatternInfo
    {
        public string itemId;
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
        public string itemId;
        public string type;
        public string name;
        public Mesh mesh1;
        public Mesh mesh2;
        public int price;
        public Sprite icon;
        public Sprite preview;
    }

    readonly List<GameObject> spawnedCards = new List<GameObject>();

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        ClearSpawnedCards();

        if (catalog == null || inventory == null)
            return;

        CreateClothCards();
        CreatePatternCards();
        CreateClothesCards();
    }

    void CreateClothCards()
    {
        if (catalog.ClothInfos == null)
            return;

        int index = 0;

        foreach (ClothInfo info in catalog.ClothInfos)
        {
            if (!IsValid(info))
                continue;

            CreateClothCard(info, index++);
        }
    }

    void CreatePatternCards()
    {
        if (catalog.PatternInfos == null)
            return;

        int index = 0;

        foreach (PatternInfo info in catalog.PatternInfos)
        {
            if (!IsValid(info))
                continue;

            CreatePatternCard(info, index++);
        }
    }

    void CreateClothesCards()
    {
        if (catalog.ClothesInfos == null)
            return;

        int index = 0;

        foreach (ClothesInfo info in catalog.ClothesInfos)
        {
            if (!IsValid(info))
                continue;

            CreateClothesCard(info, index++);
        }
    }

    void CreateClothCard(ClothInfo info, int index)
    {
        if (!CanCreate(clothCardPrefub, clothContentBox))
            return;

        GameObject cardObject = Instantiate(clothCardPrefub, clothContentBox);
        ShopClothCard card = cardObject.GetComponent<ShopClothCard>();

        if (card == null)
        {
            Destroy(cardObject);
            return;
        }

        spawnedCards.Add(cardObject);
        cardObject.transform.SetAsFirstSibling();
        SetCardPosition(cardObject, index);
        card.Configure(inventory, previewObj, info.name, info.price, info.color, info.icon, info.preview, info.itemId);
        SetCardIcon(cardObject, info.icon);
    }

    void CreatePatternCard(PatternInfo info, int index)
    {
        if (!CanCreate(patternCardPrefub, patternContentBox))
            return;

        GameObject cardObject = Instantiate(patternCardPrefub, patternContentBox);
        ShopPatternCard card = cardObject.GetComponent<ShopPatternCard>();

        if (card == null)
        {
            Destroy(cardObject);
            return;
        }

        spawnedCards.Add(cardObject);
        cardObject.transform.SetAsFirstSibling();
        SetCardPosition(cardObject, index);
        card.Configure(inventory, previewObj, info.name, info.price, info.type, info.mesh,
            info.icon, info.preview, info.itemId);
        SetCardIcon(cardObject, info.icon);
    }

    void CreateClothesCard(ClothesInfo info, int index)
    {
        if (!CanCreate(clothesCardPrefub, clothesContentBox))
            return;

        GameObject cardObject = Instantiate(clothesCardPrefub, clothesContentBox);
        ShopClothesCard card = cardObject.GetComponent<ShopClothesCard>();

        if (card == null)
        {
            Destroy(cardObject);
            return;
        }

        spawnedCards.Add(cardObject);
        cardObject.transform.SetAsFirstSibling();
        SetCardPosition(cardObject, index);
        card.Configure(inventory, previewObj, info.name, info.price, info.type,
            info.mesh1, info.mesh2, info.icon, info.preview, info.itemId);
        SetCardIcon(cardObject, info.icon);
    }

    static bool IsValid(ClothInfo info)
    {
        return info != null &&
               !string.IsNullOrWhiteSpace(info.itemId) &&
               !string.IsNullOrWhiteSpace(info.name) &&
               info.price >= 0;
    }

    static bool IsValid(PatternInfo info)
    {
        return info != null &&
               !string.IsNullOrWhiteSpace(info.itemId) &&
               !string.IsNullOrWhiteSpace(info.name) &&
               !string.IsNullOrWhiteSpace(info.type) &&
               info.mesh != null &&
               info.price >= 0;
    }

    static bool IsValid(ClothesInfo info)
    {
        return info != null &&
               !string.IsNullOrWhiteSpace(info.itemId) &&
               !string.IsNullOrWhiteSpace(info.name) &&
               !string.IsNullOrWhiteSpace(info.type) &&
               info.mesh1 != null &&
               info.mesh2 != null &&
               info.price >= 0;
    }

    bool CanCreate(GameObject prefab, Transform content)
    {
        return prefab != null && content != null && inventory != null;
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
