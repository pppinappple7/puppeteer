using UnityEngine;
using UnityEngine.Events;

public class ShowUIInZone : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UIZone"))
        {
            Debug.Log("Inv");
            other.GetComponent<UIZone>().unityEvent?.Invoke();
            if (other.GetComponent<UIZone>().UIObject != null)
            {
                other.GetComponent<UIZone>().UIObject.GetComponent<UIShower>().shown = true;
            }
                
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UIZone") && other.GetComponent<UIZone>().UIObject!=null)
        {
            other.GetComponent<UIZone>().UIObject.GetComponent<UIShower>().shown = false;
        }
    }
}
