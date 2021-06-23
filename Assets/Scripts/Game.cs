using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] Controller _redController;
    [SerializeField] Controller _blackController;
    [SerializeField] Board _board;

    private void Start()
    {
        bool redStarts = Random.value < 0.5f;

        _redController.Initialize(_board, redStarts);
        _blackController.Initialize(_board, !redStarts);

        _redController.AddDiscAtColumn += OnAddedRedDiscAtColumn;
        _blackController.AddDiscAtColumn += OnAddedBlackDiscAtColumn;
    }

    private void OnAddedRedDiscAtColumn(int col)
    {
        if (!_board.CanAddDiscAtColumn(col))
        {
            return;
        }

        if (_board.AddDiscAtColumn(col))
        {
            ShowWin(DiscColor.Red);
        }
        else
        {
            _blackController.CanMove(true);
            _redController.CanMove(false);
        }
    }

    private void OnAddedBlackDiscAtColumn(int col)
    {
        if (!_board.CanAddDiscAtColumn(col))
        {
            return;
        }

        if (_board.AddDiscAtColumn(col))
        {
            ShowWin(DiscColor.Black);
        }
        else
        {
            _redController.CanMove(true);
            _blackController.CanMove(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void ShowWin(DiscColor color)
    {

    }
}
