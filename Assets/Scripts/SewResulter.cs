using UnityEngine;

public class SewResulter : MonoBehaviour
{
    public int price;
    public int price1;
    public int price2;
    public Sprite icon;
    [SerializeField] Inventory inventory;
    [SerializeField] SewSetUp SewSetUp;
    public string type;
    [SerializeField] GameObject secondColorPanel;
    [SerializeField] GameObject model1;
    [SerializeField] GameObject model2;
    public MeshRenderer renderer1;
    public MeshRenderer renderer2;
    public MeshFilter meshFilter1;
    public MeshFilter meshFilter2;
    public bool clothes=false;
    void Start()
    {
        renderer1=model1.GetComponent<MeshRenderer>();
        renderer2=model2.GetComponent<MeshRenderer>();
        meshFilter1=model1.GetComponent<MeshFilter>();
        meshFilter2=model2.GetComponent<MeshFilter>();
        meshFilter1.mesh = null;
        meshFilter2.mesh = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (clothes)
        {
            secondColorPanel.SetActive(true);
            model2.SetActive(true);
        }
        else
        {
            secondColorPanel.SetActive(false);
            model2.SetActive(false);
        }
    }
    public void Sew()
    {
        if (clothes)
        {
            if(meshFilter1.mesh != null && meshFilter2.mesh!=null && renderer1.material.color!=Color.black && renderer2.material.color != Color.black)
            {
                inventory.combiningInventory.Add(new Inventory.Clothes()
                {
                    mesh1 = meshFilter1.mesh,
                    mesh2 = meshFilter2.mesh,
                    color1 = renderer1.material.color,
                    color2 = renderer2.material.color,
                    type = type,
                    icon = icon,
                    price = price1 + price2 + price
                });
            }
        }
        else
        {
            if (meshFilter1.mesh != null && renderer1.material.color != Color.black)
            {
                inventory.combiningInventory.Add(new Inventory.Part()
                {
                    mesh = meshFilter1.mesh,
                    color = renderer1.material.color,
                    type = type,
                    icon = icon,
                    price = price1 + price
                });
            }
        }
    }
}
