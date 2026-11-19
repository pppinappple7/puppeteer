using TMPro;

using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static SewSetUp;
using static ShopSetUp;
using static UnityEditor.Progress;

public class ShopSetUp : MonoBehaviour
{ 
    int x = 0;
    int y = 0; 
    [SerializeField] GameObject clothCardPrefub;
    [SerializeField] Transform clothContentBox;
    [SerializeField] GameObject patternCardPrefub;
    [SerializeField] Transform patternContentBox;
    [SerializeField] GameObject clothesCardPrefub;
    [SerializeField] Transform clothesContentBox;
    [SerializeField] GameObject previewObj;
    [SerializeField] Inventory inventory;

    [System.Serializable]
    public class ClothInfo
    {

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
    void Start()
    {

        foreach(var clothInfo in clothInfos)
        {
            GameObject spawnedCard = Instantiate(clothCardPrefub,clothContentBox.transform);
            spawnedCard.transform.SetAsFirstSibling();
            spawnedCard.transform.localPosition= new Vector2(75 - 37.22f + x * 75, -50 + 75 * y);
            x++;
            if (x >= 3) { x = 0; y--; }
            spawnedCard.GetComponent<ShopClothCard>().name=clothInfo.name;
            spawnedCard.GetComponent<ShopClothCard>().price = clothInfo.price;
            spawnedCard.GetComponent<ShopClothCard>().color = clothInfo.color;
            spawnedCard.GetComponent<ShopClothCard>().preview = clothInfo.preview;
            spawnedCard.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = clothInfo.icon;
            spawnedCard.GetComponent<ShopClothCard>().inventory = inventory;
            spawnedCard.GetComponent<ShopClothCard>().previewObj = previewObj;
            spawnedCard.GetComponent<ShopClothCard>().icon=clothInfo.icon;


        }
        x = 0;
        y = 0;
        foreach (var patternInfo in patternInfos)
        {
            GameObject spawnedCard = Instantiate(patternCardPrefub, patternContentBox.transform);
            spawnedCard.transform.SetAsFirstSibling();
            spawnedCard.transform.localPosition = new Vector2(75 - 37.22f + x * 75, -50 + 75 * y);
            x++;
            if (x >= 3) { x = 0; y--; } 
            spawnedCard.GetComponent<ShopPatternCard>().name = patternInfo.name;
            spawnedCard.GetComponent<ShopPatternCard>().price = patternInfo.price;
            spawnedCard.GetComponent<ShopPatternCard>().mesh = patternInfo.mesh;
            spawnedCard.GetComponent<ShopPatternCard>().preview = patternInfo.preview;
            spawnedCard.GetComponent<ShopPatternCard>().type=patternInfo.type;
            spawnedCard.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = patternInfo.icon;
            spawnedCard.GetComponent<ShopPatternCard>().inventory = inventory;
            spawnedCard.GetComponent<ShopPatternCard>().previewObj = previewObj;
            spawnedCard.GetComponent<ShopPatternCard>().icon = patternInfo.icon;
        }
        x = 0;
        y = 0;
        foreach (var clothesInfo in clothesInfos)
        {
            GameObject spawnedCard = Instantiate(clothesCardPrefub, clothesContentBox.transform);
            spawnedCard.transform.SetAsFirstSibling();
            spawnedCard.transform.localPosition = new Vector2(75- 37.22f + x * 75, -50 + 75 * y);
            x++;
            if (x >= 3) { x = 0; y--; }
            spawnedCard.GetComponent<ShopClothesCard>().name = clothesInfo.name;
            spawnedCard.GetComponent<ShopClothesCard>().price = clothesInfo.price;
            spawnedCard.GetComponent<ShopClothesCard>().mesh1 = clothesInfo.mesh1;
            spawnedCard.GetComponent<ShopClothesCard>().mesh2 = clothesInfo.mesh2;
            spawnedCard.GetComponent<ShopClothesCard>().preview = clothesInfo.preview;
            spawnedCard.GetComponent<ShopPatternCard>().type = clothesInfo.type;
            spawnedCard.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = clothesInfo.icon;
            spawnedCard.GetComponent<ShopClothesCard>().inventory = inventory;
            spawnedCard.GetComponent<ShopClothesCard>().previewObj = previewObj;
            spawnedCard.GetComponent<ShopClothesCard>().icon = clothesInfo.icon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
