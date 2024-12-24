using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Transform stackPosition;  // Deste pozisyonu
    public Transform handPosition;   // El pozisyonu
    public Transform opponentPosition; // Rakip pozisyonu
    public Transform targetPosition; // Boþ kýsým
    public GameObject cardPrefab;

    public int cardCount = 21;       // Baþlangýç kart sayýsý 
    public float stackOffset = 0.1f; // Deste kart aralýðý
    public float handSpacing = 1.5f; // Eldeki kartlar arasýndaki mesafe

    private Stack<GameObject> cardStack = new Stack<GameObject>(); // Kart yýðýný
    private List<GameObject> handCards = new List<GameObject>();  // Eldeki kartlar

    private Dictionary<System.Type, int> scriptCounters = new Dictionary<System.Type, int>();

    private GameObject selectedCard = null;

    void Start()
    {
        CreateCardStack();
    }

    void CreateCardStack()
    {
        for (int i = 0; i < cardCount; i++)
        {
            // Yeni kart oluþtur
            GameObject newCard = Instantiate(cardPrefab, stackPosition.position + Vector3.up * (i * stackOffset), Quaternion.identity);
            newCard.name = "Card " + (i + 1);

            // Kartý deste yýðýnýna ekle
            Card cardComponent = newCard.AddComponent<Card>();
            cardComponent.SetManager(this);
            cardStack.Push(newCard);
        }
    }
    public void MoveCardToHand(GameObject card)
    {
        if (handCards.Count >= 3)
        {
            Debug.Log("Eldeki kart sayýsý en fazla 3 olabilir!");
            return;
        }

        if (cardStack.Contains(card))
        {
            cardStack.Pop();
            handCards.Add(card); // Kartý el listesine ekle
            ArrangeHandCards();  // Eldeki kartlarý düzenle
        }
    }

    void ArrangeHandCards()
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            Vector3 targetPosition = handPosition.position + Vector3.right * (i * handSpacing);
            Quaternion targetRotation = Quaternion.Euler(0, 90f, 45f);

            handCards[i].transform.position = targetPosition;
            handCards[i].transform.rotation = targetRotation;
        }
    }
    public void MoveCardToTarget()
    {
        
        if (selectedCard != null)  // Eðer bir kart seçildiyse
        {
            selectedCard.transform.position = targetPosition.position; // Seçilen kartý hedef pozisyona taþý
            selectedCard = null;  // Kartý taþýdýktan sonra seçimi sýfýrla
        }
        else Debug.Log("Kart Seçilmedi!");
    }

    public void GiveCardToOpponent()
    {
        if (selectedCard != null)  // Eðer bir kart seçildiyse
        {
            selectedCard.transform.position = opponentPosition.position; // Kartý rakip pozisyonuna taþý
            selectedCard = null;  // Kartý taþýdýktan sonra seçimi sýfýrla
        }
    }

    public void UseCard(GameObject card)
    {
        // Kullan butonuna atanacak metodun içeriði burada doldurulabilir
    }
}
