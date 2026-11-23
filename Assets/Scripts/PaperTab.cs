using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PaperTab : MonoBehaviour
{
    UnityEngine.UI.Image image;
    [SerializeField] Sprite openSprite;
    [SerializeField] Sprite closedSprite;
    [SerializeField] GameObject contents;
    [SerializeField] PaperTab[] others;
    public bool open;
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        if (open)
        {
            contents.SetActive(true);
            //image.sprite = openSprite;
        }
        else
        {
            contents.SetActive(false);
            //image.sprite = closedSprite;
        }
    }
    public void Open()
    {
        foreach (PaperTab tab in others) 
        {
            tab.open = false;
        }
        open = true;
        
    }
}
