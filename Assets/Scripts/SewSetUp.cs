using UnityEngine;
using UnityEngine.UI;

public class SewSetUp : MonoBehaviour
{
    [SerializeField] private SewResulter sewResulter;

    [SerializeField] private GameObject clothCardPrefub;
    [SerializeField] private Transform clothContentBox;
    [SerializeField] private Transform clothContentBox2;

    [SerializeField] private GameObject patternCardPrefub;
    [SerializeField] private Transform patternContentBox;

    [SerializeField] private GameObject clothesCardPrefub;
    [SerializeField] private Transform clothesContentBox;

    [SerializeField] private Inventory inventory;

    public void Refresh()
    {
        if (inventory == null)
        {
            return;
        }

        ClearContainer(clothContentBox);
        ClearContainer(clothContentBox2);
        ClearContainer(patternContentBox);
        ClearContainer(clothesContentBox);

        int clothIndex = 0;
        int clothIndex2 = 0;
        int patternIndex = 0;
        int clothesIndex = 0;

        foreach (Inventory.SewingItem item in inventory.sewings)
        {
            if (item is Inventory.SewingCloth cloth)
            {
                CreateClothCard(
                    cloth,
                    clothContentBox,
                    sewResulter != null ? sewResulter.renderer1 : null,
                    false,
                    clothIndex++
                );

                CreateClothCard(
                    cloth,
                    clothContentBox2,
                    sewResulter != null ? sewResulter.renderer2 : null,
                    true,
                    clothIndex2++
                );
            }
            else if (item is Inventory.SewingPart pattern)
            {
                CreatePatternCard(pattern, patternIndex++);
            }
            else if (item is Inventory.SewingClothes clothes)
            {
                CreateClothesCard(clothes, clothesIndex++);
            }
        }
    }

    private void CreateClothCard(
        Inventory.SewingCloth cloth,
        Transform container,
        MeshRenderer target,
        bool accent,
        int index)
    {
        if (cloth.ammount <= 0 || container == null || clothCardPrefub == null)
            return;

        GameObject spawnedCard = Instantiate(clothCardPrefub, container);
        spawnedCard.transform.SetAsFirstSibling();
        SetCardPosition(spawnedCard, index);

        SewClothCard card = spawnedCard.GetComponent<SewClothCard>();

        if (card == null)
        {
            Destroy(spawnedCard);
            return;
        }

        card.accent = accent;
        card.price = cloth.price;
        card.name = cloth.name;
        card.color = cloth.color;
        card.inventory = inventory;
        card.icon = cloth.icon;
        card.ammount = cloth.ammount;
        card.targetMesh = target;
        card.sewResulter = sewResulter;
        card.sewingCloth = cloth;

        card.UpdateAmountText();
        SetCardIcon(spawnedCard, cloth.icon);
    }

    private void CreatePatternCard(Inventory.SewingPart pattern, int index)
    {
        if (patternCardPrefub == null || patternContentBox == null)
            return;

        GameObject spawnedCard = Instantiate(patternCardPrefub, patternContentBox);
        spawnedCard.transform.SetAsFirstSibling();
        SetCardPosition(spawnedCard, index);

        SewPatternCard card = spawnedCard.GetComponent<SewPatternCard>();

        if (card == null)
        {
            Destroy(spawnedCard);
            return;
        }

        card.type = pattern.type;
        card.name = pattern.name;
        card.mesh = pattern.mesh;
        card.inventory = inventory;
        card.icon = pattern.icon;
        card.sewResulter = sewResulter;
        card.price = pattern.price;
        card.sewingPart = pattern;

        SetCardIcon(spawnedCard, pattern.icon);
    }

    private void CreateClothesCard(Inventory.SewingClothes clothes, int index)
    {
        if (clothesCardPrefub == null || clothesContentBox == null)
            return;

        GameObject spawnedCard = Instantiate(clothesCardPrefub, clothesContentBox);
        spawnedCard.transform.SetAsFirstSibling();
        SetCardPosition(spawnedCard, index);

        SewClothesCard card = spawnedCard.GetComponent<SewClothesCard>();

        if (card == null)
        {
            Destroy(spawnedCard);
            return;
        }

        card.type = clothes.type;
        card.name = clothes.name;
        card.mesh1 = clothes.mesh1;
        card.mesh2 = clothes.mesh2;
        card.inventory = inventory;
        card.icon = clothes.icon;
        card.price = clothes.price;
        card.sewResulter = sewResulter;
        card.sewingClothes = clothes;

        SetCardIcon(spawnedCard, clothes.icon);
    }

    private void SetCardIcon(GameObject card, Sprite icon)
    {
        if (card.transform.childCount == 0)
            return;

        Image image = card.transform.GetChild(0).GetComponent<Image>();

        if (image != null)
            image.sprite = icon;
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
        int x = index % 3;
        int y = index / 3;

        card.transform.localPosition = new Vector2(
            75 - 37.22f + x * 75,
            -50 - 75 * y
        );
    }
}
