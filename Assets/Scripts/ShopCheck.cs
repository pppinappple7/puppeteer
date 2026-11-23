using System.Text;
using TMPro;
using UnityEngine;

public class ShopCheck : MonoBehaviour
{
    public int total;
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI listText;
    [SerializeField] TextMeshProUGUI totalText;

    void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (!TryGetTotal(out int calculatedTotal, out string error))
        {
            total = 0;
            if (listText != null)
                listText.text = "Invalid cart data";
            if (totalText != null)
                totalText.text = "—";

            if (!string.IsNullOrEmpty(error))
                Debug.LogWarning("ShopCheck: " + error, this);
            return;
        }

        total = calculatedTotal;
        StringBuilder builder = new StringBuilder();

        if (inventory != null && inventory.cart != null)
        {
            foreach (Inventory.ShopItem item in inventory.cart)
            {
                builder.Append(item.name)
                    .Append(" x")
                    .Append(item.ammount)
                    .Append(" - ")
                    .Append((long)item.price * item.ammount)
                    .Append('\n');
            }
        }

        if (listText != null)
            listText.text = builder.ToString();
        if (totalText != null)
            totalText.text = total.ToString();
    }

    public bool TryGetTotal(out int result, out string error)
    {
        result = 0;

        if (inventory == null || inventory.cart == null)
        {
            error = "Inventory or cart reference is missing.";
            return false;
        }

        long calculatedTotal = 0;
        for (int i = 0; i < inventory.cart.Count; i++)
        {
            Inventory.ShopItem item = inventory.cart[i];
            if (item == null)
            {
                error = "Cart contains an empty item reference.";
                return false;
            }

            if (item.price < 0 || item.ammount <= 0)
            {
                error = "Cart item has a negative price or a non-positive amount: " + item.name;
                return false;
            }

            calculatedTotal += (long)item.price * item.ammount;
            if (calculatedTotal > int.MaxValue)
            {
                error = "Cart total exceeds the supported integer range.";
                return false;
            }
        }

        result = (int)calculatedTotal;
        error = null;
        return true;
    }
}
