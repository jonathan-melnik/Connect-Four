using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public override void CanMove(bool canMove)
    {
        if (canMove)
        {
            float delay = Random.Range(0.3f, 1.0f); // give some random delay for the AI
            StartCoroutine(DoMove(delay));
        }
    }

    private IEnumerator DoMove(float delay)
    {
        yield return new WaitForSeconds(delay);

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
}
