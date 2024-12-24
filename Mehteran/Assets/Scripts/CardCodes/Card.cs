using UnityEngine;

public class Card : MonoBehaviour
{
    private CardManager manager;

    public void SetManager(CardManager cardManager)
    {
        manager = cardManager;
    }

    void OnMouseDown()
    {
        if (manager != null)
        {
            manager.MoveCardToHand(gameObject); 
        }
    }
}
