using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradableClickHandler : MonoBehaviour
{
    private UpgradableItem upgradable;

    private void Start()
    {
        upgradable = GetComponent<UpgradableItem>();
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // Prevent clicking through UI
        {
            UpgradableItemUI.Instance.ShowBuildingInfo(upgradable);
        }
    }
}
