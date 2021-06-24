using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public WhoPlaysUI whoPlays;

    public void ShowWhoPlays(bool isAI, DiscColor color)
    {
        whoPlays.SetWhoPlays(isAI, color);
    }
}
