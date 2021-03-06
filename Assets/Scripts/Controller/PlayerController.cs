using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player controller handles input by a human player
public class PlayerController : Controller
{
    public override void CanMove(bool canMove)
    {
        base.CanMove(canMove);
        enabled = canMove;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int col = _board.GetColumnWithMousePos(Input.mousePosition);
            if (col != -1)
            {
                AddDiscAtColumn?.Invoke(col);
            }
        }
    }
}
