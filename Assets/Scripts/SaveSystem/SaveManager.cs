using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private Transform loadMenuContainer;
    [SerializeField] private GameObject loadFilePrefab;

    private Transform player;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject continueButton;

    private string currentSaveToLoad;
    private List<PlayerBuildedObjects> currentPlacedObjects = new List<PlayerBuildedObjects>();

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        ResetPlayerObject();
    }

    private void Start()
    {
        SceneLoaderManager.Instance.OnGameLoaded += ResetPlayerObject;
        StartCoroutine(SaveCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Save();
        }    
        
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            LoadLastSave();
        }
    }

    private void ResetPlayerObject()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch
        {
            Debug.LogWarning("Player is not found");
        }

    }

    public void Save()
    {
        StopCoroutine(SaveCoroutine());
        StartCoroutine(SaveCoroutine());
    }

    public IEnumerator SaveCoroutine()
    {
        int saveTimer = 60;
        
        while (true)
        {
            yield return new WaitForSeconds(2);

            if (player != null)
            {
                Vector3 playerPosition = player.position;
                int moneyAmount = ResourcesManager.Instance.GetCurrentGold();

                SaveObject saveObject = new SaveObject
                {
                    goldAmount     = moneyAmount,
                    incomeAmount   = ResourcesManager.Instance.GetCurrentIncome(),
                    playerPosition = playerPosition,
                    buildedObjects = currentPlacedObjects,
                    currentLevel   = ProgressionManager.Instance.GetCurrentLevel(),
                    expAmount      = ProgressionManager.Instance.GetCurrentExpirience(),
                    inventory      = InventoryManager.Instance.GetInventory()
                };
        
                string json = JsonUtility.ToJson(saveObject);

                SaveSystem.Save(json);
            }
                

            yield return new WaitForSeconds(saveTimer);
        }
    }

    public void LoadLastSave()
    {

        string saveString = SaveSystem.LoadLastSave(); 
        if(saveString != null)
        {
            Load(saveString);
        }
    }

    public void Load(string saveString)
    {
        mainMenu.SetActive(false);

        if (saveString != null)
        {
            StartCoroutine(LoadSaveFileCoroutine(saveString));
        }
    }

    IEnumerator LoadSaveFileCoroutine(string saveString)
    {

        //Loading the game scene
        SceneLoaderManager.Instance.LoadMainScene();
        continueButton.SetActive(false);

        yield return new WaitForSeconds(2);

        ResetPlayerObject();
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

        //Player
        if(player != null)
            player.transform.position = saveObject.playerPosition;
        
        //Income & Progression
        ResourcesManager.Instance.LoadSave(saveObject.goldAmount, saveObject.incomeAmount);
        ProgressionManager.Instance.LoadSave(saveObject.currentLevel, saveObject.expAmount);

        // Inventory and builded objects
        LoadPlacedObject(saveObject.buildedObjects);
        InventoryManager.Instance.LoadInventory(saveObject.inventory);
    }
    public void SaveBuildedObject(Vector3 _position, DatabaseObject placedObject)
    {
        PlayerBuildedObjects newObject = new PlayerBuildedObjects
        {
            position = _position,
            _object = placedObject
        };

        currentPlacedObjects.Add(newObject);
    }

    private void LoadPlacedObject(List<PlayerBuildedObjects> savedObjects)
    {
        foreach (PlayerBuildedObjects savedObject in savedObjects)
        {
            GameObject newStructure = Instantiate(savedObject._object.Prefab);
            newStructure.transform.position = savedObject.position;

            CustomerManager.Instance.AddNewItem(newStructure.transform);
        }
    }

    public void GenerateAllLoadButtons()
    {
        FileInfo[] saveFiles = SaveSystem.GetAllSaveFiles();

        foreach(Transform child in loadMenuContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (FileInfo fileInfo in saveFiles)
        {
            GameObject newLoad = Instantiate(loadFilePrefab, loadMenuContainer);
            newLoad.GetComponentInChildren<TextMeshProUGUI>().text = "SAVE FILE FROM: " + fileInfo.LastAccessTime;

            string saveString = File.ReadAllText(fileInfo.FullName);
            newLoad.GetComponentInChildren<Button>().onClick.AddListener(() => Load(saveString));
        }
    }

    private void SetContinueButton()
    {
        if (!SaveSystem.HaveSaveFiles())
            return;

        continueButton.SetActive(true);
        continueButton.GetComponentInChildren<Button>().onClick.AddListener(() => LoadLastSave());
    }
}

[Serializable]
public class SaveObject
{
    public int goldAmount;
    public int incomeAmount;

    public int expAmount;
    public int currentLevel;

    public Vector3 playerPosition;

    public List<PlayerBuildedObjects> buildedObjects;
    public List<InventoryObject> inventory;
}

[Serializable]
public class PlayerBuildedObjects
{
    public Vector3 position;
    public DatabaseObject _object;
}
