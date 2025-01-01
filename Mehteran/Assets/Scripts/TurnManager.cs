using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; } // Singleton Pattern

    [SerializeField]private CardManager currentPlayer;
    [SerializeField]private CardManager opponentPlayer;

    public CardManager CurrentPlayer => currentPlayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void InitializePlayers(CardManager player1, CardManager player2)
    {
        currentPlayer = player1;
        opponentPlayer = player2;

        // �lk s�ray� belirle
        currentPlayer.StartPlayerTurn();
        opponentPlayer.EndPlayerTurn();
    }

    public void EndTurn()
    {
        if (currentPlayer == null || opponentPlayer == null)
        {
            Debug.LogError("TurnManager: Oyuncular hen�z atanmad�!");
            return;
        }

        // S�ralar� de�i�tir
        currentPlayer.EndPlayerTurn();
        (currentPlayer, opponentPlayer) = (opponentPlayer, currentPlayer);
        currentPlayer.StartPlayerTurn();

        Debug.Log($"S�ra {currentPlayer.gameObject.name} oyuncusunda.");
    }
}
