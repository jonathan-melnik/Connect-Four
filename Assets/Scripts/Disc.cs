using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    public DiscColor Color { get; private set; }

    public void Initialize(DiscColor color)
    {
        Color = color;
        GetComponent<SpriteRenderer>().color = color == DiscColor.Red ? UnityEngine.Color.red : UnityEngine.Color.black;
    }
}

public enum DiscColor
{
    Red,
    Black
}
