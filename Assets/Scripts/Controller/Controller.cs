using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Action<int> AddDiscAtColumn;
    protected Board _board;

    public void Initialize(Board board, bool canMove)
    {
        _board = board;
        CanMove(canMove);
    }

    public abstract void CanMove(bool canMove);
}
