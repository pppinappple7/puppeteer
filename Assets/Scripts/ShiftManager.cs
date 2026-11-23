using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShiftManager : MonoBehaviour
{
    [SerializeField] Reputation reputation;
    int shift;
    int customersServed;
    [System.Serializable]
    class customer
    {
        
        public GameObject prefab;
        public int levelNeeded;
        public bool beenUsed;
    }
    [SerializeField] customer[] easyCustomers;
    [SerializeField] customer[] normalCustomers;
    [SerializeField] customer[] hardCustomers;
    customer[][] customers;
    void Start()
    {
        customers = new customer[][]
        {
            easyCustomers,
            normalCustomers,
            hardCustomers
        };
    }
    
    void Update()
    {
        
    }
    public void NewCustomer()
    {
        customer spawning = suitableCustomer();
        Instantiate(spawning.prefab);
        spawning.beenUsed = true;
        customersServed++;
    }
    customer suitableCustomer()
    {
        List<customer> onLevelCustomers = new List<customer>();
        foreach(var customer in customers[customersServed])
        {
            if (customer.levelNeeded <= reputation.GetLevel())
            {
                onLevelCustomers.Add(customer);
            }
            
        }
        foreach (customer customer in onLevelCustomers)
        {
            if (!customer.beenUsed)
            {
                customer.beenUsed = true;
                return customer;
            }
        }

        return (onLevelCustomers[UnityEngine.Random.Range(0, onLevelCustomers.Count )]);
    }
}
