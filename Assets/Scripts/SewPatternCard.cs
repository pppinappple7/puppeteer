
using UnityEngine;
using UnityEngine.EventSystems;


public class SewPatternCard : MonoBehaviour
{
    public int price;
    public SewResulter sewResulter;
    public string type;
    public Mesh mesh;
    public Inventory inventory;
    float doubleClickTimer=0;
    public Sprite icon;
    public string name;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    
    public void Selected()
    {
        sewResulter.meshFilter1.mesh = mesh;
        sewResulter.clothes = false;
        sewResulter.type = type;
        sewResulter.icon= icon;
        sewResulter.price = price;
    }
}
