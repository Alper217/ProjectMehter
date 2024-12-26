using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<CardManager> players; // Oyuncular�n listesi.
    private int currentPlayerIndex = 0; // �u anki oyuncunun s�ras�.

    private void Start()
    {
        StartTurn(); // �lk oyuncunun s�ras�n� ba�lat.
    }

    public void EndTurn()
    {
        players[currentPlayerIndex].EndPlayerTurn(); // Aktif oyuncunun s�ras�n� bitir.
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // S�radaki oyuncuya ge�.
        StartTurn(); // Yeni oyuncunun s�ras�n� ba�lat.
    }

    private void StartTurn()
    {
        players[currentPlayerIndex].StartPlayerTurn();
        Debug.Log($"S�ra Oyuncu {currentPlayerIndex + 1}'de!");
    }
}