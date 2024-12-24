using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Transform stackPosition;  // Deste pozisyonu
    public Transform handPosition;   // El pozisyonu
    public GameObject cardPrefab;

    public int cardCount = 10;       // Ba�lang�� kart say�s�
    public float stackOffset = 0.1f; // Deste kart aral���
    public float handSpacing = 1.5f; // Eldeki kartlar aras�ndaki mesafe

    private Stack<GameObject> cardStack = new Stack<GameObject>(); // Kart y���n�
    private List<GameObject> handCards = new List<GameObject>();  // Eldeki kartlar

    // Dizi i�inde 7 farkl� script tipi olacak.
    public MonoBehaviour[] cardScripts; // 7 script dosyas�n� buraya ataca��n�z dizi

    void Start()
    {
        CreateCardStack();
    }

    void CreateCardStack()
    {
        for (int i = 0; i < cardCount; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, stackPosition.position + Vector3.up * (i * stackOffset), Quaternion.identity);
            newCard.name = "Card " + (i + 1);

            // Kart prefab'�na Card script'ini ekliyoruz ve CardManager referans�n� ba�l�yoruz.
            Card cardComponent = newCard.AddComponent<Card>();
            cardComponent.SetManager(this);

            // Rastgele bir script se�ip, bu script'i karta ekliyoruz.
            int randomIndex = Random.Range(0, cardScripts.Length); // 0 ile cardScripts.Length aras�nda rastgele bir say�
            newCard.AddComponent(cardScripts[randomIndex].GetType()); // Se�ilen script'i karta ekle

            cardStack.Push(newCard);
        }
    }

    public void MoveCardToHand(GameObject card)
    {
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
            handCards[i].transform.rotation = targetRotation; // 90 derece d�nd�r�lm�� rotasyon atan�r.
        }
    }
}
