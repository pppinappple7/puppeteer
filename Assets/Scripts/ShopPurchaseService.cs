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
            return;

        check.Refresh();
        if (!check.TryGetTotal(out int total, out _))
            return;

        if (inventory.money <= total)
        {
            if (inventory.moneyCounter != null)
            {
                Money money = inventory.moneyCounter.GetComponent<Money>();
                if (money != null)
                    money.NotEnough();
            }
            return;
        }

        if (inventory.BoxPrefab == null)
            return;

        if (!TryCreateContents(inventory.cart, out List<Inventory.SewingItem> contents))
            return;

        GameObject boxObject = Object.Instantiate(inventory.BoxPrefab);
        Box box = boxObject.GetComponent<Box>();
        if (box == null)
        {
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
        out List<Inventory.SewingItem> contents)
    {
        contents = new List<Inventory.SewingItem>();

        foreach (Inventory.ShopItem item in cart)
        {
            if (item is Inventory.ShopCloth cloth)
            {
                contents.Add(new Inventory.SewingCloth
                {
                    itemId = cloth.itemId,
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
                    return false;

                contents.Add(new Inventory.SewingPart
                {
                    itemId = part.itemId,
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
                    return false;

                contents.Add(new Inventory.SewingClothes
                {
                    itemId = clothes.itemId,
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
                return false;
            }
        }

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
