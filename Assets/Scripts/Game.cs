using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] Controller _redController;
    [SerializeField] Controller _blackController;
    [SerializeField] Board _board;
    [SerializeField] UIManager _uiManager;

    private void Start()
    {
        bool redStarts = Random.value < 0.5f;

        _redController.Initialize(_board, DiscColor.Red);
        _blackController.Initialize(_board, DiscColor.Black);

        _redController.AddDiscAtColumn += OnAddedRedDiscAtColumn;
        _blackController.AddDiscAtColumn += OnAddedBlackDiscAtColumn;

        _redController.CanMove(redStarts);
        _blackController.CanMove(!redStarts);

        UpdateWhoPlaysUI();
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
        bool isAI = activeController is AIController;

        _blackController.CanMove(false);
        _redController.CanMove(false);

        _board.AddDiscAtColumn(col, color, isAI, OnDiscAdded);
    }

    private void OnDiscAdded(DiscColor color, bool isAI, bool didWin)
    {
        if (didWin)
        {
            if (isAI)
            {
                _uiManager.ShowFailed();
            }
            else
            {
                _uiManager.ShowWin();
            }
        }
        else if (_board.IsComplete())
        {
            _uiManager.ShowBoardComplete();
        }
        else
        {
            _blackController.CanMove(color == DiscColor.Red);
            _redController.CanMove(color == DiscColor.Black);
            UpdateWhoPlaysUI();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateWhoPlaysUI()
    {
        if (activeController != null)
        {
            _uiManager.ShowWhoPlays(activeController is AIController, activeController.Color);
        }
    }

    private Controller activeController
    {
        get
        {
            if (_blackController.IsActive)
            {
                return _blackController;
            }
            if (_redController.IsActive)
            {
                return _redController;
            }
            return null;
        }
    }
}
