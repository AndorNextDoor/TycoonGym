using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradableItemUI : MonoBehaviour
{
    public static UpgradableItemUI Instance;

    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI buildingNameText;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelNextLevelText;
    
    [SerializeField] private TextMeshProUGUI incomeText;    
    [SerializeField] private TextMeshProUGUI incomeNextLevelText;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    [SerializeField] private GameObject[] objectsToHide;

    private void Awake()
    {
        Instance = this;
        uiPanel.SetActive(false); // Hide UI at the start
    }

    public void ShowBuildingInfo(UpgradableItem building)
    {
        ShowAllVariables();
        if (building.IsLastUpgrade())
        {
            HideVarialbesIfLastUpdate();
        }
        uiPanel.SetActive(true);
        buildingNameText.text    = building.GetBuildingName();

        levelText.text           = building.GetCurrentLevel().ToString();
        levelNextLevelText.text  = (building.GetCurrentLevel() + 1).ToString();

        incomeText.text          = building.GetCurrentIncome().ToString();
        incomeNextLevelText.text = building.GetNextLevelIncome();
        
        upgradeCostText.text     = building.GetUpgradeCost().ToString(); 

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => building.Upgrade());
        upgradeButton.onClick.AddListener(() => HideUpgradeMenu());
    }

    private void HideUpgradeMenu()
    {
        uiPanel.gameObject.SetActive(false);
    }

    private void HideVarialbesIfLastUpdate()
    {
        foreach(GameObject _object in objectsToHide)
        {
            _object.SetActive(false);
        }
    }

    private void ShowAllVariables()
    {
        foreach (GameObject _object in objectsToHide)
        {
            _object.SetActive(true);
        }
    }

    public void HideUI()
    {
        uiPanel.SetActive(false);
    }
}


