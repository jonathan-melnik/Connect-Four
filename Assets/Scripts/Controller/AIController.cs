using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The AI controller handles input by a AI player
// The difficulty at which it plays can be set
public class AIController : Controller
{
    [Range(0, 9)]
    [SerializeField] private int _difficulty = 1;

    public override void CanMove(bool canMove)
    {
        base.CanMove(canMove);
        if (canMove)
        {
            float delay = Random.Range(0.5f, 1.5f); // give some random delay for the AI
            StartCoroutine(DoMove(delay));
        }
    }

    private IEnumerator DoMove(float delay)
    {
        yield return new WaitForSeconds(delay);

        int rand = Random.Range(0, 9);
        if (rand >= _difficulty)
        {
            MakeRandomMove();
        }
        else
        {
            MakeGoodMove();
        }
    }

    private void MakeRandomMove()
    {
        var availableColumns = new List<int>();
        for (int col = 0; col < Board.COLUMNS; col++)
        {
            if (_board.CanAddDiscAtColumn(col))
            {
                availableColumns.Add(col);
            }
        }

        var randomCol = availableColumns[Random.Range(0, availableColumns.Count)];
        AddDiscAtColumn?.Invoke(randomCol);
    }

    // Make a move that tries to be the best move possible
    // It will make a match if possible or prevent a match from the opponent
    private void MakeGoodMove()
    {
        int col = _board.GetGoodMove(Color);
        if (col == -1)
        {
            MakeRandomMove();
        }
        else
        {
            AddDiscAtColumn?.Invoke(col);
        }
    }
}
