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

    [System.Serializable]
    public abstract class combining
    {
        public string itemId;
    }

    [System.Serializable]
    public class Part : combining
    {
        public int price;
        public Mesh mesh;
        public Color color;
        public string type;
        public Sprite icon;
    }

    [System.Serializable]
    public class Clothes : combining
    {
        public int price;
        public Mesh mesh1;
        public Mesh mesh2;
        public Color color1;
        public Color color2;
        public string type;
        public Sprite icon;
    }

    [System.Serializable]
    public class SewingItem
    {
        public string itemId;
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

    [System.Serializable]
    public class ShopItem
    {
        public string itemId;
        public Sprite icon;
        public int ammount;
        public string name;
        public int price;
    }

    [System.Serializable]
    public class ShopCloth : ShopItem
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

    [SerializeReference] public List<ShopItem> cart = new List<ShopItem>();
    [SerializeReference] public List<SewingItem> sewings = new List<SewingItem>();
    [SerializeReference] public List<combining> combiningInventory = new List<combining>();

    internal GameObject BoxPrefab => boxPrefab;
    internal ShopCheck ShopCheck => check;
    internal PlayerMovement PlayerMovement => playerMovement;

    void Start()
    {
        if (moneyCounter != null)
            moneyText = moneyCounter.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }

    public void Buy()
    {
        ShopPurchaseService.TryBuy(this);
    }
}
