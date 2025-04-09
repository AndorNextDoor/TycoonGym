using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject scene;
    private void Start()
    {
        MainMenuManager.Instance.SetScene(scene);
        GetComponent<Button>().onClick.AddListener(()  => MainMenuManager.Instance.ShowMainMenu());
    }
}
