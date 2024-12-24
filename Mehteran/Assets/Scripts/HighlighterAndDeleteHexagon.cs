using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlighterAndDeleteHexagon : MonoBehaviour
{
    [SerializeField] private Color highLightColor = new Color(1f, 1f, 0.5f);
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private GameObject currentHighlightedObject; 

    void Start()
    {
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null && child.CompareTag("HexTile"))
            {
                originalColors[child.gameObject] = renderer.material.color;
            }
        }
    }

    void Update()
    {
        HandleMouseHover();
        HandleMouseClick();
    }

    private void HandleMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hoveredObject = hit.collider.gameObject;

            if (hoveredObject.CompareTag("HexTile") && originalColors.ContainsKey(hoveredObject))
            {
                if (currentHighlightedObject != null && currentHighlightedObject != hoveredObject)
                {
                    ResetHighlight();
                }

                // Yeni objeyi highlight et
                Renderer renderer = hoveredObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = highLightColor;
                    currentHighlightedObject = hoveredObject;
                }
            }
        }
        else
        {
            // Eðer hiçbir objeye hover edilmiyorsa highlight'ý sýfýrla
            ResetHighlight();
        }
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Sol týk
        {
            if (currentHighlightedObject != null && currentHighlightedObject.CompareTag("HexTile"))
            {
                Destroy(currentHighlightedObject);
                currentHighlightedObject = null; // Highlight objesi artýk yok
            }
        }
    }

    private void ResetHighlight()
    {
        if (currentHighlightedObject != null)
        {
            Renderer renderer = currentHighlightedObject.GetComponent<Renderer>();
            if (renderer != null && originalColors.ContainsKey(currentHighlightedObject))
            {
                renderer.material.color = originalColors[currentHighlightedObject];
            }
            currentHighlightedObject = null;
        }
    }
}
