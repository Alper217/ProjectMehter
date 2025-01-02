// TurnManager.cs
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    float roundIndex = 0;
    public List<CardManager> players; // Oyuncuların listesi
    private int currentPlayerIndex = 0; // Şu anki oyuncunun sırası

    public int CurrentPlayerIndex { get; internal set; }

    public void StartGame()
    {
        if (players.Count > 0)
        {
            StartTurn(); // İlk oyuncunun sırasını başlat
        }
        else
        {
            Debug.LogError("TurnManager: Oyuncu listesi boş!");
        }
    }

    public void EndTurn()
    {
        roundIndex += 1;
        players[currentPlayerIndex].EndPlayerTurn(); // Aktif oyuncunun sırasını bitir
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // Sıradaki oyuncuya geç
        StartTurn(); // Yeni oyuncunun sırasını başlat
        Debug.Log(roundIndex);

        // roundIndex 3 olduğunda kart çekme işlemini durdur
        if (roundIndex == 3)
        {
            foreach (var player in players)
            {
                player.StopDrawingCards(); // Kart çekme işlemini durdur
            }
            Debug.Log("Kart çekme işlemi durduruldu.");
        }
    }

    private void StartTurn()
    {
        players[currentPlayerIndex].StartPlayerTurn();
        Debug.Log($"Sıra Oyuncu {currentPlayerIndex + 1}'de!");
    }

    public CardManager GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }
}
