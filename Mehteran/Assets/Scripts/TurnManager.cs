using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public FirstPlayerCode firstPlayer;
    public SecondPlayerCode secondPlayer;

    void Start()
    {
        secondPlayer.enabled = false;
        Debug.Log("Oyuna Ho�geldiniz, Birinci oyuncu oyunu ba�lat�cakt�r!");
    }

    void Update()
    {
        if (firstPlayer.isMoved)
        {
            secondPlayer.enabled = true;
            secondPlayer.isMoved = false;
            firstPlayer.enabled = false;
            firstPlayer.isMoved = false; 
        }
        else if (secondPlayer.isMoved)
        {
            firstPlayer.enabled = true;
            firstPlayer.isMoved = false;
            secondPlayer.enabled = false;
        }
    }
}
