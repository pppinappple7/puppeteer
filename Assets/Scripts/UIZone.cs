using UnityEngine;

public class UIZone : MonoBehaviour
{
    public int remove;
    [SerializeField] GameObject UIObject;
    private void Update()
    {
        if (remove == 2)
        {
            UIObject.SetActive(false);
        }
        else
        {
            UIObject.SetActive(true);
        }
        if (remove == 1)
        {
            remove = 2;
        }
        remove = 1;
    }
}
