// TurnManager.cs
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<CardManager> players; // Oyuncularýn listesi
    private int currentPlayerIndex = 0; // Þu anki oyuncunun sýrasý

    public int CurrentPlayerIndex { get; internal set; }

    public void StartGame()
    {
        if (players.Count > 0)
        {
            StartTurn(); // Ýlk oyuncunun sýrasýný baþlat
        }
        else
        {
            Debug.LogError("TurnManager: Oyuncu listesi boþ!");
        }
    }

    public void EndTurn()
    {
        players[currentPlayerIndex].EndPlayerTurn(); // Aktif oyuncunun sýrasýný bitir
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // Sýradaki oyuncuya geç
        StartTurn(); // Yeni oyuncunun sýrasýný baþlat
    }

    private void StartTurn()
    {
        players[currentPlayerIndex].StartPlayerTurn();
        Debug.Log($"Sýra Oyuncu {currentPlayerIndex + 1}'de!");
    }

    public CardManager GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }
}
