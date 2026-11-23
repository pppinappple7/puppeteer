using UnityEngine;
using UnityEngine.UI;

public class CombineSetUp : MonoBehaviour
{
    public MeshRenderer tagetmesh;
    public MeshFilter tagetFilter;
    public MeshRenderer tagetmesh1;
    public MeshFilter tagetFilter1;
    public MeshRenderer tagetmesh2;
    public MeshFilter tagetFilter2;
    public string type;

    [SerializeField] private GameObject partCardPrefub;
    [SerializeField] private Transform partContentBox;
    [SerializeField] private GameObject clothesCardPrefub;
    [SerializeField] private Transform clothesContentBox;
    [SerializeField] private Inventory inventory;

    private PartPicker activeSlot;

    public PartPicker ActiveSlot => activeSlot;

    public void SelectSlot(PartPicker slot)
    {
        if (slot == null)
            return;

        activeSlot = slot;
        type = slot.Type;

        tagetmesh = slot.MainRenderer;
        tagetFilter = slot.MainFilter;

        GameObject firstClothesObject = slot.ClothesObject1;
        GameObject secondClothesObject = slot.ClothesObject2;

        tagetmesh1 = firstClothesObject != null
            ? firstClothesObject.GetComponent<MeshRenderer>()
            : null;
        tagetFilter1 = firstClothesObject != null
            ? firstClothesObject.GetComponent<MeshFilter>()
            : null;
        tagetmesh2 = secondClothesObject != null
            ? secondClothesObject.GetComponent<MeshRenderer>()
            : null;
        tagetFilter2 = secondClothesObject != null
            ? secondClothesObject.GetComponent<MeshFilter>()
            : null;

        Refresh();
    }

    public void Refresh()
    {
        ClearContainer(partContentBox);
        ClearContainer(clothesContentBox);

        if (inventory == null || activeSlot == null)
            return;

        int partIndex = 0;
        int clothesIndex = 0;
        DollAssembly assembly = activeSlot.Assembly;

        foreach (Inventory.combining item in inventory.combiningInventory)
        {
            if (item is Inventory.Part part && part.type == type)
            {
                CreatePartCard(part, partIndex++, assembly);
            }
            else if (item is Inventory.Clothes clothes && clothes.type == type)
            {
                CreateClothesCard(clothes, clothesIndex++, assembly);
            }
        }
    }

    public bool TryInstallPart(Inventory.Part part)
    {
        return TryInstall(part);
    }

    public bool TryInstallClothes(Inventory.Clothes clothes)
    {
        return TryInstall(clothes);
    }

    private bool TryInstall(Inventory.combining item)
    {
        if (activeSlot == null)
        {
            Debug.LogWarning("CombineSetUp: no assembly slot is selected.", this);
            return false;
        }

        DollAssembly assembly = activeSlot.Assembly;
        if (assembly == null)
        {
            Debug.LogError("CombineSetUp: add DollAssembly to the root of the doll.", activeSlot);
            return false;
        }

        if (!assembly.TryInstall(inventory, activeSlot, item, out string error))
        {
            Debug.LogWarning("Could not install item: " + error, this);
            return false;
        }

        return true;
    }

    private void CreatePartCard(Inventory.Part part, int index, DollAssembly assembly)
    {
        if (partCardPrefub == null || partContentBox == null)
            return;

        GameObject spawnedCard = Instantiate(partCardPrefub, partContentBox);
        spawnedCard.transform.SetAsFirstSibling();
        SetCardPosition(spawnedCard, index);

        PartCard card = spawnedCard.GetComponent<PartCard>();
        if (card == null)
        {
            Debug.LogError("The part card prefab has no PartCard component.", spawnedCard);
            Destroy(spawnedCard);
            return;
        }

        card.Configure(part, this);
        card.SetAvailable(assembly != null && !assembly.IsInstalled(part));

        Image image = GetCardImage(spawnedCard);
        if (image != null)
        {
            image.sprite = part.icon;
            image.color = part.color;
        }
    }

    private void CreateClothesCard(Inventory.Clothes clothes, int index, DollAssembly assembly)
    {
        if (clothesCardPrefub == null || clothesContentBox == null)
            return;

        GameObject spawnedCard = Instantiate(clothesCardPrefub, clothesContentBox);
        spawnedCard.transform.SetAsFirstSibling();
        SetCardPosition(spawnedCard, index);

        ClothesCard card = spawnedCard.GetComponent<ClothesCard>();
        if (card == null)
        {
            Debug.LogError("The clothes card prefab has no ClothesCard component.", spawnedCard);
            Destroy(spawnedCard);
            return;
        }

        card.Configure(clothes, this);
        card.SetAvailable(assembly != null && !assembly.IsInstalled(clothes));

        Image image = GetCardImage(spawnedCard);
        if (image != null)
        {
            image.sprite = clothes.icon;
            image.color = clothes.color1;
        }
    }

    private Image GetCardImage(GameObject card)
    {
        if (card.transform.childCount == 0)
            return null;

        return card.transform.GetChild(0).GetComponent<Image>();
    }

    private void ClearContainer(Transform container)
    {
        if (container == null)
            return;

        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i).gameObject);
    }

    private void SetCardPosition(GameObject card, int index)
    {
        int x = index % 2;
        int y = index / 2;

        card.transform.localPosition = new Vector2(
            75 - 37.22f + x * 75,
            -50 - 75 * y
        );
    }
}
