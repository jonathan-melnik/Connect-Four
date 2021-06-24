using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Discs can have one of two colors
public class Disc : MonoBehaviour
{
    public DiscColor Color { get; private set; }

    public void Initialize(DiscColor color)
    {
        Color = color;
        GetComponent<SpriteRenderer>().color = color == DiscColor.Red ? new Color(0.9f, 0.1f, 0.1f) : new Color(0.2f, 0.9f, 0.1f, 1);
    }
}

public enum DiscColor
{
    Red,
    Black
}
