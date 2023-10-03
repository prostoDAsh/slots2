using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopupAfterFS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText;
    
    private int totalScore;

    private GameObject popup;

    public void UpdatePopupTxt(int newScore)
    {
        totalScore += newScore;
        popupText.text = totalScore.ToString();
    }
}
