using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ShopCartUtility
{
    public static int GetAmount(Inventory inventory, string itemName)
    {
        if (inventory == null || inventory.cart == null)
            return 0;

        for (int i = 0; i < inventory.cart.Count; i++)
        {
            Inventory.ShopItem item = inventory.cart[i];
            if (item != null && item.name == itemName)
                return Mathf.Max(0, item.ammount);
        }

        return 0;
    }

    public static void AddCloth(Inventory inventory, string itemName, int price, Sprite icon, Color color)
    {
        if (inventory == null || inventory.cart == null)
            return;

        Inventory.ShopItem existingItem = FindByName(inventory, itemName);
        if (existingItem != null)
        {
            if (existingItem.ammount < int.MaxValue)
                existingItem.ammount++;
            return;
        }

        inventory.cart.Add(new Inventory.ShopCloth
        {
            name = itemName,
            price = price,
            icon = icon,
            color = color,
            ammount = 1
        });
    }

    public static void AddPattern(Inventory inventory, string itemName, int price, Sprite icon, string type, Mesh mesh)
    {
        if (inventory == null || inventory.cart == null)
            return;

        Inventory.ShopItem existingItem = FindByName(inventory, itemName);
        if (existingItem != null)
        {
            existingItem.ammount = 1;
            return;
        }

        inventory.cart.Add(new Inventory.ShopPart
        {
            name = itemName,
            price = price,
            icon = icon,
            type = type,
            mesh = mesh,
            ammount = 1
        });
    }

    public static void AddClothes(Inventory inventory, string itemName, int price, Sprite icon, string type, Mesh mesh1, Mesh mesh2)
    {
        if (inventory == null || inventory.cart == null)
            return;

        Inventory.ShopItem existingItem = FindByName(inventory, itemName);
        if (existingItem != null)
        {
            existingItem.ammount = 1;
            return;
        }

        inventory.cart.Add(new Inventory.ShopClothes
        {
            name = itemName,
            price = price,
            icon = icon,
            type = type,
            mesh1 = mesh1,
            mesh2 = mesh2,
            ammount = 1
        });
    }

    public static int RemoveOne(Inventory inventory, string itemName)
    {
        if (inventory == null || inventory.cart == null)
            return 0;

        for (int i = 0; i < inventory.cart.Count; i++)
        {
            Inventory.ShopItem item = inventory.cart[i];
            if (item == null || item.name != itemName)
                continue;

            item.ammount--;
            if (item.ammount <= 0)
            {
                inventory.cart.RemoveAt(i);
                return 0;
            }

            return item.ammount;
        }

        return 0;
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

    private static Inventory.ShopItem FindByName(Inventory inventory, string itemName)
    {
        for (int i = 0; i < inventory.cart.Count; i++)
        {
            Inventory.ShopItem item = inventory.cart[i];
            if (item != null && item.name == itemName)
                return item;
        }

        return null;
    }
}
