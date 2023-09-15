using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopupAfterFS : MonoBehaviour
{
    private TextMeshProUGUI _popupText;
    
    private int _totalScore;

    private GameObject _popup;

    private void Awake()
    {
        _popupText = GetComponentInChildren<TextMeshProUGUI>();
        _popup = GameObject.FindGameObjectWithTag("Popup");
    }

    public void UpdatePopupTxt(int newScore)
    {
        _totalScore += newScore;
        _popupText.text = _totalScore.ToString();
    }
}
