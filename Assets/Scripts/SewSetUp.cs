using TMPro;

using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static SewSetUp;
using static ShopSetUp;
using static UnityEditor.Progress;

public class SewSetUp : MonoBehaviour
{ 
    int x = 0;
    int y = 0;
    [SerializeField] SewResulter sewResulter;
    [SerializeField] GameObject clothCardPrefub;
    [SerializeField] Transform clothContentBox;
    [SerializeField] Transform clothContentBox2;
    [SerializeField] GameObject patternCardPrefub;
    [SerializeField] Transform patternContentBox;
    [SerializeField] GameObject clothesCardPrefub;
    [SerializeField] Transform clothesContentBox;
    [SerializeField] Inventory inventory;

    [System.Serializable]
    public class ClothInfo
    {

        
        public Color color;
        public int price;
        public Sprite icon;
        
    }
    [System.Serializable]
    public class PatternInfo
    {

        public string name;
        public Mesh mesh;
        
        public Sprite icon;
        
    }
    [System.Serializable]
    public class ClothesInfo
    {

        public string name;
        public Mesh mesh1;
        public Mesh mesh2;
        
        public Sprite icon;
        
    }

    
    void Start()
    {

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Refresh()
    {
        
        foreach (Transform child in clothContentBox)
            Destroy(child.gameObject);

        foreach (Transform child in clothContentBox2)
            Destroy(child.gameObject);

        foreach (Transform child in patternContentBox)
            Destroy(child.gameObject);

        foreach (Transform child in clothesContentBox)
            Destroy(child.gameObject);

        
        int clothIndex = 0;
        int clothIndex2 = 0;
        int patternIndex = 0;
        int clothesIndex = 0;

        foreach (Inventory.SewingItem item in inventory.sewings)
        {
            
            if (item is Inventory.SewingCloth cloth)
            {
                
                GameObject spawnedCard = Instantiate(clothCardPrefub, clothContentBox);
                spawnedCard.transform.SetAsFirstSibling();
                SetCardPosition(spawnedCard, clothIndex++);

                SewClothCard card = spawnedCard.GetComponent<SewClothCard>();

                card.name = cloth.name;
                card.color = cloth.color;
                card.inventory = inventory;
                card.icon = cloth.icon;
                card.ammount = cloth.ammount;
                card.targetMesh = sewResulter.renderer1;
                
                spawnedCard.transform.GetChild(0)
                    .GetComponent<UnityEngine.UI.Image>().sprite = cloth.icon;


                
                GameObject spawnedCard2 = Instantiate(clothCardPrefub, clothContentBox2);
                spawnedCard2.transform.SetAsFirstSibling();
                SetCardPosition(spawnedCard2, clothIndex2++);

                SewClothCard card2 = spawnedCard2.GetComponent<SewClothCard>();

                card2.name = cloth.name;
                card2.color = cloth.color;
                card2.inventory = inventory;
                card2.icon = cloth.icon;
                card2.ammount = cloth.ammount;
                card2.targetMesh = sewResulter.renderer2;
                spawnedCard2.transform.GetChild(0)
                    .GetComponent<UnityEngine.UI.Image>().sprite = cloth.icon;
            }

            
            else if (item is Inventory.SewingPart pattern)
            {
                GameObject spawnedCard = Instantiate(patternCardPrefub, patternContentBox);
                spawnedCard.transform.SetAsFirstSibling();
                SetCardPosition(spawnedCard, patternIndex++);

                SewPatternCard card = spawnedCard.GetComponent<SewPatternCard>();
                card.type=pattern.type;
                card.name = pattern.name;
                card.mesh = pattern.mesh;
                card.inventory = inventory;
                card.icon = pattern.icon;
                card.sewResulter = sewResulter;
                spawnedCard.transform.GetChild(0)
                    .GetComponent<UnityEngine.UI.Image>().sprite = pattern.icon;
            }

            
            else if (item is Inventory.SewingClothes clothes)
            {
                GameObject spawnedCard = Instantiate(clothesCardPrefub, clothesContentBox);
                spawnedCard.transform.SetAsFirstSibling();
                SetCardPosition(spawnedCard, clothesIndex++);

                SewClothesCard card = spawnedCard.GetComponent<SewClothesCard>();
                card.type = clothes.type;
                card.name = clothes.name;
                card.mesh1 = clothes.mesh1;
                card.mesh2 = clothes.mesh2;
                card.inventory = inventory;
                card.icon = clothes.icon;
                card.sewResulter=sewResulter;
                spawnedCard.transform.GetChild(0)
                    .GetComponent<UnityEngine.UI.Image>().sprite = clothes.icon;
            }
        }
    }
    void SetCardPosition(GameObject card, int index)
    {
        int x = index % 3;
        int y = index / 3;

        card.transform.localPosition = new Vector2(
            75 - 37.22f + x * 75,
            -50 - 75 * y
        );
    }
}
