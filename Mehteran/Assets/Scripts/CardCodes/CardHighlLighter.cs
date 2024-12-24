using UnityEngine;

public class CardHighlighter : MonoBehaviour
{
    private Renderer cardRenderer; // Kartýn Renderer'ýný kontrol etmek için.
    private Color originalColor;   // Kartýn orijinal rengi.
    private static CardHighlighter selectedCard; // Týklanan kartý takip etmek için.

    private void Start()
    {
        cardRenderer = GetComponent<Renderer>();
        if (cardRenderer != null)
        {
            originalColor = cardRenderer.material.color; // Kartýn ilk rengini kaydet.
        }
    }

    private void OnMouseEnter()
    {
        if (selectedCard != this) // Eðer bu kart seçili deðilse hover efekti uygula.
        {
            SetCardColor(new Color(1f, 1f, 0f, 0.5f)); // Sarý renk (hover).
        }
    }

    private void OnMouseExit()
    {
        if (selectedCard != this) // Eðer bu kart seçili deðilse normal rengi geri al.
        {
            ResetCardColor();
        }
    }

    private void OnMouseDown()
    {
        if (selectedCard != null && selectedCard != this)
        {
            selectedCard.ResetCardColor(); // Önceki seçili kartýn rengini sýfýrla.
        }

        selectedCard = this; // Bu kartý seçili yap.
        SetCardColor(new Color(1f, 0f, 0f, 0.5f)); // Kýrmýzý renk (seçili).
    }

    private void SetCardColor(Color color)
    {
        if (cardRenderer != null)
        {
            cardRenderer.material.color = color;
        }
    }

    private void ResetCardColor()
    {
        if (cardRenderer != null)
        {
            cardRenderer.material.color = originalColor;
        }
    }
}
