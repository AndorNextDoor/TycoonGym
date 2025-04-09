using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance;
    
    // Money
    private int gold;
    [SerializeField] private GameObject notEnoughGoldText;
    [SerializeField] private GameObject enoughGoldText;
    [SerializeField] private TextMeshProUGUI goldText;
    private int currentGymIncome = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGoldText();
        GetGold(5000);
    }

    public bool HaveEnoughToBuy(int cost)
    {
        if (gold < cost)
        {
            StartCoroutine(ShowNotEnoughGold());
        }
        else
        {
            StartCoroutine(ShowItemBought());
        }
        return (gold >= cost);
    }   
    public void SpendGold(int cost)
    {
        gold -= cost;
        UpdateGoldText();
    } 
    public void GetGold(int cost)
    {
        gold += cost;
        UpdateGoldText();
    }  
    public void GetGold()
    {
        gold += currentGymIncome;
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        goldText.text = gold.ToString();
    }

    IEnumerator ShowNotEnoughGold()
    {
        notEnoughGoldText.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        notEnoughGoldText.SetActive(false);
    }
    IEnumerator ShowItemBought()
    {
        enoughGoldText.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        enoughGoldText.SetActive(false);
    }

    public int GetCurrentGold()
    {
        return gold;
    }

    public void SetCurrentGold(int savedAmount)
    {
        gold = savedAmount;
        UpdateGoldText();
    }

    public void IncreaseIncomeFromCustomers(int addedIncome)
    {
        currentGymIncome += addedIncome;
    }  
    public void DecreaseIncomeFromCustomers(int addedIncome)
    {
        currentGymIncome += addedIncome;
    }
    public void SetIncomeFromCustomers(int addedIncome)
    {
        currentGymIncome = addedIncome;
    }

    public void LoadSave(int currentGold, int currentIncome)
    {
        this.currentGymIncome = currentIncome;
        this.gold = currentGold;
        UpdateGoldText();
    }

    public int GetCurrentIncome()
    {
        return currentGymIncome;
    }
}
