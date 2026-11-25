using UnityEngine;
using UnityEngine.InputSystem;

public class SewClothMover : MonoBehaviour
{
    float constantZ;
    [SerializeField] Vector3 border1;
    [SerializeField] Vector3 border2;
    void Start()
    {
        constantZ=transform.localPosition.z;
    }

    void Update()
    {
        float mouseX=Input.mousePosition.x/Screen.width;
        float mouseY = Input.mousePosition.y / Screen.height;
        transform.localPosition = border1 + new Vector3((1-mouseX) * (border2.x - border1.x), (1-mouseY )* (border2.y - border1.y), 1.241f);
        transform.localPosition=new Vector3(transform.localPosition.x,transform.localPosition.y, constantZ);
    }
}
