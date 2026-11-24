using UnityEngine;

public class UIShower : MonoBehaviour
{
    [SerializeField] float speed=6;
    CanvasGroup canvasGroup;
    public bool shown;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    
    void Update()
    {
        if (shown)
        {
            if (canvasGroup.interactable)
            {
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha += speed * Time.deltaTime;
            }

                canvasGroup.interactable=true;
        }
        else
        {
            if (!canvasGroup.interactable)
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha -= speed * Time.deltaTime;
            }
                
            canvasGroup.interactable = false;
        }
    }
}
