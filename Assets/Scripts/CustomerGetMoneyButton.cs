using UnityEngine;
using UnityEngine.UI;

public class CustomerGetMoneyButton : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => ResourcesManager.Instance.GetGold());
    }
}
