using UnityEngine;

public class PartPicker : MonoBehaviour
{
    [SerializeField] UIShower uI;
    MeshRenderer meshRenserer;
    Outline outline;
    [SerializeField] CombineSetUp CombineSetUp;
    [SerializeField] string type;
    [SerializeField] LayerMask layerMask;
    void Start()
    {
        outline = GetComponent<Outline>();
        meshRenserer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (uI.shown == false)
        {
            outline.OutlineWidth = 0;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (CombineSetUp.tagetmesh==meshRenserer && CombineSetUp.type!=null)
        {
            outline.OutlineWidth = 10;
            outline.OutlineColor = Color.yellow;
        }

        else if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {

            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CombineSetUp.tagetmesh=GetComponent<MeshRenderer>();
                    CombineSetUp.tagetFilter=GetComponent<MeshFilter>();
                    CombineSetUp.type = type;
                    CombineSetUp.Refresh();
                }
                outline.OutlineWidth = 5;
                outline.OutlineColor = Color.white;
            }
            else
            {
                outline.OutlineWidth = 0;
            }
        }
        else
        {
            outline.OutlineWidth = 0;
        }
    }
    
}
