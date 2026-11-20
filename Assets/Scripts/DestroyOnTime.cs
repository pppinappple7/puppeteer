using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    [SerializeField] float time;
    float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > time)
        {
            Destroy(gameObject);
        }
    }
}
