
using UnityEngine;


public class CombineSetUp : MonoBehaviour
{ 
    int x = 0;
    int y = 0;
    public MeshRenderer tagetmesh;
    public MeshFilter tagetFilter;
    public MeshRenderer tagetmesh1;
    public MeshFilter tagetFilter1;
    public MeshRenderer tagetmesh2;
    public MeshFilter tagetFilter2;
    public string type;
    [SerializeField] GameObject partCardPrefub;
    [SerializeField] Transform partContentBox;
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
    
   
    
    void Start()
    {

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Refresh()
    {
        
        
        foreach (Transform child in partContentBox)
            Destroy(child.gameObject);

        foreach (Transform child in clothesContentBox)
            Destroy(child.gameObject);

        
        
        int partIndex = 0;
        int clothesIndex = 0;

        foreach (Inventory.combining item in inventory.combiningInventory)
        {
            
            if (item is Inventory.Part part && part.type==type)
            {
                GameObject spawnedCard = Instantiate(partCardPrefub, partContentBox);
                spawnedCard.transform.SetAsFirstSibling();
                SetCardPosition(spawnedCard, partIndex++);

                PartCard card = spawnedCard.GetComponent<PartCard>();
                card.combineSetUp = GetComponent<CombineSetUp>();
                card.type=part.type;
                card.mesh = part.mesh;
                card.inventory = inventory;
                card.icon = part.icon;
                card.color=part.color;
                card.price=part.price;
                spawnedCard.transform.GetChild(0)
                    .GetComponent<UnityEngine.UI.Image>().sprite = part.icon;
                spawnedCard.transform.GetChild(0)
                    .GetComponent<UnityEngine.UI.Image>().color = part.color;
            }

            
            else if (item is Inventory.Clothes clothes && clothes.type == type)
            {
                GameObject spawnedCard = Instantiate(clothesCardPrefub, clothesContentBox);
                spawnedCard.transform.SetAsFirstSibling();
                SetCardPosition(spawnedCard, clothesIndex++);

                ClothesCard card = spawnedCard.GetComponent<ClothesCard>();
                card.price = clothes.price;
                card.combineSetUp = GetComponent<CombineSetUp>();
                card.type = clothes.type;
                card.mesh1 = clothes.mesh1;
                card.mesh2 = clothes.mesh2;
                card.color1 = clothes.color1;
                card.color2 = clothes.color2;
                card.inventory = inventory;
                card.icon = clothes.icon;
                spawnedCard.transform.GetChild(0)
                    .GetComponent<UnityEngine.UI.Image>().sprite = clothes.icon;
                spawnedCard.transform.GetChild(0)
                    .GetComponent<UnityEngine.UI.Image>().color = clothes.color1;
            }
        }
    }
    void SetCardPosition(GameObject card, int index)
    {
        int x = index % 2;
        int y = index / 2;

        card.transform.localPosition = new Vector2(
            75 - 37.22f + x * 75,
            -50 - 75 * y
        );
    }
}
