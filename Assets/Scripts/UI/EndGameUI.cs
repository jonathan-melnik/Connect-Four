using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _win;
    [SerializeField] private GameObject _failed;
    [SerializeField] private GameObject _boardComplete;

    private void Awake()
    {
        _win.SetActive(false);
        _failed.SetActive(false);
        _boardComplete.SetActive(false);
    }

    public void ShowWin()
    {
        _win.SetActive(true);
    }

    public void ShowFailed()
    {
        _failed.SetActive(true);
    }

    public void ShowBoardComplete()
    {
        _boardComplete.SetActive(true);
    }
}
