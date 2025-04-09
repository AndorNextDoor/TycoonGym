using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GridBlocker : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(BlockGrid());
    }

    IEnumerator BlockGrid()
    {
        yield return new WaitForSeconds(1);
        Vector3 size = this.GetComponent<Renderer>().bounds.size;
        Vector2Int size2D = new Vector2Int(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.z));

        Renderer rend = this.GetComponent<Renderer>();
        Bounds bounds = rend.bounds;
        Vector3 leftForward = new Vector3(bounds.min.x, bounds.center.y, bounds.max.z);
        PlacementSystem.Instance.gridData.AddObjectAt(PlacementSystem.Instance.grid.WorldToCell(leftForward), size2D, 999, 999);
    }
    

}
