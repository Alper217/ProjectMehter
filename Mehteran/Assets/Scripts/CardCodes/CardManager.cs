using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Transform stackPosition;  // Deste pozisyonu
    public Transform handPosition;   // El pozisyonu
    public GameObject cardPrefab;

    public int cardCount = 10;       // Baþlangýç kart sayýsý
    public float stackOffset = 0.1f; // Deste kart aralýðý
    public float handSpacing = 1.5f; // Eldeki kartlar arasýndaki mesafe

    private Stack<GameObject> cardStack = new Stack<GameObject>(); // Kart yýðýný
    private List<GameObject> handCards = new List<GameObject>();  // Eldeki kartlar

    // Dizi içinde 7 farklý script tipi olacak.
    public MonoBehaviour[] cardScripts; // 7 script dosyasýný buraya atacaðýnýz dizi

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

            // Kart prefab'ýna Card script'ini ekliyoruz ve CardManager referansýný baðlýyoruz.
            Card cardComponent = newCard.AddComponent<Card>();
            cardComponent.SetManager(this);

            // Rastgele bir script seçip, bu script'i karta ekliyoruz.
            int randomIndex = Random.Range(0, cardScripts.Length); // 0 ile cardScripts.Length arasýnda rastgele bir sayý
            newCard.AddComponent(cardScripts[randomIndex].GetType()); // Seçilen script'i karta ekle

            cardStack.Push(newCard);
        }
    }

    public void MoveCardToHand(GameObject card)
    {
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
            handCards[i].transform.rotation = targetRotation; // 90 derece döndürülmüþ rotasyon atanýr.
        }
    }
}
