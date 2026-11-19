using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraView : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    PlayerMovement PlayerMovement;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask boxLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerMovement=GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Cursor.visible = false;
        RaycastHit hitInfo;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hitInfo, 20f, interactableLayer))
        {
           GameObject hitObject=hitInfo.collider.gameObject;
            //if (hitObject.GetComponent<Outline>() != null)
            {
                Outline outline = hitObject.GetComponent<Outline>();
                outline.OutlineWidth = 10;

                MovePoint movePoint = hitObject.GetComponent<MovePoint>();
                movePoint.remove = false;
                if (Input.GetMouseButton(0))
                {

                    PlayerMovement.targetTransform = movePoint.targetTransform;
                }
            }
        }
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hitInfo, 20f, boxLayer))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            //if (hitObject.GetComponent<Outline>() != null)
            {
                Outline outline = hitObject.GetComponent<Outline>();
                outline.OutlineWidth = 10;

                Box box = hitObject.GetComponent<Box>();
                box.remove = false;
                if (Input.GetMouseButton(0))
                {
                    foreach(Inventory.SewingItem item in box.contents)
                    {
                        inventory.sewings.Add(item);
                        Destroy(box.gameObject);
                    }
                    
                }
            }
        }
    }
}
