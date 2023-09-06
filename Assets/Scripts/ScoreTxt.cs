using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTxt : MonoBehaviour
{
    private TextMeshProUGUI _scoreTxt;
    [SerializeField] private TextMeshProUGUI moneyChangeTxt;
    private int _currentScore = 0;
    private int _targetScore = 0;
    private int _moneyChange = 0; 
    private readonly float _duration = 1.5f;
    private float _elapsedTime = 0.0f;

    private void Awake()
    {
        _scoreTxt = GetComponentInChildren<TextMeshProUGUI>(); 
        moneyChangeTxt.gameObject.SetActive(false); 
    }

    private void Start()
    {
        UpdateScore(_currentScore);
    }

    public void UpdateScore(int newScore)
    {
        _targetScore = newScore;
        _moneyChange = _targetScore - _currentScore; 
        _elapsedTime = 0.0f;
        StartCoroutine(AnimateScoreChange());
    }
    
    // public void UpdateScoreImmediately(int newScore)
    // {
    //     _scoreTxt.text += newScore.ToString();
    // }

    public IEnumerator AnimateScoreChange()
    {
        int startScore = _currentScore;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            float progress = _elapsedTime / _duration;
            _currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, _targetScore, progress));
            _scoreTxt.text = _currentScore.ToString();
            
            
            if (_moneyChange != 0)
            {
                moneyChangeTxt.gameObject.SetActive(true);
                moneyChangeTxt.text = (_moneyChange > 0 ? "+" : "") + _moneyChange.ToString();
                moneyChangeTxt.alpha = Mathf.Lerp(1.0f, 0.0f, progress); 
            }
            
            yield return null;
        }

        _currentScore = _targetScore;
        _scoreTxt.text = _currentScore.ToString();
        _moneyChange = 0;
        moneyChangeTxt.gameObject.SetActive(false); // Скрываем текст изменения денег
    }
}
