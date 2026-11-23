using System.Collections.Generic;
using UnityEngine;

public static class ShopPurchaseService
{
    public static void TryBuy(Inventory inventory)
    {
        if (inventory == null || inventory.cart == null || inventory.cart.Count == 0)
            return;

        ShopCheck check = inventory.ShopCheck;
        if (check == null)
        {
            Debug.LogError("ShopPurchaseService: ShopCheck reference is missing.", inventory);
            return;
        }

        check.Refresh();
        if (!check.TryGetTotal(out int total, out string error))
        {
            Debug.LogWarning("ShopPurchaseService: purchase cancelled. " + error, inventory);
            return;
        }

        if (inventory.money <= total)
        {
            GameObject moneyCounter = inventory.moneyCounter;
            if (moneyCounter != null)
            {
                Money money = moneyCounter.GetComponent<Money>();
                if (money != null)
                    money.NotEnough();
                else
                    Debug.LogWarning("ShopPurchaseService: Money component is missing.", moneyCounter);
            }
            return;
        }

        if (inventory.BoxPrefab == null)
        {
            Debug.LogError("ShopPurchaseService: box prefab is not assigned.", inventory);
            return;
        }

        if (!TryCreateContents(inventory.cart, out List<Inventory.SewingItem> contents, out error))
        {
            Debug.LogWarning("ShopPurchaseService: purchase cancelled. " + error, inventory);
            return;
        }

        GameObject boxObject = Object.Instantiate(inventory.BoxPrefab);
        Box box = boxObject.GetComponent<Box>();
        if (box == null)
        {
            Debug.LogError("ShopPurchaseService: Box prefab has no Box component.", boxObject);
            Object.Destroy(boxObject);
            return;
        }

        box.contents.AddRange(contents);
        inventory.money -= total;

        if (inventory.moneyCounter != null)
            inventory.moneyCounter.transform.localScale *= 2f;

        HideCartCounters();
        inventory.cart.Clear();

        if (inventory.PlayerMovement != null)
            inventory.PlayerMovement.targetTransform = null;
    }

    static bool TryCreateContents(List<Inventory.ShopItem> cart,
        out List<Inventory.SewingItem> contents, out string error)
    {
        contents = new List<Inventory.SewingItem>();

        foreach (Inventory.ShopItem item in cart)
        {
            if (item is Inventory.ShopCloth cloth)
            {
                contents.Add(new Inventory.SewingCloth
                {
                    price = cloth.price,
                    ammount = cloth.ammount,
                    name = cloth.name,
                    icon = cloth.icon,
                    color = cloth.color
                });
            }
            else if (item is Inventory.ShopPart part)
            {
                if (part.mesh == null || string.IsNullOrWhiteSpace(part.type))
                {
                    error = "Pattern has missing mesh or type: " + part.name;
                    return false;
                }

                contents.Add(new Inventory.SewingPart
                {
                    price = part.price,
                    name = part.name,
                    icon = part.icon,
                    mesh = part.mesh,
                    type = part.type
                });
            }
            else if (item is Inventory.ShopClothes clothes)
            {
                if (clothes.mesh1 == null || clothes.mesh2 == null || string.IsNullOrWhiteSpace(clothes.type))
                {
                    error = "Clothes have missing meshes or type: " + clothes.name;
                    return false;
                }

                contents.Add(new Inventory.SewingClothes
                {
                    price = clothes.price,
                    name = clothes.name,
                    icon = clothes.icon,
                    mesh1 = clothes.mesh1,
                    mesh2 = clothes.mesh2,
                    type = clothes.type
                });
            }
            else
            {
                error = item == null ? "Cart contains an empty item reference." :
                    "Cart contains an unsupported item type: " + item.GetType().Name;
                return false;
            }
        }

        error = null;
        return true;
    }

    static void HideCartCounters()
    {
        foreach (ShopClothesCard card in Object.FindObjectsByType<ShopClothesCard>(FindObjectsSortMode.None))
        {
            if (card.countObj != null)
                card.countObj.SetActive(false);
        }

        foreach (ShopPatternCard card in Object.FindObjectsByType<ShopPatternCard>(FindObjectsSortMode.None))
        {
            if (card.countObj != null)
                card.countObj.SetActive(false);
        }

        foreach (ShopClothCard card in Object.FindObjectsByType<ShopClothCard>(FindObjectsSortMode.None))
        {
            if (card.countObj != null)
                card.countObj.SetActive(false);
        }
    }
}
