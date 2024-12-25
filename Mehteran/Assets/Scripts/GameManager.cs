using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<CardManager> players; // Oyuncularýn listesi.
    private int currentPlayerIndex = 0; // Þu anki oyuncunun sýrasý.

    private void Start()
    {
        StartTurn(); // Ýlk oyuncunun sýrasýný baþlat.
    }

    public void EndTurn()
    {
        players[currentPlayerIndex].EndPlayerTurn(); // Aktif oyuncunun sýrasýný bitir.
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // Sýradaki oyuncuya geç.
        StartTurn(); // Yeni oyuncunun sýrasýný baþlat.
    }

    private void StartTurn()
    {
        players[currentPlayerIndex].StartPlayerTurn();
        Debug.Log($"Sýra Oyuncu {currentPlayerIndex + 1}'de!");
    }
}
