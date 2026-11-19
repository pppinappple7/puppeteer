using UnityEngine;
using UnityEngine.Events;

public class ShowUIInZone : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UIZone"))
        {
            other.GetComponent<UIZone>().unityEvent?.Invoke();
            other.GetComponent<UIZone>().UIObject.GetComponent<UIShower>().shown = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UIZone"))
        {
            other.GetComponent<UIZone>().UIObject.GetComponent<UIShower>().shown = false;
        }
    }
}
