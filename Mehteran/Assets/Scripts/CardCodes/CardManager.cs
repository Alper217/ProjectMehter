using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class CardManager : MonoBehaviour
{
    [SerializeField] private string playerTag = "FirstPlayerCard"; // Oyuncu kartı tag'i, Inspector üzerinden ayarlanabilir
    [SerializeField] private string opponentTag = "SecondPlayerCard"; // Rakip kartı tag'i, Inspector üzerinden ayarlanabilir

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
    private int currentDrawCount = 0; // Oyuncunun bu turda çektiği kart sayısı

    [Header("Scripts for Cards")]
    public Type[] scriptTypes = new Type[] // Inspector üzerinden eklenebilir script'ler
    {
        typeof(ArcherMovementOne),
        typeof(ArcherMovementTwo),
        typeof(HorseManMovementOne),
        typeof(HorseManMovementTwo),
        typeof(InfantryMovementOne),
        typeof(InfantryMovementTwo),
        typeof(ShahMovement)
    };

    private void Start()
    {

        CreateCardStack();
        Debug.Log($"{gameObject.name} için oyun başladı. Kart çekmek için karta tıklayın.");
    }

    private void CreateCardStack()
    {
        // Script dağılımını kontrol etmek için bir sayaç listesi oluştur
        Dictionary<Type, int> scriptCount = new Dictionary<Type, int>();
        foreach (Type scriptType in scriptTypes)
        {
            scriptCount[scriptType] = 0; // Başlangıçta tüm script türleri için sayaç sıfır
        }

        System.Random random = new System.Random(); // Rastgele seçim yapmak için
        for (int i = 0; i < cardCount; i++)
        {
            float extraSpacing = (i / 3) * .5f;
            // Kartın pozisyonunu hesapla
            Vector3 cardPosition = stackPosition.position + Vector3.up * (i * stackOffset + extraSpacing);

            GameObject newCard = Instantiate(cardPrefab, cardPosition, Quaternion.identity);
            newCard.name = $"{gameObject.name} Card {i + 1}";

            // Kart tag'ini ayarla
            newCard.tag = playerTag; // Kartlara oyuncu tag'ini ekle

            Card cardComponent = newCard.AddComponent<Card>();
            cardComponent.SetManager(this);

            // Rastgele bir script türü seç ve karta ata
            Type selectedScriptType = null;
            while (selectedScriptType == null)
            {
                Type potentialType = scriptTypes[random.Next(scriptTypes.Length)];
                if (scriptCount[potentialType] < 3)
                {
                    selectedScriptType = potentialType;
                    scriptCount[potentialType]++;
                }
            }

            newCard.AddComponent(selectedScriptType); // Seçilen scripti karta ekle
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
        if (IsMyTurn && cardStack.Contains(card) && currentDrawCount < drawLimit)
        {
            cardStack.Pop();
            handCards.Add(card);
            currentDrawCount++;
            ArrangeHandCards(); // Eldeki kartları düzenle
            Debug.Log($"{gameObject.name}: Kart eline taşındı: {card.name}");
        }
        else if (currentDrawCount >= drawLimit)
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
            // Rakibe kart verirken, kartın tag'ini rakip tag'ine ayarla
            selectedCard.tag = opponentTag;  // Rakip kartına özel tag ekle
            selectedCard.transform.position = opponentPosition.position + Vector3.right * (opponentCards.Count * handSpacing);

            handCards.Remove(selectedCard);
            opponentCards.Add(selectedCard);
            selectedCard = null;
            hasGivenCardToOpponent = true;

            Debug.Log($"{gameObject.name}: Kart rakibe verildi.");
        }
        else if (!IsMyTurn)
        {
            Debug.Log($"{gameObject.name}: Sıra sende değil!");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Rakibe verilecek bir kart seçilmedi veya elde değil!");
        }
    }

    public void MoveToTarget()
    {
        if (IsMyTurn && selectedCard != null && handCards.Contains(selectedCard) && !hasPlayedCardToTarget)
        {
            selectedCard.transform.position = targetPosition.position;
            handCards.Remove(selectedCard);
            selectedCard = null;
            hasPlayedCardToTarget = true;
            ArrangeHandCards(); // Eldeki kartları düzenle
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
        float totalWidth = (handCards.Count - 1) * handSpacing; // Kartların toplam genişliği
        Vector3 startPosition = handPosition.position - Vector3.right * (totalWidth / 2); // Ortadan başla

        for (int i = 0; i < handCards.Count; i++)
        {
            Vector3 targetPosition = startPosition + Vector3.right * (i * handSpacing);
            handCards[i].transform.position = targetPosition;
        }
    }
    public void UseCard()
    {
        if (IsMyTurn && selectedCard != null && handCards.Contains(selectedCard))
        {
            // Kartı elden kaldır ve temizle
            handCards.Remove(selectedCard);
            if (selectedCard.CompareTag("FirstPlayerCard"))
            {
                Destroy(selectedCard); // Kartı yok et
            }
            selectedCard = null; // Seçilen kartı sıfırla
            ArrangeHandCards(); // Eldeki kartları düzenle
            Debug.Log($"{gameObject.name}: Kart kullanıldı ve silindi.");
        }
        else if (!IsMyTurn)
        {
            Debug.Log($"{gameObject.name}: Sıra sende değil!");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Kullanılacak bir kart seçilmedi veya elde değil!");
        }
    }

    public void StartPlayerTurn()
    {
        currentDrawCount = 0; // Yeni turda çekilen kart sayısını sıfırla
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