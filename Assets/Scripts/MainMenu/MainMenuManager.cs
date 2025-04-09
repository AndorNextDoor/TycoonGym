using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject continueButton;

    private GameObject scene;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ShowMainMenu()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetScene(GameObject button)
    {
        scene = button;
        continueButton.GetComponent<Button>().onClick.AddListener(() => ShowScene());
    }

    public void ShowScene()
    {
        scene.SetActive(true);
    }
}
