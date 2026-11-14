using UnityEngine;

public class MovePoint : MonoBehaviour
{
    
    public Transform targetTransform;
    public bool remove;
    Outline Outline;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Outline=GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (remove)
        {
            Outline.OutlineWidth = 0;
        }
        if(Outline.OutlineWidth == 10)
        {
            remove = true;
        }
        else
        {
            remove = false;
        }
    }
}
