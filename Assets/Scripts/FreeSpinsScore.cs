using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FreeSpinsScore : MonoBehaviour
{
    private TextMeshProUGUI _freeSpinsScoreTxt;
    [SerializeField] private TextMeshProUGUI freeSpinsScorePlus;
    private int _currentFreeSpinsScore;
    private int _targetFreeSpinsScore;
    private int _fsScorePlus;
    private const float Duration = 1.5f;
    private float _elapsedTime;
    public bool isFreeSpinsRunning = false;

    private void Awake()
    {
        _freeSpinsScoreTxt = GetComponentInChildren<TextMeshProUGUI>();
        freeSpinsScorePlus.gameObject.SetActive(false);
    }

    private void Start()
    {
       UpdateFreeSpinsScore(_currentFreeSpinsScore);
    }

    public void UpdateFreeSpinsScore(int newFreeSpinsScore)
    {
        _targetFreeSpinsScore = newFreeSpinsScore;
        _elapsedTime = 0.0f;
        _fsScorePlus = _targetFreeSpinsScore - _currentFreeSpinsScore;
        StartCoroutine(AnimateFreeSpinsScoreChange());
    }
    
    private IEnumerator AnimateFreeSpinsScoreChange()
    {
        var startScore = _currentFreeSpinsScore;

        while (_elapsedTime < Duration)
        {
            _elapsedTime += Time.deltaTime;
            var progress = _elapsedTime / Duration;
            _currentFreeSpinsScore = Mathf.RoundToInt(Mathf.Lerp(startScore, _targetFreeSpinsScore, progress));
            _freeSpinsScoreTxt.text = _currentFreeSpinsScore.ToString();

            if (_fsScorePlus != 0)
            {
                freeSpinsScorePlus.gameObject.SetActive(true);
                freeSpinsScorePlus.text = "+" + _fsScorePlus;
                freeSpinsScorePlus.alpha = Mathf.Lerp(1.0f, 0.0f, progress);
            }
            
            yield return null;
        }
        
        _currentFreeSpinsScore = _targetFreeSpinsScore;
        _freeSpinsScoreTxt.text = _currentFreeSpinsScore.ToString();
    }
}

