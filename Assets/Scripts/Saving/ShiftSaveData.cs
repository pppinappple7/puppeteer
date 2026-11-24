using System;
using System.Collections.Generic;


[Serializable]
public class ShiftSaveData
{
    public int completedShifts;

   
    public List<string> usedCustomerIds = new List<string>();

    public void EnsureCollections()
    {
        if (usedCustomerIds == null)
            usedCustomerIds = new List<string>();
    }
}
