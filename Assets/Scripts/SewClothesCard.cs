using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class SewClothesCard : MonoBehaviour, IPointerClickHandler
{
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
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
        }

    }
    public void Selected()
    {
        sewResulter.meshFilter1.mesh = mesh1;
        sewResulter.meshFilter2.mesh = mesh2;
        sewResulter.clothes = true;
        sewResulter.type = type;
    }
}
