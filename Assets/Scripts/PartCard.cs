
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class PartCard : MonoBehaviour, IPointerClickHandler
{
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
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
        }
           
    }
    public void Select()
    {
        combineSetUp.tagetmesh.material.color = color;
        combineSetUp.tagetFilter.mesh = mesh;
    }
}
