using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public bool isPlayer1Turn = true; // �lk oyuncu s�ras� ba�ta

    public TileManager tileManager;

    private void Start()
    {
        SetPlayerTurn(player1); // �lk oyuncuyu ba�lat
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
        // E�er oyuncu se�ilirse, onu se�ip hareket ettirebiliriz.
        PlayerSelection playerSelection = currentPlayer.GetComponent<PlayerSelection>();
        if (playerSelection != null)
        {
            playerSelection.HandlePlayerAction();
        }
    }

    public void EndTurn()
    {
        isPlayer1Turn = !isPlayer1Turn; // S�ra de�i�tirilir.
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
        // Oyuncunun s�ras� ba�lad���nda g�rsel de�i�iklikler yap�labilir
        PlayerHighlighter.Instance.ClearHighlights(); // �nceki vurgulamalar temizlenir.
        tileManager.UpdateCurrentHex(player); // Oyuncunun mevcut hex alan� g�ncellenir.
    }
}
