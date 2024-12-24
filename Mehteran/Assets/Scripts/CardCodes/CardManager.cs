using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Transform stackPosition;  // Deste pozisyonu
    public Transform handPosition;   // El pozisyonu
    public Transform opponentPosition; // Rakip pozisyonu
    public Transform targetPosition; // Bo� k�s�m
    public GameObject cardPrefab;

    public int cardCount = 21;       // Ba�lang�� kart say�s� 
    public float stackOffset = 0.1f; // Deste kart aral���
    public float handSpacing = 1.5f; // Eldeki kartlar aras�ndaki mesafe

    private Stack<GameObject> cardStack = new Stack<GameObject>(); // Kart y���n�
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
            // Yeni kart olu�tur
            GameObject newCard = Instantiate(cardPrefab, stackPosition.position + Vector3.up * (i * stackOffset), Quaternion.identity);
            newCard.name = "Card " + (i + 1);

            // Kart� deste y���n�na ekle
            Card cardComponent = newCard.AddComponent<Card>();
            cardComponent.SetManager(this);
            cardStack.Push(newCard);
        }
    }
    public void MoveCardToHand(GameObject card)
    {
        if (handCards.Count >= 3)
        {
            Debug.Log("Eldeki kart say�s� en fazla 3 olabilir!");
            return;
        }

        if (cardStack.Contains(card))
        {
            cardStack.Pop();
            handCards.Add(card); // Kart� el listesine ekle
            ArrangeHandCards();  // Eldeki kartlar� d�zenle
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
        
        if (selectedCard != null)  // E�er bir kart se�ildiyse
        {
            selectedCard.transform.position = targetPosition.position; // Se�ilen kart� hedef pozisyona ta��
            selectedCard = null;  // Kart� ta��d�ktan sonra se�imi s�f�rla
        }
        else Debug.Log("Kart Se�ilmedi!");
    }

    public void GiveCardToOpponent()
    {
        if (selectedCard != null)  // E�er bir kart se�ildiyse
        {
            selectedCard.transform.position = opponentPosition.position; // Kart� rakip pozisyonuna ta��
            selectedCard = null;  // Kart� ta��d�ktan sonra se�imi s�f�rla
        }
    }

    public void UseCard(GameObject card)
    {
        // Kullan butonuna atanacak metodun i�eri�i burada doldurulabilir
    }
}
