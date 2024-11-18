using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public bool isPlayer1Turn = true; // Ýlk oyuncu sýrasý baþta

    public TileManager tileManager;

    private void Start()
    {
        SetPlayerTurn(player1); // Ýlk oyuncuyu baþlat
    }

    private void Update()
    {
        if (isPlayer1Turn && Input.GetMouseButtonDown(0))
        {
            HandlePlayerTurn(player1);
        }
        else if (!isPlayer1Turn && Input.GetMouseButtonDown(0))
        {
            HandlePlayerTurn(player2);
        }
    }

    private void HandlePlayerTurn(GameObject currentPlayer)
    {
        // Eðer oyuncu seçilirse, onu seçip hareket ettirebiliriz.
        PlayerSelection playerSelection = currentPlayer.GetComponent<PlayerSelection>();
        if (playerSelection != null)
        {
            playerSelection.HandlePlayerAction();
        }
    }

    public void EndTurn()
    {
        isPlayer1Turn = !isPlayer1Turn; // Sýra deðiþtirilir.
        if (isPlayer1Turn)
        {
            SetPlayerTurn(player1);
        }
        else
        {
            SetPlayerTurn(player2);
        }
    }

    private void SetPlayerTurn(GameObject player)
    {
        // Oyuncunun sýrasý baþladýðýnda görsel deðiþiklikler yapýlabilir
        PlayerHighlighter.Instance.ClearHighlights(); // Önceki vurgulamalar temizlenir.
        tileManager.UpdateCurrentHex(player); // Oyuncunun mevcut hex alaný güncellenir.
    }
}
