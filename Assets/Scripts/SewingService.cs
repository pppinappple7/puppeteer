using UnityEngine;

public static class SewingService
{
    public static bool TrySewPart(
        Inventory inventory,
        Inventory.SewingCloth cloth,
        Inventory.SewingPart pattern,
        out string error)
    {
        error = null;

        if (!HasInventory(inventory, out error))
            return false;

        if (!IsAvailable(inventory, cloth, 1))
        {
            error = "Недостаточно ткани или выбранная ткань отсутствует в инвентаре.";
            return false;
        }

        if (pattern == null || !inventory.sewings.Contains(pattern))
        {
            error = "Выбранная выкройка отсутствует в инвентаре.";
            return false;
        }

        if (pattern.mesh == null)
        {
            error = "У выбранной выкройки не назначен меш.";
            return false;
        }

        Inventory.Part result = new Inventory.Part
        {
            itemId = pattern.itemId,
            mesh = pattern.mesh,
            color = cloth.color,
            type = pattern.type,
            icon = pattern.icon,
            price = cloth.price + pattern.price
        };

        inventory.combiningInventory.Add(result);
        ConsumeCloth(inventory, cloth, 1);

        return true;
    }

    public static bool TrySewClothes(
        Inventory inventory,
        Inventory.SewingCloth cloth1,
        Inventory.SewingCloth cloth2,
        Inventory.SewingClothes pattern,
        out string error)
    {
        error = null;

        if (!HasInventory(inventory, out error))
            return false;

        if (cloth1 == null || cloth2 == null)
        {
            error = "Для пошива одежды нужно выбрать две ткани.";
            return false;
        }

        if (cloth1 == cloth2)
        {
            if (!IsAvailable(inventory, cloth1, 2))
            {
                error = "Для пошива одежды необходимо две единицы выбранной ткани.";
                return false;
            }
        }
        else if (!IsAvailable(inventory, cloth1, 1) ||
                 !IsAvailable(inventory, cloth2, 1))
        {
            error = "Одна из выбранных тканей отсутствует в инвентаре или закончилась.";
            return false;
        }

        if (pattern == null || !inventory.sewings.Contains(pattern))
        {
            error = "Выбранная выкройка одежды отсутствует в инвентаре.";
            return false;
        }

        if (pattern.mesh1 == null || pattern.mesh2 == null)
        {
            error = "У выкройки одежды не назначены необходимые меши.";
            return false;
        }

        Inventory.Clothes result = new Inventory.Clothes
        {
            itemId = pattern.itemId,
            mesh1 = pattern.mesh1,
            mesh2 = pattern.mesh2,
            color1 = cloth1.color,
            color2 = cloth2.color,
            type = pattern.type,
            icon = pattern.icon,
            price = cloth1.price + cloth2.price + pattern.price
        };

        inventory.combiningInventory.Add(result);

        if (cloth1 == cloth2)
        {
            ConsumeCloth(inventory, cloth1, 2);
        }
        else
        {
            ConsumeCloth(inventory, cloth1, 1);
            ConsumeCloth(inventory, cloth2, 1);
        }

        return true;
    }

    private static bool HasInventory(Inventory inventory, out string error)
    {
        error = null;

        if (inventory == null)
        {
            error = "Inventory не назначен.";
            return false;
        }

        if (inventory.sewings == null || inventory.combiningInventory == null)
        {
            error = "Списки Inventory не инициализированы.";
            return false;
        }

        return true;
    }

    private static bool IsAvailable(
        Inventory inventory,
        Inventory.SewingCloth cloth,
        int requiredAmount)
    {
        return cloth != null &&
               inventory.sewings.Contains(cloth) &&
               cloth.ammount >= requiredAmount;
    }

    private static void ConsumeCloth(
        Inventory inventory,
        Inventory.SewingCloth cloth,
        int amount)
    {
        cloth.ammount -= amount;

        if (cloth.ammount <= 0)
            inventory.sewings.Remove(cloth);
    }
}
