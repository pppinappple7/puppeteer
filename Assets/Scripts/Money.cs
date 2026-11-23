using SmoothShakeFree;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Money : MonoBehaviour
{
    Color defColor;
    Vector2 defScale;
    SmoothShake shake;
    TextMeshProUGUI text;
    
    void Start()
    {
        
        defScale=transform.localScale;
        text =gameObject. GetComponent<TextMeshProUGUI>();
        shake =gameObject. GetComponent<SmoothShake>();
        defColor = text.color; 
    }

    void Update()
    {
        if (text != null)
        {
            text.color = Color.Lerp(
                text.color,
                defColor,
                Time.deltaTime * 3);
        }
        
        if (transform.localScale.x > defScale.x)
        {
            transform.localScale-=Vector3.one*3*Time.deltaTime;
        }
        else if(transform.localScale.x < defScale.x)
        {
            transform.localScale = defScale;
        }
    }
    public void NotEnough()
    {
        text.color = Color.red;
        shake.StartShake();
    }
}
