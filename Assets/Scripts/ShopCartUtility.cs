using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ShopCartUtility
{
    public static int GetAmount(Inventory inventory, string itemName, string itemId = null)
    {
        Inventory.ShopItem item = FindItem(inventory, itemId, itemName);
        return item == null ? 0 : Mathf.Max(0, item.ammount);
    }

    public static void AddCloth(Inventory inventory, string itemName, int price, Sprite icon, Color color, string itemId = null)
    {
        if (inventory == null || inventory.cart == null)
            return;

        Inventory.ShopItem existingItem = FindItem(inventory, itemId, itemName);
        if (existingItem != null)
        {
            if (existingItem.ammount < int.MaxValue)
                existingItem.ammount++;
            return;
        }

        inventory.cart.Add(new Inventory.ShopCloth
        {
            itemId = itemId,
            name = itemName,
            price = price,
            icon = icon,
            color = color,
            ammount = 1
        });
    }

    public static void AddPattern(Inventory inventory, string itemName, int price, Sprite icon, string type, Mesh mesh, string itemId = null)
    {
        if (inventory == null || inventory.cart == null)
            return;

        Inventory.ShopItem existingItem = FindItem(inventory, itemId, itemName);
        if (existingItem != null)
        {
            existingItem.ammount = 1;
            return;
        }

        inventory.cart.Add(new Inventory.ShopPart
        {
            itemId = itemId,
            name = itemName,
            price = price,
            icon = icon,
            type = type,
            mesh = mesh,
            ammount = 1
        });
    }

    public static void AddClothes(Inventory inventory, string itemName, int price, Sprite icon, string type, Mesh mesh1, Mesh mesh2, string itemId = null)
    {
        if (inventory == null || inventory.cart == null)
            return;

        Inventory.ShopItem existingItem = FindItem(inventory, itemId, itemName);
        if (existingItem != null)
        {
            existingItem.ammount = 1;
            return;
        }

        inventory.cart.Add(new Inventory.ShopClothes
        {
            itemId = itemId,
            name = itemName,
            price = price,
            icon = icon,
            type = type,
            mesh1 = mesh1,
            mesh2 = mesh2,
            ammount = 1
        });
    }

    public static int RemoveOne(Inventory inventory, string itemName, string itemId = null)
    {
        if (inventory == null || inventory.cart == null)
            return 0;

        Inventory.ShopItem item = FindItem(inventory, itemId, itemName);
        if (item == null)
            return 0;

        item.ammount--;
        if (item.ammount <= 0)
        {
            inventory.cart.Remove(item);
            return 0;
        }

        return item.ammount;
    }

    public static void SetCount(GameObject countObject, int amount)
    {
        if (countObject == null)
            return;

        countObject.SetActive(amount > 0);

        if (countObject.transform.childCount == 0)
            return;

        TextMeshProUGUI amountText = countObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (amountText != null)
            amountText.text = amount.ToString();
    }

    public static void ShowPreview(GameObject previewObject, Sprite preview, string itemName, int price)
    {
        if (previewObject == null)
            return;

        Image previewImage = previewObject.GetComponent<Image>();
        if (previewImage != null)
            previewImage.sprite = preview;

        if (previewObject.transform.childCount > 0)
        {
            TextMeshProUGUI nameText = previewObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (nameText != null)
                nameText.text = itemName;
        }

        if (previewObject.transform.childCount > 1)
        {
            TextMeshProUGUI priceText = previewObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            if (priceText != null)
                priceText.text = price.ToString();
        }
    }

    private static Inventory.ShopItem FindItem(Inventory inventory, string itemId, string itemName)
    {
        if (inventory == null || inventory.cart == null)
            return null;

        for (int i = 0; i < inventory.cart.Count; i++)
        {
            Inventory.ShopItem item = inventory.cart[i];
            if (item == null)
                continue;

            if (!string.IsNullOrWhiteSpace(itemId))
            {
                if (item.itemId == itemId)
                    return item;
            }
            else if (item.name == itemName)
            {
                return item;
            }
        }

        return null;
    }
}
