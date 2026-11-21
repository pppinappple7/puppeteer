
using UnityEngine;
using UnityEngine.EventSystems;


public class SewClothesCard : MonoBehaviour
{
    public int price;
    public SewResulter sewResulter;
    public string type;
    public Mesh mesh1;
    public Mesh mesh2;
    public Inventory inventory;
    float doubleClickTimer = 0;
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
        Debug.Log("pressed");
        sewResulter.clothes = true;
        sewResulter.meshFilter1.mesh = mesh1;
        sewResulter.meshFilter2.mesh = mesh2;
        sewResulter.price = price;
        sewResulter.type = type;
        sewResulter.price = price;
        sewResulter.icon = icon;
    }
}
