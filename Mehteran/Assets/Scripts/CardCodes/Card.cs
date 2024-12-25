using UnityEngine;

public class Card : MonoBehaviour
{
    private CardManager manager;

    public void SetManager(CardManager cardManager)
    {
        manager = cardManager;
    }

    private void OnMouseDown()
    {
        if (manager != null)
        {
            if (manager.IsInHand(gameObject))
            {
                // Elde olan bir kartı seç
                manager.SelectCard(gameObject);
            }
            else
            {
                // Destede olan bir kartı ele taşı
                manager.MoveCardToHand(gameObject);
            }
        }
        else
        {
            Debug.LogError("CardManager atanmadı!");
        }
    }
}
