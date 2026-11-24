using TMPro;
using UnityEngine;

public class Splash : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUI;
    UIShower uiShower;
    float timer=0;
    void Start()
    {
        uiShower = GetComponent<UIShower>();
    }

    void Update()
    {
        if(timer > 0)
        {
            uiShower.shown=true;
            timer-=Time.deltaTime;
            if(timer < 0) { timer = 0; }
        }
        else
        {
            uiShower.shown = false;
        }
    }
    public void CreateSplash(string text, float time)
    {
        timer = time;
        textUI.text = text;
    }
}
