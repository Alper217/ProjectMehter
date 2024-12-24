using UnityEngine;

public class CardHighlighter : MonoBehaviour
{
    private Renderer cardRenderer; // Kart�n Renderer'�n� kontrol etmek i�in.
    private Color originalColor;   // Kart�n orijinal rengi.
    public static CardHighlighter selectedCard; // T�klanan kart� takip etmek i�in.

    private void Start()
    {
        cardRenderer = GetComponent<Renderer>();
        if (cardRenderer != null)
        {
            originalColor = cardRenderer.material.color; // Kart�n ilk rengini kaydet.
        }
    }

    private void OnMouseEnter()
    {
        if (selectedCard != this) // E�er bu kart se�ili de�ilse hover efekti uygula.
        {
            SetCardColor(new Color(1f, 1f, 0f, 0.5f)); // Sar� renk (hover).
        }
    }

    private void OnMouseExit()
    {
        if (selectedCard != this) // E�er bu kart se�ili de�ilse normal rengi geri al.
        {
            ResetCardColor();
        }
    }

    private void OnMouseDown()
    {
        if (selectedCard != null && selectedCard != this)
        {
            selectedCard.ResetCardColor(); // �nceki se�ili kart�n rengini s�f�rla.
        }

        selectedCard = this; // Bu kart� se�ili yap.
        SetCardColor(new Color(1f, 0f, 0f, 0.5f)); // K�rm�z� renk (se�ili).
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