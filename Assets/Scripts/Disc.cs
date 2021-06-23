using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    public void Initialize(bool isRed)
    {
        GetComponent<SpriteRenderer>().color = isRed ? Color.red : Color.black;
    }
}
