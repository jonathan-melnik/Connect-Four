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

        AddDiscAtColumn(col, DiscColor.Red);
    }

    private void OnAddedBlackDiscAtColumn(int col)
    {
        if (!_board.CanAddDiscAtColumn(col))
        {
            return;
        }

        AddDiscAtColumn(col, DiscColor.Black);
    }

    void AddDiscAtColumn(int col, DiscColor color)
    {
        _blackController.CanMove(false);
        _redController.CanMove(false);

        if (_board.AddDiscAtColumn(col, color))
        {
            ShowWin(color);
        }
        else if (_board.IsComplete())
        {
            ShowBoardComplete();
        }
        else
        {
            _blackController.CanMove(color == DiscColor.Red);
            _redController.CanMove(color == DiscColor.Black);
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

    private void ShowBoardComplete()
    {

    }
}
