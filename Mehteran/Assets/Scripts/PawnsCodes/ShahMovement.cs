using System.Collections.Generic;
using UnityEngine;

public class ShahMovement : MonoBehaviour
{
    public GameObject playerObject;
    public Material selectedMaterial;
    public Material adjacentMaterial;
    private GameObject currentHex;
    private List<GameObject> adjacentHexes = new List<GameObject>();
    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    private bool isSelected = false;

    void Start()
    {
        UpdateCurrentHex();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == playerObject)
                {
                    ToggleSelection();
                }
                else if (isSelected && hit.collider.CompareTag("HexTile") && adjacentHexes.Contains(hit.collider.gameObject))
                {
                    MoveToTileCenter(hit.collider.gameObject);
                }
            }
        }
    }

    void ToggleSelection()
    {
        isSelected = !isSelected;

        if (isSelected)
        {
            Renderer playerRenderer = playerObject.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                if (!originalMaterials.ContainsKey(playerObject))
                {
                    originalMaterials[playerObject] = playerRenderer.material;
                }
                playerRenderer.material = selectedMaterial;
            }
            HighlightAdjacentTiles();
        }
        else
        {
            Renderer playerRenderer = playerObject.GetComponent<Renderer>();
            if (playerRenderer != null && originalMaterials.ContainsKey(playerObject))
            {
                playerRenderer.material = originalMaterials[playerObject];
            }
            ClearPreviousHighlights();
        }
    }

    void MoveToTileCenter(GameObject targetTile)
    {
        Vector3 newPosition = targetTile.transform.position;
        newPosition.y += 4.5f; 
        playerObject.transform.position = newPosition;

        ClearPreviousHighlights();
        isSelected = false;

        Renderer playerRenderer = playerObject.GetComponent<Renderer>();
        if (playerRenderer != null && originalMaterials.ContainsKey(playerObject))
        {
            playerRenderer.material = originalMaterials[playerObject];
        }
        UpdateCurrentHex();
    }

    void UpdateCurrentHex()
    {
        Ray ray = new Ray(playerObject.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("HexTile"))
        {
            currentHex = hit.collider.gameObject;
        }
    }

    void HighlightAdjacentTiles()
    {
        if (currentHex == null) return;

        Collider[] adjacentColliders = Physics.OverlapSphere(currentHex.transform.position, 10f);
        foreach (var col in adjacentColliders)
        {
            if (col.gameObject == currentHex) continue;

            Renderer renderer = col.GetComponent<Renderer>();
            if (renderer != null && col.CompareTag("HexTile"))
            {
                if (!originalMaterials.ContainsKey(col.gameObject))
                {
                    originalMaterials[col.gameObject] = renderer.material;
                }
                renderer.material = adjacentMaterial;
                adjacentHexes.Add(col.gameObject);
            }
        }
    }

    void ClearPreviousHighlights()
    {
        foreach (GameObject hex in adjacentHexes)
        {
            Renderer renderer = hex.GetComponent<Renderer>();
            if (renderer != null && originalMaterials.ContainsKey(hex))
            {
                renderer.material = originalMaterials[hex];
            }
        }

        adjacentHexes.Clear();

        if (originalMaterials.ContainsKey(playerObject))
        {
            playerObject.GetComponent<Renderer>().material = originalMaterials[playerObject];
        }

        originalMaterials.Clear();
    }
}
