using System.Collections.Generic;
using UnityEngine;

public class DollAssembly : MonoBehaviour
{
    private Inventory inventory;

    private readonly Dictionary<PartPicker, Inventory.combining> installedBySlot =
        new Dictionary<PartPicker, Inventory.combining>();

    private readonly Dictionary<Inventory.combining, PartPicker> slotByItem =
        new Dictionary<Inventory.combining, PartPicker>();

    public bool IsInstalled(Inventory.combining item)
    {
        return item != null && slotByItem.ContainsKey(item);
    }

    public bool TryInstall(
        Inventory sourceInventory,
        PartPicker slot,
        Inventory.combining item,
        out string error)
    {
        error = null;

        if (!ValidateInventory(sourceInventory, out error))
            return false;

        if (slot == null)
        {
            error = "The selected assembly slot is missing.";
            return false;
        }

        if (item == null)
        {
            error = "No item was selected.";
            return false;
        }

        if (!sourceInventory.combiningInventory.Contains(item))
        {
            error = "The selected item is no longer in the inventory.";
            return false;
        }

        if (slotByItem.TryGetValue(item, out PartPicker occupiedSlot))
        {
            error = occupiedSlot == slot
                ? "This item is already installed in this slot."
                : "This item is already installed in another slot.";
            return false;
        }

        if (!slot.CanInstall(item, out error))
            return false;

        if (installedBySlot.TryGetValue(slot, out Inventory.combining previousItem))
            slotByItem.Remove(previousItem);

        slot.CaptureOriginalVisuals();
        slot.RestoreOriginalVisuals();

        if (!slot.ApplyItem(item, out error))
        {
            if (previousItem != null)
            {
                slot.ApplyItem(previousItem, out _);
                slotByItem[previousItem] = slot;
            }
            else
            {
                installedBySlot.Remove(slot);
            }

            return false;
        }

        installedBySlot[slot] = item;
        slotByItem[item] = slot;

        RefreshAssemblyUI();
        return true;
    }

    
    public bool TryCompleteOrder(Inventory sourceInventory, out string error)
    {
        error = null;

        if (!ValidateInventory(sourceInventory, out error))
            return false;

        foreach (KeyValuePair<PartPicker, Inventory.combining> entry in installedBySlot)
        {
            if (entry.Key == null || entry.Value == null)
            {
                error = "The assembly contains a missing slot or item reference.";
                return false;
            }

            if (!sourceInventory.combiningInventory.Contains(entry.Value))
            {
                error = "An installed item is missing from the inventory.";
                return false;
            }
        }

        foreach (Inventory.combining item in slotByItem.Keys)
            sourceInventory.combiningInventory.Remove(item);

        installedBySlot.Clear();
        slotByItem.Clear();

        RefreshAssemblyUI();
        return true;
    }

    private bool ValidateInventory(Inventory sourceInventory, out string error)
    {
        error = null;

        if (sourceInventory == null)
        {
            error = "Inventory is not assigned.";
            return false;
        }

        if (sourceInventory.combiningInventory == null)
        {
            error = "The combining inventory is not initialized.";
            return false;
        }

        if (inventory != null && inventory != sourceInventory)
        {
            error = "This doll is already linked to a different Inventory.";
            return false;
        }

        inventory = sourceInventory;
        return true;
    }

    private void RefreshAssemblyUI()
    {
        CombineSetUp[] panels = FindObjectsOfType<CombineSetUp>();

        foreach (CombineSetUp panel in panels)
        {
            if (panel != null)
                panel.Refresh();
        }
    }
}
