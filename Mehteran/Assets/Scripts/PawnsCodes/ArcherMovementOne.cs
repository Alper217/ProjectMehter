using System.Collections.Generic;
using UnityEngine;

public class ArcherMovementOne : MonoBehaviour
{
    public GameObject playerObject;
    public Material selectedMaterial;
    public Material movableMaterial;
    private GameObject currentHex;
    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    private List<GameObject> validHexes = new List<GameObject>();
    private bool isSelected = false;

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
                else if (isSelected && validHexes.Contains(hit.collider.gameObject))
                {
                    MoveToTile(hit.collider.gameObject);
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
            HighlightValidTiles();
        }
        else
        {
            Renderer playerRenderer = playerObject.GetComponent<Renderer>();
            if (playerRenderer != null && originalMaterials.ContainsKey(playerObject))
            {
                playerRenderer.material = originalMaterials[playerObject];
            }
            ClearHighlights();
        }
    }

    void MoveToTile(GameObject targetTile)
    {
        Vector3 newPosition = targetTile.transform.position;
        newPosition.y += 2f; // Karakterin yüksekliðini ayarlamak için
        playerObject.transform.position = newPosition;

        ClearHighlights();
        isSelected = false;

        Renderer playerRenderer = playerObject.GetComponent<Renderer>();
        if (playerRenderer != null && originalMaterials.ContainsKey(playerObject))
        {
            playerRenderer.material = originalMaterials[playerObject];
        }
    }
    void HighlightValidTiles()
    {
        validHexes.Clear();
        UpdateCurrentHex(); // Mevcut bloðu tespit et

        Vector3 currentPos = playerObject.transform.position;

        // Ýleri yönde (sadece düz, sað-sol deðil)
        //for (int i = 1; i <= 3; i++)
        //{
           
        //}
        Vector3 forwardPos = currentPos + (playerObject.transform.forward * 15f); // Hex mesafesine göre ölçek
        HighlightTileAtPosition(forwardPos);
        // Geri yönde (sadece düz, sað-sol deðil)
        Vector3 backwardPos = currentPos - (playerObject.transform.forward * 10f); // Hex mesafesi geriye
        HighlightTileAtPosition(backwardPos);
    }

    void HighlightTileAtPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 2f); // Alan geniþliði artýrýldý
        foreach (var collider in colliders)
        {
            // Eðer hedef pozisyon, oyuncunun mevcut bloðuna denk geliyorsa atla
            if (collider.gameObject == currentHex)
                continue;

            if (collider.CompareTag("HexTile"))
            {
                Renderer renderer = collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (!originalMaterials.ContainsKey(collider.gameObject))
                    {
                        originalMaterials[collider.gameObject] = renderer.material;
                    }
                    renderer.material = movableMaterial;
                    validHexes.Add(collider.gameObject);
                }
            }
        }
    }

    void UpdateCurrentHex()
    {
        Ray ray = new Ray(playerObject.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f) && hit.collider.CompareTag("HexTile"))
        {
            currentHex = hit.collider.gameObject;
        }
    }
    void ClearHighlights()
    {
        foreach (var hex in validHexes)
        {
            Renderer renderer = hex.GetComponent<Renderer>();
            if (renderer != null && originalMaterials.ContainsKey(hex))
            {
                renderer.material = originalMaterials[hex];
            }
        }

        validHexes.Clear();

        if (originalMaterials.ContainsKey(playerObject))
        {
            playerObject.GetComponent<Renderer>().material = originalMaterials[playerObject];
        }

        originalMaterials.Clear();
    }
}
