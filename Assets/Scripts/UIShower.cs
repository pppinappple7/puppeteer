using UnityEngine;

public class UIShower : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public bool shown;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shown)
        {
            canvasGroup.blocksRaycasts=true;
            canvasGroup.alpha +=6* Time.deltaTime;
            canvasGroup.interactable=true;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha -=6* Time.deltaTime;
            canvasGroup.interactable = false;
        }
    }
}
