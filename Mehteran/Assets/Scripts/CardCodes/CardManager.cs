using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Transform stackPosition;  // Deste pozisyonu
    public Transform handPosition;   // El pozisyonu
    public Transform opponentPosition; // Rakip pozisyonu
    public Transform targetPosition; // Hedef pozisyon
    public GameObject cardPrefab;

    public int cardCount = 21;        // Başlangıç kart sayısı 
    public float stackOffset = 0.1f; // Deste kart aralığı
    public float handSpacing = 15f;  // Eldeki kartlar arasındaki mesafe

    private Stack<GameObject> cardStack = new Stack<GameObject>(); // Kart yığını
    private List<GameObject> handCards = new List<GameObject>();  // Eldeki kartlar
    private List<GameObject> opponentCards = new List<GameObject>(); // Rakibin kartları
    private GameObject selectedCard = null; // Kullanıcı tarafından seçilen kart

    private int drawLimit = 3; // Oyuncunun toplam çekebileceği kart sayısı
    private bool hasGivenCardToOpponent = false; // Rakibe kart verilip verilmediğini kontrol eder
    private bool hasPlayedCardToTarget = false; // Hedefe kart oynanıp oynanmadığını kontrol eder

    private void Start()
    {
        CreateCardStack();
        Debug.Log("Oyun başladı. Kart çekmek için karta tıklayın.");
    }

    private void CreateCardStack()
    {
        for (int i = 0; i < cardCount; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, stackPosition.position + Vector3.up * (i * stackOffset), Quaternion.identity);
            newCard.name = "Card " + (i + 1);

            Card cardComponent = newCard.AddComponent<Card>();
            cardComponent.SetManager(this);
            cardStack.Push(newCard);

            BoxCollider collider = newCard.AddComponent<BoxCollider>();
            collider.size = new Vector3(1, 1, 1);
        }
    }

    public bool IsInHand(GameObject card)
    {
        return handCards.Contains(card);
    }

    public void MoveCardToHand(GameObject card)
    {
        if (cardStack.Contains(card) && handCards.Count < drawLimit && !hasGivenCardToOpponent && !hasPlayedCardToTarget)
        {
            cardStack.Pop();
            handCards.Add(card);
            ArrangeHandCards();
            Debug.Log($"Kart eline taşındı: {card.name}");
        }
        else if (handCards.Count >= drawLimit)
        {
            Debug.Log("3'ten fazla kart çekemezsin!");
        }
        else if (hasGivenCardToOpponent || hasPlayedCardToTarget)
        {
            Debug.Log("Kart verildikten veya oynandıktan sonra yeni kart çekemezsin!");
        }
        else
        {
            Debug.Log("Bu kart destede değil!");
        }
    }

    public void SelectCard(GameObject card)
    {
        if (handCards.Contains(card))
        {
            selectedCard = card;
            Debug.Log($"Kart seçildi: {card.name}");
        }
        else
        {
            Debug.Log("Bu kart elde değil!");
        }
    }

    public void GiveCardToOpponent()
    {
        if (selectedCard != null && handCards.Contains(selectedCard) && !hasGivenCardToOpponent)
        {
            selectedCard.transform.position = opponentPosition.position + Vector3.right * (opponentCards.Count * handSpacing); // Kartı rakip pozisyonuna taşı
            handCards.Remove(selectedCard); // Kartı elden çıkar
            opponentCards.Add(selectedCard); // Rakibin kartlarına ekle
            selectedCard = null; // Seçimi sıfırla
            hasGivenCardToOpponent = true; // Kartın verildiğini işaretle
            Debug.Log("Kart rakibe verildi.");
        }
        else if (hasGivenCardToOpponent)
        {
            Debug.Log("Zaten bir kart verdin!");
        }
        else
        {
            Debug.Log("Rakibe verilecek bir kart seçilmedi veya elde değil!");
        }
    }

    public void MoveToTarget()
    {
        if (selectedCard != null && handCards.Contains(selectedCard) && !hasPlayedCardToTarget)
        {
            selectedCard.transform.position = targetPosition.position; // Kartı hedef pozisyona taşı
            handCards.Remove(selectedCard); // El listesinden çıkar
            selectedCard = null; // Seçimi sıfırla
            hasPlayedCardToTarget = true; // Kartın oynandığını işaretle
            Debug.Log("Seçilen kart hedefe taşındı.");
        }
        else if (hasPlayedCardToTarget)
        {
            Debug.Log("Zaten bir kart oynadın!");
        }
        else
        {
            Debug.Log("Hedefe taşınacak kart seçilmedi veya elde değil!");
        }
    }

    private void ArrangeHandCards()
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            Vector3 targetPosition = handPosition.position + Vector3.right * (i * handSpacing);
            handCards[i].transform.position = targetPosition;
        }
    }
}
