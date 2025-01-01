using System.Collections.Generic;
using UnityEngine;

public class HorseManMovementTwo : MonoBehaviour
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
        newPosition.y += 6.5f; // Karakterin yüksekliðini ayarlamak için
        playerObject.transform.position = newPosition;

        ClearHighlights();
        isSelected = false;

        Renderer playerRenderer = playerObject.GetComponent<Renderer>();
        if (playerRenderer != null && originalMaterials.ContainsKey(playerObject))
        {
            playerRenderer.material = originalMaterials[playerObject];
        }

        // Yeni konum ve yönü güncelle
        UpdateCurrentHex();
    }

    void HighlightValidTiles()
    {
        validHexes.Clear();
        UpdateCurrentHex(); // Mevcut bloðu tespit et

        Vector3 currentPos = playerObject.transform.position;

        // Geri yönde 1 birim (düz)
        Vector3 forwardPos = currentPos + (playerObject.transform.forward * 20f);
        HighlightTileAtPosition(forwardPos);

        // Ýleri sað çapraz (2 birim)
        Vector3 backwardRightPos = currentPos -
                                  (playerObject.transform.forward * 4f) +
                                  (playerObject.transform.right * 4f);
        HighlightTileAtPosition(backwardRightPos);

        // Ýleri sol çapraz (2 birim)
        Vector3 backwardLeftPos = currentPos -
                                 (playerObject.transform.forward * 4f) -
                                 (playerObject.transform.right * 4f);
        HighlightTileAtPosition(backwardLeftPos);
    }

    void HighlightTileAtPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 5f);
        foreach (var collider in colliders)
        {
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
