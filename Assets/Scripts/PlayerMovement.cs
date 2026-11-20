using EasyPeasyFirstPersonController;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform camTransform;
    FirstPersonController FPSCon;
    [SerializeField] GameObject backButton;
    CameraView cameraView;
    public Transform targetTransform;
    [SerializeField] float positionSmoothTime = 0.3f;
    float cameraRotationVelocity = 5;
    [SerializeField] float rotationSpeed = 5f;
    Vector3 positionVelocity=Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        cameraView = GetComponent<CameraView>();
        FPSCon = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
        if(targetTransform != null)
        {
            
            FPSCon.enabled=false;
            cameraView.enabled = false;
            backButton.SetActive(true);
            MoveToPoint();
            Cursor.visible = true;
        }
        else
        {

            FPSCon.enabled = true;
            transform.position = Vector3.SmoothDamp(
                transform.position,
                Vector3.zero,
                ref positionVelocity,
                positionSmoothTime
            );
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0, transform.eulerAngles.y, 0),
                rotationSpeed * Time.deltaTime
            );
            Cursor.visible = false;
            
            targetTransform =null;
            cameraView.enabled = true;
            backButton.SetActive(false);
        }
        
    }
    public void MoveToPoint()
    {
        Vector3 camAngles = camTransform.localEulerAngles;

        camAngles.x = Mathf.SmoothDampAngle(
            camAngles.x,
            targetTransform.eulerAngles.x,
            ref cameraRotationVelocity,
            positionSmoothTime
        );

        camTransform.localEulerAngles = camAngles;
        transform.position = Vector3.SmoothDamp
            (
                transform.position,
                targetTransform.position,
                ref positionVelocity,
                positionSmoothTime
            );
        transform.rotation = Quaternion.Slerp
            (
                transform.rotation,
                targetTransform.rotation,
                rotationSpeed * Time.deltaTime
            );
    }
    public void GoBack()
    {
        targetTransform = null;

    }
}
