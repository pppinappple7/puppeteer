
using TMPro;
using UnityEngine;


public class PartCard : MonoBehaviour
{
    public int price;
    public string type;
    public Mesh mesh;
    public Color color;
    public CombineSetUp combineSetUp;
    public Inventory inventory;
    public Sprite icon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    
    public void Select()
    {
        combineSetUp.tagetmesh.material.color = color;
        combineSetUp.tagetFilter.mesh = mesh;
        combineSetUp.tagetmesh.GetComponent<PartPicker>().price = price;
    }
}
