using UnityEngine;
using UnityEngine.EventSystems;

public class GymUpgradableClickHandler : MonoBehaviour
{
    [SerializeField] private GameObject upgradesMenu;

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Prevent clicking through UI
        {
            upgradesMenu.SetActive(true);
            ProgressionManager.Instance.SetGymMenuUpgradeValues();
        }
    }
}
