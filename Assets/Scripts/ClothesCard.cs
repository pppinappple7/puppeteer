
using UnityEngine;
using UnityEngine.EventSystems;


public class ClothesCard : MonoBehaviour
{
    public int price;
    public string type;
    public Mesh mesh1;
    public Mesh mesh2;
    public Color color1;
    public Color color2;
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
        combineSetUp.tagetmesh1.material.color = color1;
        combineSetUp.tagetFilter1.mesh = mesh1;
        combineSetUp.tagetmesh1.transform.parent.GetComponent<Pricer>().price = price;
        combineSetUp.tagetmesh2.material.color = color2;
        combineSetUp.tagetFilter2.mesh = mesh2;
    }
}
