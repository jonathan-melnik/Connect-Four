using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WhoPlaysUI : MonoBehaviour
{
    [SerializeField] TMP_Text _txt;

    public void SetWhoPlays(bool isAI, DiscColor color)
    {
        if (isAI)
        {
            _txt.text = "AI PLAYS\n";
        }
        else
        {
            _txt.text = "YOU PLAY\n";
        }
        _txt.text += color == DiscColor.Red ? "RED" : "BLACK";
    }
}
