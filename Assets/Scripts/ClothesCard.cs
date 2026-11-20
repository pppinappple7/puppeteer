using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class ClothesCard : MonoBehaviour, IPointerClickHandler
{
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
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
        }
           
    }
    public void Select()
    {
        
    }
}
