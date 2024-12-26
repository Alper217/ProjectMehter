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

    public bool IsAbondoned = false;
    public bool IsThrowed = false;

    public bool IsMyTurn { get; private set; } // Oyuncunun sırası olup olmadığını kontrol eder

    public GameManager gameManager; // GameManager referansı

    private void Start()
    {
        CreateCardStack();
        Debug.Log($"{gameObject.name} için oyun başladı. Kart çekmek için karta tıklayın.");
    }

    private void CreateCardStack()
    {
        for (int i = 0; i < cardCount; i++)
        {
            // Her üç karttan sonra ek bir boşluk bırak
            float extraSpacing = (i / 3) * .5f;

            // Kartın pozisyonunu hesapla
            Vector3 cardPosition = stackPosition.position + Vector3.up * (i * stackOffset + extraSpacing);

            GameObject newCard = Instantiate(cardPrefab, cardPosition, Quaternion.identity);
            newCard.name = $"{gameObject.name} Card {i + 1}";

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
        if (IsMyTurn && cardStack.Contains(card) && handCards.Count < drawLimit && !hasGivenCardToOpponent && !hasPlayedCardToTarget)
        {
            cardStack.Pop();
            handCards.Add(card);
            ArrangeHandCards();
            Debug.Log($"{gameObject.name}: Kart eline taşındı: {card.name}");
        }
        else if (handCards.Count >= drawLimit)
        {
            Debug.Log($"{gameObject.name}: 3'ten fazla kart çekemezsin!");
        }
        else if (!IsMyTurn)
        {
            Debug.Log($"{gameObject.name}: Sıra sende değil!");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Bu kart destede değil!");
        }
    }

    public void SelectCard(GameObject card)
    {
        if (IsMyTurn && handCards.Contains(card))
        {
            selectedCard = card;
            Debug.Log($"{gameObject.name}: Kart seçildi: {card.name}");
        }
        else if (!IsMyTurn)
        {
            Debug.Log($"{gameObject.name}: Sıra sende değil!");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Bu kart elde değil!");
        }
    }

    public void GiveCardToOpponent()
    {
        if (IsMyTurn && selectedCard != null && handCards.Contains(selectedCard) && !hasGivenCardToOpponent)
        {
            selectedCard.transform.position = opponentPosition.position + Vector3.right * (opponentCards.Count * handSpacing);
            handCards.Remove(selectedCard);
            opponentCards.Add(selectedCard);
            selectedCard = null;
            hasGivenCardToOpponent = true;
            Debug.Log($"{gameObject.name}: Kart rakibe verildi.");

            if (hasPlayedCardToTarget)
            {
                gameManager.EndTurn(); // Sıra geçişini bildir
            }
        }
        else if (!IsMyTurn)
        {
            Debug.Log($"{gameObject.name}: Sıra sende değil!");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Rakibe verilecek bir kart seçilmedi veya elde değil!");
        }
        IsAbondoned = true;
    }

    public void MoveToTarget()
    {
        if (IsMyTurn && selectedCard != null && handCards.Contains(selectedCard) && !hasPlayedCardToTarget)
        {
            selectedCard.transform.position = targetPosition.position;
            handCards.Remove(selectedCard);
            selectedCard = null;
            hasPlayedCardToTarget = true;
            Debug.Log($"{gameObject.name}: Seçilen kart hedefe taşındı.");

            if (hasGivenCardToOpponent)
            {
                gameManager.EndTurn(); // Sıra geçişini bildir
            }
        }
        else if (!IsMyTurn)
        {
            Debug.Log($"{gameObject.name}: Sıra sende değil!");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Hedefe taşınacak kart seçilmedi veya elde değil!");
        }
        IsThrowed = true;
    }

    private void ArrangeHandCards()
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            Vector3 targetPosition = handPosition.position + Vector3.right * (i * handSpacing);
            handCards[i].transform.position = targetPosition;
        }
    }

    public void StartPlayerTurn()
    {
        IsMyTurn = true;
        hasGivenCardToOpponent = false;
        hasPlayedCardToTarget = false;
        Debug.Log($"{gameObject.name}: Sıra sende!");
    }

    public void EndPlayerTurn()
    {
        IsMyTurn = false;
        Debug.Log($"{gameObject.name}: Sıra bitti.");
    }
}
