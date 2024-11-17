using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public GameObject playerObject;
    public Material adjacentMaterial;
    public Material enemyAdjacentMaterial;
    public Material selectedMaterial;
    public Material sideTwoHexMaterial; // MAVÝ BÖLGE ALANI
    // HEX ALANI
    private GameObject currentHex;
    private List<GameObject> adjacentHexes = new List<GameObject>();
    private List<GameObject> enemyHexes = new List<GameObject>();
    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();
    // BOOL KONTROL
    private bool isSelected = false;

    void Start()
    {
        UpdateCurrentHex(); //HEX GÜNCELLEME
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == playerObject)
                {                                                       //KARAKTERÝN ÝLERLETÝLMESÝ VE BÖLGE KONTROLÜ
                    ToggleSelection();
                }
                else if (isSelected && hit.collider.CompareTag("HexTile") && adjacentHexes.Contains(hit.collider.gameObject))
                {
                    MoveToTileCenter(hit.collider.gameObject);
                }
                else if (isSelected && hit.collider.CompareTag("HexTileEnemy") && enemyHexes.Contains(hit.collider.gameObject))
                {
                    CaptureEnemyTile(hit.collider.gameObject);
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
        newPosition.y += 3.5f;
        playerObject.transform.position = newPosition;

        ClearPreviousHighlights();
        isSelected = false;

        Renderer playerRenderer = playerObject.GetComponent<Renderer>();
        if (playerRenderer != null && originalMaterials.ContainsKey(playerObject))
        {
            playerRenderer.material = originalMaterials[playerObject];
        }
        Debug.Log("Oyuncu Bir birim ilerledi");
        UpdateCurrentHex();
    }

    void CaptureEnemyTile(GameObject enemyTile)
    {
        // Karakteri iþgal edilen alana taþý
        MoveToTileCenter(enemyTile);

        // Ýþgal edilen alanýn rengini maviye çevir ve etiketini HexTile olarak deðiþtir
        Renderer renderer = enemyTile.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = sideTwoHexMaterial; // HexTile için varsayýlan mavi materyale ayarla
            enemyTile.tag = "HexTile"; // HexTileEnemy'den HexTile'a geçiþ yap
        }
        Debug.Log("Oyuncu Düþman bölgesi ele geçirdi");
        ClearPreviousHighlights(); // Komþu alanlarý temizle
        UpdateCurrentHex(); // Yeni konumu güncelle
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
            if (renderer != null)
            {
                if (col.CompareTag("HexTile"))
                {
                    if (!originalMaterials.ContainsKey(col.gameObject))
                    {
                        originalMaterials[col.gameObject] = renderer.material;
                    }
                    renderer.material = adjacentMaterial;
                    adjacentHexes.Add(col.gameObject);
                }
                else if (col.CompareTag("HexTileEnemy"))
                {
                    if (!originalMaterials.ContainsKey(col.gameObject))
                    {
                        originalMaterials[col.gameObject] = renderer.material;
                    }
                    renderer.material = enemyAdjacentMaterial;
                    enemyHexes.Add(col.gameObject);
                   
                }
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

        foreach (GameObject enemyHex in enemyHexes)
        {
            Renderer renderer = enemyHex.GetComponent<Renderer>();
            if (renderer != null && originalMaterials.ContainsKey(enemyHex))
            {
                renderer.material = originalMaterials[enemyHex];
            }
        }

        adjacentHexes.Clear();
        enemyHexes.Clear();

        if (originalMaterials.ContainsKey(playerObject))
        {
            playerObject.GetComponent<Renderer>().material = originalMaterials[playerObject];
        }

        originalMaterials.Clear();
    }
}
