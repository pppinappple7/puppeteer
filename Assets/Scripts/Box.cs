using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject dust;
    public List<Inventory.SewingItem> contents=new List<Inventory.SewingItem>();
    Outline outline;
    public bool remove;
    bool shot;
    float initialTimer = 0;
    [SerializeField] float minImpulse;
    [SerializeField] float maxImpulse;
    Rigidbody rigidbody;
    [SerializeField] GameObject visuals;
    
    void Start()
    {
        
        outline = GetComponent<Outline>();
        Outline Outline;
        visuals.SetActive(false);
        rigidbody=GetComponent<Rigidbody>();
        rigidbody.constraints=RigidbodyConstraints.FreezeAll;
    }

    void Update()
    {
        if (!shot)
        {
            initialTimer += Time.deltaTime;
            if (initialTimer > 2)
            {
                rigidbody.constraints = RigidbodyConstraints.None;
                visuals.SetActive(true);
                rigidbody.AddForce(Vector3.right * Random.Range(minImpulse, maxImpulse), ForceMode.Impulse);
                shot = true;
            }
        }
        if (remove)
        {
            outline.OutlineWidth = 0;
        }
        if (outline.OutlineWidth == 10)
        {
            remove = true;
        }
        else
        {
            remove = false;
        }
    }
    private void OnDestroy()
    {
        Instantiate(dust,transform.position,dust.transform.rotation);
    }
}
