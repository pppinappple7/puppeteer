using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.UnifiedRayTracing;

public class InteractiveSewing : MonoBehaviour
{
    [SerializeField] UIShower UIShower;
    [SerializeField] UIShower BackShower;
    [SerializeField] GameObject clothParent;
    [SerializeField] UnityEvent addToInventoryEvent;
    [SerializeField] SewingLine sewingLine;
    public GameObject targetPoint;
    [SerializeField] GameObject[] sewPoints;
    [SerializeField] GameObject cloth;

    private void OnEnable()
    {
        targetPoint=sewPoints[0];
        clothParent.SetActive(true);
        UIShower.shown = false;
        BackShower.shown = false;
        foreach (GameObject point in sewPoints)
            point.SetActive(true);

        sewingLine.Refresh();
    }

    

    void Start()
    {
        sewingLine.Refresh();
    }

    public void Refresh()
    {

        sewingLine.Refresh();
        foreach (GameObject point in sewPoints)
        {
            if (point.activeSelf) { targetPoint= point; return; }
        }
        addToInventoryEvent.Invoke();
        clothParent.SetActive(false);
        UIShower.shown = true;
        BackShower.shown = true;
        enabled = false;
    }
}