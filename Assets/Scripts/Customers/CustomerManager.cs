using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [SerializeField] private GameObject[] customerPrefabs;
    [SerializeField] private Transform[] customerSpawnPoints;

    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform cashRegisterPoint;

    [System.Serializable]
    public class GymItem
    {
        public Transform itemTransform;
        public bool isUsed = false;
    }

    private List<GymItem> gymItems = new List<GymItem>();

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomerCoroutine());
    }

    IEnumerator SpawnCustomerCoroutine()
    {
        while (true)
        {
            int averageMachineUse = 10;
            if(gymItems.Count > 0)
            {
                yield return new WaitForSeconds(Random.Range((averageMachineUse / gymItems.Count), (averageMachineUse/gymItems.Count * 1.3f) ));
            }
            else
            {
                yield return new WaitForSeconds(Random.Range((averageMachineUse / 1), (averageMachineUse/1 * 1.3f) ));
            }
            if (HaveAviableSeat())
            {
                SpawnCustomer();
            }
        }
    }

    private void SpawnCustomer()
    {
        GameObject newCustomer = Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Length - 1)]);

        Transform randomSpawnPoint = customerSpawnPoints[Random.Range(0, customerSpawnPoints.Length)];
        newCustomer.transform.position = randomSpawnPoint.position;
    }

    // Get a random unused item
    public GymItem GetAvailableItem()
    {
        List<GymItem> available = gymItems.Where(item => !item.isUsed).ToList();
        if (available.Count == 0) return null;

        GymItem chosen = available[Random.Range(0, available.Count)];
        chosen.isUsed = true;
        return chosen;
    }

    private bool HaveAviableSeat()
    {
        List<GymItem> available = gymItems.Where(item => !item.isUsed).ToList();
        if (available.Count == 0) return false;

        return true;
    }

    public void AddNewItem(Transform newItemTransform)
    {
        GymItem newItem = new GymItem
        {
            itemTransform = newItemTransform,
            isUsed = false
        };

        gymItems.Add(newItem);
    }

    public void ReleaseItem(GymItem item)
    {
        item.isUsed = false;
    }

    public Transform GetExitPoint()
    {
        return exitPoint;
    }

    public Transform GetCashRegisterPoint()
    {
        return cashRegisterPoint;
    }
}
