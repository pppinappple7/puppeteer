using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    [SerializeField] Splash splash;
    float inbetweenTimer = 7;
    [SerializeField] Inventory inventory;
    [SerializeField] SaveManager saveManager;
    [SerializeField] GameObject doll;
    [SerializeField] TransitionSettings transition;
    [SerializeField] Reputation reputation;

    int shift=1;
    public int customersServed;
    bool transitionStarted;

    [System.Serializable]
    class customer
    {
        public string customerId;
        public GameObject prefab;
        public int levelNeeded;
        public bool beenUsed;
    }

    [SerializeField] customer[] easyCustomers;
    [SerializeField] customer[] normalCustomers;
    [SerializeField] customer[] hardCustomers;

    customer[][] customers;
    Customer spawnedCustomer;

    public int CompletedShifts => shift;
    IEnumerator Start()
    {
        while (!saveManager.IsInitialized && string.IsNullOrEmpty(saveManager.LastError))
            yield return null;

        if (!saveManager.IsInitialized)
        {
            enabled = false;
            yield break;
        }

        if (saveManager.SaveLoaded)
        {
            
        }
        else
        {

        }
        splash.CreateSplash("Смена №" + shift, 3);
    }

    private void Awake()
    {
        EnsureCustomerGroups();
    }

    private void Update()
    {
        if (customersServed==3)
        {
            splash.CreateSplash("Смена окончена", 3);
            customersServed = 4;
        }
        if (spawnedCustomer != null)
            return;

        if (inbetweenTimer <= 0)
        {
            NewCustomer();
            inbetweenTimer = 4;
        }
        else
        {
            inbetweenTimer -= Time.deltaTime;
        }
    }

    void EnsureCustomerGroups()
    {
        customers = new customer[][]
        {
            easyCustomers ?? new customer[0],
            normalCustomers ?? new customer[0],
            hardCustomers ?? new customer[0]
        };
    }

    void NewCustomer()
    {
        if (customersServed >= 3)
            return;

        customer spawning = SuitableCustomer();
        if (spawning == null || spawning.prefab == null)
            return;

        GameObject spawnedObject = Instantiate(spawning.prefab);
        spawnedCustomer = spawnedObject.GetComponent<Customer>();

        if (spawnedCustomer == null)
        {
            Destroy(spawnedObject);
            return;
        }

        spawnedCustomer.inventory = inventory;
        spawnedCustomer.doll = doll;
        spawnedCustomer.ShiftManager = this;

        spawning.beenUsed = true;
    }

    customer SuitableCustomer()
    {
        EnsureCustomerGroups();

        if (reputation == null || customersServed < 0 || customersServed >= customers.Length)
            return null;

        List<customer> eligibleCustomers = new List<customer>();
        customer[] group = customers[customersServed];

        foreach (customer entry in group)
        {
            if (entry != null && entry.levelNeeded <= reputation.GetLevel())
                eligibleCustomers.Add(entry);
        }

        if (eligibleCustomers.Count == 0)
            return null;

        foreach (customer entry in eligibleCustomers)
        {
            if (!entry.beenUsed)
                return entry;
        }

        return eligibleCustomers[Random.Range(0, eligibleCustomers.Count)];
    }

    public bool TryGetUsedCustomerIds(List<string> result, out string error)
    {
        error = null;

        if (result == null)
        {
            error = "The customer ID list is not assigned.";
            return false;
        }

        EnsureCustomerGroups();
        result.Clear();

        HashSet<string> allIds = new HashSet<string>();

        foreach (customer[] group in customers)
        {
            foreach (customer entry in group)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.customerId))
                {
                    error = "Every customer entry needs a customerId.";
                    return false;
                }

                if (!allIds.Add(entry.customerId))
                {
                    error = "Duplicate customerId: " + entry.customerId;
                    return false;
                }

                if (entry.beenUsed)
                    result.Add(entry.customerId);
            }
        }

        return true;
    }

    public bool TryRestoreProgress(int completedShifts, List<string> usedCustomerIds, out string error)
    {
        error = null;

        if (completedShifts < 0)
        {
            error = "Completed shifts cannot be negative.";
            return false;
        }

        if (usedCustomerIds == null)
        {
            error = "The saved customer ID list is missing.";
            return false;
        }

        EnsureCustomerGroups();

        Dictionary<string, customer> customersById = new Dictionary<string, customer>();

        foreach (customer[] group in customers)
        {
            foreach (customer entry in group)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.customerId))
                {
                    error = "Every customer entry needs a customerId.";
                    return false;
                }

                if (customersById.ContainsKey(entry.customerId))
                {
                    error = "Duplicate customerId: " + entry.customerId;
                    return false;
                }

                customersById.Add(entry.customerId, entry);
            }
        }

        HashSet<string> usedIds = new HashSet<string>();

        foreach (string customerId in usedCustomerIds)
        {
            if (string.IsNullOrWhiteSpace(customerId) || !usedIds.Add(customerId))
            {
                error = "The saved customer ID list contains an empty or duplicate ID.";
                return false;
            }

            if (!customersById.ContainsKey(customerId))
            {
                error = "The saved customerId does not exist in the current customer lists: " + customerId;
                return false;
            }
        }

        foreach (KeyValuePair<string, customer> entry in customersById)
            entry.Value.beenUsed = usedIds.Contains(entry.Key);

        shift = completedShifts;
        customersServed = 0;
        transitionStarted = false;
        return true;
    }

    public void NextDay()
    {
        Debug.Log("Invoken");
        if (customersServed < 3 || transitionStarted)
            return;

        if (saveManager == null)
            return;

        shift++;
        Debug.Log("Passed returns");
        if (!saveManager.TrySaveGame(out string error))
        {
            shift--;
            return;
        }
        Debug.Log("Transition");
        transitionStarted = true;
        TransitionManager.Instance().Transition("Workshop", transition, 0f);
    }

    public void Give()
    {
        if (spawnedCustomer != null)
            spawnedCustomer.GetDolled();
    }
}
