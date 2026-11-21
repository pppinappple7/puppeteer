using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject boxPrefab;
    public GameObject moneyCounter;
    TextMeshProUGUI moneyText;
    [SerializeField] ShopCheck check;
    public int money;
    [System.Serializable] public abstract class combining { }
    [System.Serializable]
    public class Part:combining
    {
        public int price;
        public Mesh mesh;
        public Color color;
        public string type;
        public Sprite icon;
    }
    [System.Serializable]
    public class Clothes: combining
    {
        public int price;
        public Mesh mesh1;
        public Mesh mesh2;
        public Color color1;
        public Color color2;
        public string type;
        public Sprite icon;
    }

    #region sewing classes
    [System.Serializable]
    public class SewingItem
    {
        public Sprite icon;
        public string name;
    }
    [System.Serializable]
    public class SewingCloth : SewingItem
    {
        public int price;
        public int ammount;
        public Color color;
    }
    [System.Serializable]
    public class SewingPart : SewingItem
    {
        public int price;
        public string type;
        public Mesh mesh;
    }
    [System.Serializable]
    public class SewingClothes : SewingItem
    {
        public int price;
        public string type;
        public Sprite icon2;
        public Mesh mesh1;
        public Mesh mesh2;
    }
    #endregion

    #region shopClasses
    [System.Serializable]
    public class ShopItem
    {
        
        public Sprite icon;
        public int ammount;
        public string name;
        public int price;
    }
    [System.Serializable]
    public class ShopCloth:ShopItem
    {
        
        public Color color;
    }
    [System.Serializable]
    public class ShopPart : ShopItem
    {
        public string type;
        public Mesh mesh;
    }
    [System.Serializable]
    public class ShopClothes : ShopItem
    {
        public Mesh mesh1;
        public Mesh mesh2;
        public string type;
    }
    #endregion
    [SerializeReference] public List<ShopItem> cart = new List<ShopItem>();

    [SerializeReference] public List<SewingItem> sewings = new List<SewingItem>();

    [SerializeReference] public List<combining> combiningInventory= new List<combining>();
    void Start()
    {
        moneyText=moneyCounter.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text=money.ToString();
    }
    public void Buy()
    {
        if (cart.Count > 0)
        {
            
            if (money > check.total)
            {
                moneyCounter.transform.localScale *= 2;
                money -= check.total;
                Box box=Instantiate(boxPrefab).GetComponent<Box>();
                foreach (ShopItem item in cart)
                {
                    if (item is ShopCloth cloth)
                    {
                        box.contents.Add(new SewingCloth
                        {
                            price=cloth.price,
                            ammount= cloth.ammount,
                            name = cloth.name,
                            icon = cloth.icon,
                            color = cloth.color
                        });
                    }
                    else if (item is ShopPart part)
                    {
                        box.contents.Add(new SewingPart
                        {
                            price=part.price,
                            name = part.name,
                            icon = part.icon,
                            mesh = part.mesh,
                            type=part.type
                            
                        });
                        Debug.Log(part.type);
                    }
                    else if (item is ShopClothes clothes)
                    {
                        box.contents.Add(new SewingClothes
                        {
                            price = clothes.price,
                            name = clothes.name,
                            icon = clothes.icon,
                            mesh1 = clothes.mesh1,
                            mesh2 = clothes.mesh2,
                            type = clothes.type
                        });
                    }
                }
                foreach (ShopClothesCard clothesCard in GameObject.FindObjectsByType<ShopClothesCard>(FindObjectsSortMode.None))
                {
                    clothesCard.countObj.SetActive(false);
                }
                foreach (ShopPatternCard patternCard in GameObject.FindObjectsByType<ShopPatternCard>(FindObjectsSortMode.None))
                {
                    patternCard.countObj.SetActive(false);
                }
                foreach (ShopClothCard clothCard in GameObject.FindObjectsByType<ShopClothCard>(FindObjectsSortMode.None))
                {
                    clothCard.countObj.SetActive(false);
                }
                cart.Clear();
                
                playerMovement.targetTransform = null;
            }
            else
            {
                moneyCounter.GetComponent<Money>().NotEnough();
            }
        }
        
    }
}
