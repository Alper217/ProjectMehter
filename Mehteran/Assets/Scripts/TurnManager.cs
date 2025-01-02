// TurnManager.cs
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<CardManager> players; // Oyuncular�n listesi
    private int currentPlayerIndex = 0; // �u anki oyuncunun s�ras�

    public int CurrentPlayerIndex { get; internal set; }

    public void StartGame()
    {
        if (players.Count > 0)
        {
            StartTurn(); // �lk oyuncunun s�ras�n� ba�lat
        }
        else
        {
            Debug.LogError("TurnManager: Oyuncu listesi bo�!");
        }
    }

    public void EndTurn()
    {
        players[currentPlayerIndex].EndPlayerTurn(); // Aktif oyuncunun s�ras�n� bitir
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // S�radaki oyuncuya ge�
        StartTurn(); // Yeni oyuncunun s�ras�n� ba�lat
    }

    private void StartTurn()
    {
        players[currentPlayerIndex].StartPlayerTurn();
        Debug.Log($"S�ra Oyuncu {currentPlayerIndex + 1}'de!");
    }

    public CardManager GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }
}
