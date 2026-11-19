using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class SewClothCard : MonoBehaviour, IPointerClickHandler
{

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
    }
}
