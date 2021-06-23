using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public override void CanMove(bool value)
    {
        enabled = value;
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
