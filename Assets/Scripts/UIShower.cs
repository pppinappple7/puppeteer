using UnityEngine;

public class UIShower : MonoBehaviour
{
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
                canvasGroup.alpha += 6 * Time.deltaTime;
            }

                canvasGroup.interactable=true;
        }
        else
        {
            if (!canvasGroup.interactable)
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha -= 6 * Time.deltaTime;
            }
                
            canvasGroup.interactable = false;
        }
    }
}
