using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private WhoPlaysUI _whoPlays;
    [SerializeField] private EndGameUI _endGame;

    public void ShowWhoPlays(bool isAI, DiscColor color)
    {
        _whoPlays.SetWhoPlays(isAI, color);
    }

    public void ShowWin()
    {
        _endGame.ShowWin();
    }

    public void ShowFailed()
    {
        _endGame.ShowFailed();
    }

    public void ShowBoardComplete()
    {
        _endGame.ShowBoardComplete();
    }
}
