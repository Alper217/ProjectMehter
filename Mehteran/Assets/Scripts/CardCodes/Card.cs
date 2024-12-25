using UnityEngine;

public class Card : MonoBehaviour
{
    private CardManagerOne manager;

    public void SetManager(CardManagerOne cardManager)
    {
        manager = cardManager;
    }

    private void OnMouseDown()
    {
        if (manager != null)
        {
            if (manager.IsInHand(gameObject))
            {
                // Eðer kart eldeyse seçmek için kullanýlacak.
                manager.SelectCard(gameObject);
            }
            else
            {
                // Eðer kart destedeyse ele taþýr.
                manager.MoveCardToHand(gameObject);
            }
        }
        else
        {
            Debug.LogError("CardManager atanmadý!");
        }
    }
}
