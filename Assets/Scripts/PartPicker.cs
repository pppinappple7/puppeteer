using UnityEngine;
using UnityEngine.Serialization;

public class PartPicker : MonoBehaviour
{
    [SerializeField] private UIShower uI;
    [FormerlySerializedAs("CombineSetUp")]
    [SerializeField] private CombineSetUp combineSetUp;
    [SerializeField] private string type;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject clothes1;
    [SerializeField] private GameObject clothes2;

    public int price;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Outline outline;

    private MeshRenderer clothesRenderer1;
    private MeshRenderer clothesRenderer2;
    private MeshFilter clothesFilter1;
    private MeshFilter clothesFilter2;
    private Pricer clothesPricer;

    private bool originalVisualsCaptured;
    private Mesh originalPartMesh;
    private Color originalPartColor;
    private int originalPartPrice;
    private Mesh originalClothesMesh1;
    private Mesh originalClothesMesh2;
    private Color originalClothesColor1;
    private Color originalClothesColor2;
    private int originalClothesPrice;

    public string Type => type;
    public MeshRenderer MainRenderer => meshRenderer;
    public MeshFilter MainFilter => meshFilter;
    public GameObject ClothesObject1 => clothes1;
    public GameObject ClothesObject2 => clothes2;
    public DollAssembly Assembly => GetComponentInParent<DollAssembly>();

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        outline = GetComponent<Outline>();

        if (clothes1 != null)
        {
            clothesRenderer1 = clothes1.GetComponent<MeshRenderer>();
            clothesFilter1 = clothes1.GetComponent<MeshFilter>();
            if (clothes1.transform.parent != null)
                clothesPricer = clothes1.transform.parent.GetComponent<Pricer>();
        }

        if (clothes2 != null)
        {
            clothesRenderer2 = clothes2.GetComponent<MeshRenderer>();
            clothesFilter2 = clothes2.GetComponent<MeshFilter>();
        }
    }

    private void Update()
    {
        if (uI != null && !uI.shown)
        {
            SetOutline(0f, Color.white);
            return;
        }

        if (combineSetUp == null || Camera.main == null)
            return;

        if (combineSetUp.ActiveSlot == this)
        {
            SetOutline(10f, Color.yellow);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask) &&
            hit.collider.gameObject == gameObject)
        {
            SetOutline(5f, Color.white);

            if (Input.GetMouseButtonDown(0))
                combineSetUp.SelectSlot(this);
        }
        else
        {
            SetOutline(0f, Color.white);
        }
    }

    public bool CanInstall(Inventory.combining item, out string error)
    {
        error = null;

        if (item == null)
        {
            error = "The selected item is missing.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(type))
        {
            error = "The selected slot has no type configured.";
            return false;
        }

        if (item is Inventory.Part part)
        {
            if (part.type != type)
            {
                error = "This part is not compatible with the selected slot.";
                return false;
            }

            if (part.mesh == null)
            {
                error = "The selected part has no mesh.";
                return false;
            }

            if (meshRenderer == null || meshFilter == null)
            {
                error = "The selected slot needs a MeshRenderer and a MeshFilter.";
                return false;
            }

            return true;
        }

        if (item is Inventory.Clothes clothes)
        {
            if (clothes.type != type)
            {
                error = "This clothing item is not compatible with the selected slot.";
                return false;
            }

            if (clothes.mesh1 == null || clothes.mesh2 == null)
            {
                error = "The selected clothing item has a missing mesh.";
                return false;
            }

            if (clothesRenderer1 == null || clothesRenderer2 == null ||
                clothesFilter1 == null || clothesFilter2 == null)
            {
                error = "The selected slot needs two configured clothing objects with MeshRenderers and MeshFilters.";
                return false;
            }

            if (clothesPricer == null)
            {
                error = "The clothing slot needs a Pricer component on the parent of its first clothing object.";
                return false;
            }

            return true;
        }

        error = "The selected inventory item has an unsupported type.";
        return false;
    }

    public void CaptureOriginalVisuals()
    {
        if (originalVisualsCaptured)
            return;

        if (meshFilter != null)
            originalPartMesh = meshFilter.sharedMesh;

        if (meshRenderer != null)
            originalPartColor = meshRenderer.material.color;

        originalPartPrice = price;

        if (clothesFilter1 != null)
            originalClothesMesh1 = clothesFilter1.sharedMesh;

        if (clothesFilter2 != null)
            originalClothesMesh2 = clothesFilter2.sharedMesh;

        if (clothesRenderer1 != null)
            originalClothesColor1 = clothesRenderer1.material.color;

        if (clothesRenderer2 != null)
            originalClothesColor2 = clothesRenderer2.material.color;

        if (clothesPricer != null)
            originalClothesPrice = clothesPricer.price;

        originalVisualsCaptured = true;
    }

    public void RestoreOriginalVisuals()
    {
        if (!originalVisualsCaptured)
            return;

        if (meshFilter != null)
            meshFilter.sharedMesh = originalPartMesh;

        if (meshRenderer != null)
            meshRenderer.material.color = originalPartColor;

        price = originalPartPrice;

        if (clothesFilter1 != null)
            clothesFilter1.sharedMesh = originalClothesMesh1;

        if (clothesFilter2 != null)
            clothesFilter2.sharedMesh = originalClothesMesh2;

        if (clothesRenderer1 != null)
            clothesRenderer1.material.color = originalClothesColor1;

        if (clothesRenderer2 != null)
            clothesRenderer2.material.color = originalClothesColor2;

        if (clothesPricer != null)
            clothesPricer.price = originalClothesPrice;
    }

    public bool ApplyItem(Inventory.combining item, out string error)
    {
        if (!CanInstall(item, out error))
            return false;

        if (item is Inventory.Part part)
        {
            meshFilter.mesh = part.mesh;
            meshRenderer.material.color = part.color;
            price = part.price;

            if (clothesPricer != null)
                clothesPricer.price = 0;

            return true;
        }

        Inventory.Clothes clothes = item as Inventory.Clothes;
        clothesFilter1.mesh = clothes.mesh1;
        clothesRenderer1.material.color = clothes.color1;
        clothesFilter2.mesh = clothes.mesh2;
        clothesRenderer2.material.color = clothes.color2;

        price = 0;
        clothesPricer.price = clothes.price;
        return true;
    }

    private void SetOutline(float width, Color color)
    {
        if (outline == null)
            return;

        outline.OutlineWidth = width;
        outline.OutlineColor = color;
    }
}
