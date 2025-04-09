using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    [SerializeField] private GymUpgrades[] gymUpgrades;

    private int neededExpirience;
    private int currentExpirience;
    private int currentLevel;

    [SerializeField] private Slider expirienceSlider;
    [SerializeField] private TextMeshProUGUI expirienceText;


    //UI Upgrades
    [SerializeField] private TextMeshProUGUI neededExpirienceText;
    [SerializeField] private TextMeshProUGUI currentExpirienceText;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;
    [SerializeField] private TextMeshProUGUI currentIncomeText;
    [SerializeField] private TextMeshProUGUI nextIncomeText;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private Button upgradeButton;
    

    private void Awake()
    {
        Instance = this;
        SetSliderAndTextExpirienceValues();
        neededExpirience = gymUpgrades[currentLevel].neededExpirience;
        upgradeButton.onClick.AddListener(() => UpgradeLevel());
    }

    private void UpgradeLevel()
    {
        if (currentExpirience < neededExpirience)
        {
            // TO DO: Show that you need more expirience;
            return;
        }
        upgradeButton.onClick.AddListener(() => UpgradeLevel());

        if (ResourcesManager.Instance.HaveEnoughToBuy(gymUpgrades[currentLevel].upgradeCost))
        {
            ResourcesManager.Instance.SpendGold(gymUpgrades[currentLevel].upgradeCost);
        }

        SetSliderAndTextExpirienceValues();
        currentLevel++;

        neededExpirience = gymUpgrades[currentLevel].neededExpirience;
        gymUpgrades[currentLevel].ObjectsToDisable.gameObject.SetActive(true);
        gymUpgrades[currentLevel].ObjectsToDisable.gameObject.SetActive(false);

        SetGymMenuUpgradeValues();
    }

    public bool IsEnoughLevel(int requiredLevel)
    {
        return currentLevel >= requiredLevel;
    }

    private void SetSliderAndTextExpirienceValues()
    {
        expirienceSlider.maxValue = neededExpirience;
        expirienceSlider.value = currentExpirience;

        expirienceText.text = "EXPIRIENCE: " + currentExpirience + "/" + neededExpirience;
    }


    public void GetExpirience()
    {
        currentExpirience ++;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetCurrentExpirience()
    {
        return currentExpirience;
    }

    public void LoadSave(int currentLevel, int currentExpirience)
    {

    }

    public void SetGymMenuUpgradeValues()
    {
        neededExpirienceText.text  = neededExpirience.ToString();
        currentExpirienceText.text = currentExpirience.ToString();

        int currentIncome = ResourcesManager.Instance.GetCurrentIncome();
        currentIncomeText.text = currentIncome.ToString();
        nextIncomeText.text    = (currentIncome + gymUpgrades[currentLevel].incomeUpgrade).ToString();

        currentLevelText.text = (currentLevel + 1).ToString();
        nextLevelText.text = (currentLevel + 2).ToString();

        upgradeCost.text = gymUpgrades[currentLevel].upgradeCost.ToString();
    }
}

[Serializable]
class GymUpgrades
{
    public int incomeUpgrade;
    public int neededExpirience;
    public Transform ObjectsToDisable;
    public Transform ObjectsToEnable;
    public int upgradeCost;
}
