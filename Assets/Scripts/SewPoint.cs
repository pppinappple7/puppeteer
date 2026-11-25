using UnityEngine;

public class SewPoint : MonoBehaviour
{
    [SerializeField] InteractiveSewing interactiveSewing;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("needle"))
        {
            if (gameObject == interactiveSewing.targetPoint)
            {
                gameObject.SetActive(false);

                interactiveSewing.Refresh();
            }
        }
    }
}
