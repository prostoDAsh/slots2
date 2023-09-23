using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopupAfterFS : MonoBehaviour
{
    private TextMeshProUGUI popupText;
    
    private int totalScore;

    private GameObject popup;

    private void Awake()
    {
        popupText = GetComponentInChildren<TextMeshProUGUI>();
        popup = GameObject.FindGameObjectWithTag("Popup");
    }

    public void UpdatePopupTxt(int newScore)
    {
        totalScore += newScore;
        popupText.text = totalScore.ToString();
    }
}
