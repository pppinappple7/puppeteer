using SmoothShakeFree;
using UnityEngine;

public class Remover : MonoBehaviour
{
    Rigidbody rigidbody;
    float deleteTimer = 0;
    Outline outline;
    SmoothShake shake;

    public bool removed = false;

    float shrinkTimer = 0;
    Vector3 startScale;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        shake = GetComponent<SmoothShake>();
        startScale = transform.localScale;
    }

    void Update()
    {
        if (removed)
        {
            shrinkTimer += Time.deltaTime;

            float t = Mathf.Clamp01(shrinkTimer / 0.5f);
            float scale = 1 - Mathf.Pow(t, 2);

            transform.localScale = startScale * scale;

            if (t >= 1)
                Destroy(gameObject);

            return;
        }

        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    deleteTimer += Time.deltaTime;
                    rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                    outline.OutlineWidth += Time.deltaTime * 5f;
                    shake.rotationShake.amplitude += Vector3.one * Time.deltaTime * 7 * 0.5f;

                    if (deleteTimer >= 2)
                        removed = true;
                }
            }
        }
        else
        {
            deleteTimer = 0;
            outline.OutlineWidth = 0;
            shake.rotationShake.amplitude = Vector3.zero;
        }
    }
}