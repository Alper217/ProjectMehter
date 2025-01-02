using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<CardManager> players; // Oyuncuların listesi.
    private int currentPlayerIndex = 0; // Şu anki oyuncunun sırası.

    private void Start()
    {
        StartTurn(); // İlk oyuncunun sırasını başlat.
    }

    public void EndTurn()
    {
        players[currentPlayerIndex].EndPlayerTurn(); // Aktif oyuncunun sırasını bitir.
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // Sıradaki oyuncuya geç.
        StartTurn(); // Yeni oyuncunun sırasını başlat.
    }

    private void StartTurn()
    {
        players[currentPlayerIndex].StartPlayerTurn();
        Debug.Log($"Sıra Oyuncu {currentPlayerIndex + 1}'de!");
    }
}
