using System;
using UnityEngine;

public class UpgradableItem : MonoBehaviour
{
    private BuildingUpgradesSO upgrades;

    //Stats
    private string buildingName;
    private string description;
    private int income;

    private int currentUpgradeIndex = - 1;

    public void Upgrade()
    {
        if(currentUpgradeIndex < 0)
        {
            if (ResourcesManager.Instance.HaveEnoughToBuy(upgrades.buildingUpgrades[currentUpgradeIndex + 1].upgradeCost))
            {
                ResourcesManager.Instance.SpendGold(upgrades.buildingUpgrades[currentUpgradeIndex + 1].upgradeCost);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (ResourcesManager.Instance.HaveEnoughToBuy(upgrades.buildingUpgrades[currentUpgradeIndex].upgradeCost))
            {
                ResourcesManager.Instance.SpendGold(upgrades.buildingUpgrades[currentUpgradeIndex].upgradeCost);
            }
            else
            {
                return;
            }
        }

        ResourcesManager.Instance.DecreaseIncomeFromCustomers(income);

        currentUpgradeIndex++;
        Destroy(transform.GetChild(0).gameObject);
        Instantiate(upgrades.buildingUpgrades[currentUpgradeIndex].newPrefab, transform);
        this.income = upgrades.buildingUpgrades[currentUpgradeIndex].income;

        ResourcesManager.Instance.IncreaseIncomeFromCustomers(income);
    }

    public void SetUpgradableStats(int income, string name, string description, BuildingUpgradesSO upgrades)
    {
        this.income = income;
        this.buildingName = name;
        this.description = description;
        this.upgrades = upgrades;
        Upgrade();
    }

    public void AddIncome()
    {
        ResourcesManager.Instance.IncreaseIncomeFromCustomers(income);
    }

    public string GetBuildingName()
    {
        return buildingName;
    }

    public string GetUpgradeCost()
    {
        return upgrades.buildingUpgrades[currentUpgradeIndex].upgradeCost.ToString();
    }

    public string GetCurrentIncome()
    {
        return income.ToString();
    }

    public string GetNextLevelIncome()
    {
        if(currentUpgradeIndex == upgrades.buildingUpgrades.Count)
        {
            return upgrades.buildingUpgrades[currentUpgradeIndex + 1].income.ToString();
        }
        else
        {
            return upgrades.buildingUpgrades[currentUpgradeIndex].income.ToString();
        }
    }

    public int GetCurrentLevel()
    {
        return (currentUpgradeIndex + 1);
    }

    public bool IsLastUpgrade()
    {
        return upgrades.buildingUpgrades[currentUpgradeIndex].lastUpgrade;
    }
}
