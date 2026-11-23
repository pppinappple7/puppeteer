using UnityEngine;

public class MovePoint : MonoBehaviour
{
    
    public Transform targetTransform;
    public bool remove;
    Outline Outline;
    
    void Start()
    {
        Outline=GetComponent<Outline>();
    }

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
