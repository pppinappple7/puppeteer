using UnityEngine;

public class ShowUIInZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("UIZone"))
        {
            other.GetComponent<UIZone>().remove = 0;
        }
    }
}
