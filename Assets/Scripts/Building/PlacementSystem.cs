using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance;

    private DatabaseObject currentObject;

    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    public Grid grid;
    private InputManagerUpgradableItem inputManager;

    private GameObject selectedObject;
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 0.05f, 0.5f);

    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private LayerMask buildingGhostLayer;


    public GridData gridData;
    private Renderer[] previewRenderer;

    private List<GameObject> placedObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        gridVisualization.SetActive(false);
    }

    private void Start()
    {
        inputManager = InputManagerUpgradableItem.Instance;

        ExitPlacement();

        gridData = new GridData();
        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();
    }

    private void ExitPlacement()
    {
        currentObject = null;
        Destroy(selectedObject);

        HideGridAndIndicators();
    }

    public void SelectObjectToPlace(int ID)
    {
        gridVisualization.SetActive(true);

        currentObject = ObjectsDatabase.Instance.FindObjectByID(ID);
        selectedObject = Instantiate(currentObject.Prefab);

        DisableAllScriptsForSelectedObject();

        if (currentObject == null)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        ShowGridAndIndicators();
    }

    private void PlaceObject()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = InputManagerUpgradableItem.Instance.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, currentObject.ID);

        if (!placementValidity)
            return;
        // TO DO: Show that you can't build there

        currentObject = ObjectsDatabase.Instance.FindObjectByIndex(currentObject.ID);
        GameObject newStructure = Instantiate(currentObject.Prefab);
        newStructure.transform.position = grid.CellToWorld(gridPosition) + new Vector3(offset.x * currentObject.Size.x, offset.y, offset.z * currentObject.Size.y);

        InventoryManager.Instance.RemoveItemFromInventory(currentObject);

        placedObjects.Add(newStructure);
        
        gridData.AddObjectAt(gridPosition, currentObject.Size, currentObject.ID, placedObjects.Count - 1);

        ProgressionManager.Instance.GetExpirience();
        if(newStructure.transform.TryGetComponent<UpgradableItem>(out UpgradableItem upgradable))
        {
            upgradable.SetUpgradableStats(currentObject.Income, currentObject.Name, currentObject.Description, currentObject.Upgrades);
        }

        CustomerManager.Instance.AddNewItem(newStructure.transform);
        SaveManager.Instance.SaveBuildedObject(newStructure.transform.position, currentObject);


        ExitPlacement();
    }

    private void Update()
    {
        if (inputManager == null)
            return;

        if(currentObject == null)
        {
            return;
        }

        Vector3 mousePosition = InputManagerUpgradableItem.Instance.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, currentObject.ID);

        foreach(Renderer renderer in previewRenderer)
        {
            renderer.material.color = placementValidity ? Color.white : Color.red;
        }

        selectedObject.transform.position = cellIndicator.transform.position + new Vector3(offset.x * currentObject.Size.x, offset.y, offset.z * currentObject.Size.y);
         
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectID)
    {
        return gridData.CanPlaceObjectAt(gridPosition, currentObject.Size);
    }

    private void ShowGridAndIndicators()
    {
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceObject;
        inputManager.OnExit += ExitPlacement;
    }

    private void HideGridAndIndicators()
    {
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceObject;
        inputManager.OnExit -= ExitPlacement;
    }

    private void DisableAllScriptsForSelectedObject()
    {
        MonoBehaviour[] scripts = selectedObject.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        Collider[] colliders = selectedObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        if(selectedObject.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }

        //selectedObject.layer = buildingGhostLayer.value;
    }

}
