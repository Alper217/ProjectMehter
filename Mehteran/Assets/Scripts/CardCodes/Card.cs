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
                // E�er kart eldeyse se�mek i�in kullan�lacak.
                manager.SelectCard(gameObject);
            }
            else
            {
                // E�er kart destedeyse ele ta��r.
                manager.MoveCardToHand(gameObject);
            }
        }
        else
        {
            Debug.LogError("CardManager atanmad�!");
        }
    }
}
