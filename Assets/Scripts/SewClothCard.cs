using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SewClothCard : MonoBehaviour, IPointerClickHandler
{
    public bool accent;
    public SewResulter sewResulter;
    public int price;
    public MeshRenderer targetMesh;
    public Color color;
    public Inventory inventory;
    float doubleClickTimer=0;
    public Sprite icon;
    public string name;
    
    [SerializeField] TextMeshProUGUI ammountText;
    public int ammount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ammountText.text=ammount.ToString();
        
        
        if (doubleClickTimer > -10)
        {
            doubleClickTimer -= Time.deltaTime;
        }
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
        }
           
    }
    public void Select()
    {
        targetMesh.material.color= color;
        Debug.Log(price);
        if (!accent)
        {
            sewResulter.price1= price;
        }
        else
        {
            sewResulter.price2 = price;
        }
    }
}
