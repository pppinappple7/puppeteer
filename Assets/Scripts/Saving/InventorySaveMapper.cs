using System.Collections.Generic;
using UnityEngine;

public static class InventorySaveMapper
{
    public static bool TryCreate(
        Inventory inventory,
        ShopCatalog catalog,
        out InventorySaveData saveData,
        out string error)
    {
        saveData = null;
        error = null;

        if (!ValidateReferences(inventory, catalog, out error))
            return false;

        InventorySaveData result = new InventorySaveData
        {
            money = inventory.money
        };

        foreach (Inventory.SewingItem item in inventory.sewings)
        {
            if (!TryCreateSewingEntry(item, catalog, out InventoryItemSaveData entry, out error))
                return false;

            result.sewingItems.Add(entry);
        }

        foreach (Inventory.combining item in inventory.combiningInventory)
        {
            if (!TryCreateCombiningEntry(item, catalog, out InventoryItemSaveData entry, out error))
                return false;

            result.combiningItems.Add(entry);
        }

        saveData = result;
        return true;
    }

    public static bool TryRestore(
        Inventory inventory,
        ShopCatalog catalog,
        InventorySaveData saveData,
        out string error)
    {
        error = null;

        if (inventory == null)
        {
            error = "Inventory is not assigned.";
            return false;
        }

        if (catalog == null)
        {
            error = "ShopCatalog is not assigned.";
            return false;
        }

        if (saveData == null)
        {
            error = "Inventory save data is missing.";
            return false;
        }

        if (!catalog.TryValidateIds(out error))
            return false;

        saveData.EnsureCollections();

        List<Inventory.SewingItem> sewingItems = new List<Inventory.SewingItem>();
        List<Inventory.combining> combiningItems = new List<Inventory.combining>();

        foreach (InventoryItemSaveData entry in saveData.sewingItems)
        {
            if (!TryRestoreSewingEntry(entry, catalog, out Inventory.SewingItem item, out error))
                return false;

            sewingItems.Add(item);
        }

        foreach (InventoryItemSaveData entry in saveData.combiningItems)
        {
            if (!TryRestoreCombiningEntry(entry, catalog, out Inventory.combining item, out error))
                return false;

            combiningItems.Add(item);
        }

        inventory.money = saveData.money;

        if (inventory.sewings == null)
            inventory.sewings = new List<Inventory.SewingItem>();
        else
            inventory.sewings.Clear();

        if (inventory.combiningInventory == null)
            inventory.combiningInventory = new List<Inventory.combining>();
        else
            inventory.combiningInventory.Clear();

        inventory.sewings.AddRange(sewingItems);
        inventory.combiningInventory.AddRange(combiningItems);

        if (inventory.cart != null)
            inventory.cart.Clear();

        return true;
    }

    static bool ValidateReferences(Inventory inventory, ShopCatalog catalog, out string error)
    {
        error = null;

        if (inventory == null)
        {
            error = "Inventory is not assigned.";
            return false;
        }

        if (catalog == null)
        {
            error = "ShopCatalog is not assigned.";
            return false;
        }

        if (inventory.sewings == null || inventory.combiningInventory == null)
        {
            error = "Inventory lists are not initialized.";
            return false;
        }

        return catalog.TryValidateIds(out error);
    }

    static bool TryCreateSewingEntry(
        Inventory.SewingItem item,
        ShopCatalog catalog,
        out InventoryItemSaveData entry,
        out string error)
    {
        entry = null;
        error = null;

        if (item == null)
        {
            error = "Sewing inventory contains an empty entry.";
            return false;
        }

        if (item is Inventory.SewingCloth cloth)
        {
            if (cloth.ammount <= 0 || cloth.price < 0)
            {
                error = "A sewing cloth has an invalid amount or price.";
                return false;
            }

            if (!TryResolve(catalog, cloth.itemId, ShopProductType.Cloth, out _, out error))
                return false;

            entry = new InventoryItemSaveData
            {
                itemId = cloth.itemId,
                itemKind = InventoryItemKind.SewingCloth,
                amount = cloth.ammount,
                price = cloth.price,
                color1 = cloth.color
            };
            return true;
        }

        if (item is Inventory.SewingPart part)
        {
            if (part.price < 0)
            {
                error = "A sewing pattern has an invalid price.";
                return false;
            }

            if (!TryResolve(catalog, part.itemId, ShopProductType.Pattern, out _, out error))
                return false;

            entry = new InventoryItemSaveData
            {
                itemId = part.itemId,
                itemKind = InventoryItemKind.SewingPart,
                amount = 1,
                price = part.price
            };
            return true;
        }

        if (item is Inventory.SewingClothes clothes)
        {
            if (clothes.price < 0)
            {
                error = "A sewing clothes pattern has an invalid price.";
                return false;
            }

            if (!TryResolve(catalog, clothes.itemId, ShopProductType.Clothes, out _, out error))
                return false;

            entry = new InventoryItemSaveData
            {
                itemId = clothes.itemId,
                itemKind = InventoryItemKind.SewingClothes,
                amount = 1,
                price = clothes.price
            };
            return true;
        }

        error = "Sewing inventory contains an unsupported item type.";
        return false;
    }

    static bool TryCreateCombiningEntry(
        Inventory.combining item,
        ShopCatalog catalog,
        out InventoryItemSaveData entry,
        out string error)
    {
        entry = null;
        error = null;

        if (item == null)
        {
            error = "Combining inventory contains an empty entry.";
            return false;
        }

        if (item is Inventory.Part part)
        {
            if (part.price < 0)
            {
                error = "A combined part has an invalid price.";
                return false;
            }

            if (!TryResolve(catalog, part.itemId, ShopProductType.Pattern, out _, out error))
                return false;

            entry = new InventoryItemSaveData
            {
                itemId = part.itemId,
                itemKind = InventoryItemKind.CombiningPart,
                amount = 1,
                price = part.price,
                color1 = part.color
            };
            return true;
        }

        if (item is Inventory.Clothes clothes)
        {
            if (clothes.price < 0)
            {
                error = "Combined clothes have an invalid price.";
                return false;
            }

            if (!TryResolve(catalog, clothes.itemId, ShopProductType.Clothes, out _, out error))
                return false;

            entry = new InventoryItemSaveData
            {
                itemId = clothes.itemId,
                itemKind = InventoryItemKind.CombiningClothes,
                amount = 1,
                price = clothes.price,
                color1 = clothes.color1,
                color2 = clothes.color2
            };
            return true;
        }

        error = "Combining inventory contains an unsupported item type.";
        return false;
    }

    static bool TryRestoreSewingEntry(
        InventoryItemSaveData entry,
        ShopCatalog catalog,
        out Inventory.SewingItem item,
        out string error)
    {
        item = null;
        error = null;

        if (entry == null || entry.price < 0)
        {
            error = "A sewing inventory save entry is invalid.";
            return false;
        }

        if (entry.itemKind == InventoryItemKind.SewingCloth)
        {
            if (entry.amount <= 0)
            {
                error = "A saved cloth amount must be greater than zero.";
                return false;
            }

            if (!TryResolve(catalog, entry.itemId, ShopProductType.Cloth, out ShopProductData clothProduct, out error))
                return false;

            item = new Inventory.SewingCloth
            {
                itemId = entry.itemId,
                name = clothProduct.ProductName,
                icon = clothProduct.Icon,
                price = entry.price,
                ammount = entry.amount,
                color = entry.color1
            };
            return true;
        }

        if (entry.amount != 1)
        {
            error = "A non-stackable sewing item must have amount 1.";
            return false;
        }

        if (entry.itemKind == InventoryItemKind.SewingPart)
        {
            if (!TryResolve(catalog, entry.itemId, ShopProductType.Pattern, out ShopProductData partProduct, out error))
                return false;

            item = new Inventory.SewingPart
            {
                itemId = entry.itemId,
                name = partProduct.ProductName,
                icon = partProduct.Icon,
                price = entry.price,
                type = partProduct.Type,
                mesh = partProduct.Mesh
            };
            return true;
        }

        if (entry.itemKind == InventoryItemKind.SewingClothes)
        {
            if (!TryResolve(catalog, entry.itemId, ShopProductType.Clothes, out ShopProductData clothesProduct, out error))
                return false;

            item = new Inventory.SewingClothes
            {
                itemId = entry.itemId,
                name = clothesProduct.ProductName,
                icon = clothesProduct.Icon,
                price = entry.price,
                type = clothesProduct.Type,
                mesh1 = clothesProduct.Mesh,
                mesh2 = clothesProduct.SecondMesh
            };
            return true;
        }

        error = "The saved item kind does not belong to the sewing inventory.";
        return false;
    }

    static bool TryRestoreCombiningEntry(
        InventoryItemSaveData entry,
        ShopCatalog catalog,
        out Inventory.combining item,
        out string error)
    {
        item = null;
        error = null;

        if (entry == null || entry.price < 0 || entry.amount != 1)
        {
            error = "A combining inventory save entry is invalid.";
            return false;
        }

        if (entry.itemKind == InventoryItemKind.CombiningPart)
        {
            if (!TryResolve(catalog, entry.itemId, ShopProductType.Pattern, out ShopProductData partProduct, out error))
                return false;

            item = new Inventory.Part
            {
                itemId = entry.itemId,
                price = entry.price,
                mesh = partProduct.Mesh,
                type = partProduct.Type,
                icon = partProduct.Icon,
                color = entry.color1
            };
            return true;
        }

        if (entry.itemKind == InventoryItemKind.CombiningClothes)
        {
            if (!TryResolve(catalog, entry.itemId, ShopProductType.Clothes, out ShopProductData clothesProduct, out error))
                return false;

            item = new Inventory.Clothes
            {
                itemId = entry.itemId,
                price = entry.price,
                mesh1 = clothesProduct.Mesh,
                mesh2 = clothesProduct.SecondMesh,
                type = clothesProduct.Type,
                icon = clothesProduct.Icon,
                color1 = entry.color1,
                color2 = entry.color2
            };
            return true;
        }

        error = "The saved item kind does not belong to the combining inventory.";
        return false;
    }

    static bool TryResolve(
        ShopCatalog catalog,
        string itemId,
        ShopProductType expectedType,
        out ShopProductData product,
        out string error)
    {
        product = null;
        error = null;

        if (string.IsNullOrWhiteSpace(itemId))
        {
            error = "An inventory item has no itemId. Move this item to ShopCatalog and assign a stable ID.";
            return false;
        }

        if (!catalog.TryGetItem(itemId, out product))
        {
            error = "An inventory itemId is missing from ShopCatalog or is ambiguous: " + itemId;
            return false;
        }

        if (product.ProductType != expectedType)
        {
            error = "The ShopCatalog product type does not match the saved inventory item: " + itemId;
            product = null;
            return false;
        }

        if (!product.TryValidate(out error))
        {
            product = null;
            return false;
        }

        return true;
    }
}
