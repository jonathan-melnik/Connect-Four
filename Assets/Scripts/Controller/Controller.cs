using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Action<int> AddDiscAtColumn;
    protected Board _board;
    public DiscColor Color { get; private set; }
    public bool IsActive { get; protected set; }

    public void Initialize(Board board, DiscColor color)
    {
        _board = board;
        Color = color;
    }

    public virtual void CanMove(bool canMove)
    {
        IsActive = canMove;
    }
}
