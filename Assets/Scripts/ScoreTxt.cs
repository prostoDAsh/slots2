using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreTxt : MonoBehaviour
{
    private TextMeshProUGUI _scoreTxt;
    
    [SerializeField] private TextMeshProUGUI moneyChangeTxt;
    
    private const float Duration = 1.5f;
    
    private int _currentScore;
    
    private int _targetScore;
    
    private int _moneyChange; 
    
    private float _elapsedTime;

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
    
    public void UpdateScoreImmediately(int newScore)
    { 
        _moneyChange = newScore - _currentScore;
        _scoreTxt.text = newScore.ToString();
        _currentScore = newScore;
    }

    private IEnumerator AnimateScoreChange()
    {
        var startScore = _currentScore;

        while (_elapsedTime < Duration)
        {
            _elapsedTime += Time.deltaTime;
            var progress = _elapsedTime / Duration;
            _currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, _targetScore, progress));
            _scoreTxt.text = _currentScore.ToString();
            
            if (_moneyChange != 0)
            {
                moneyChangeTxt.gameObject.SetActive(true);
                moneyChangeTxt.text = "+" + _moneyChange;
                moneyChangeTxt.alpha = Mathf.Lerp(1.0f, 0.0f, progress); 
            }
            
            yield return null;
        }

        _currentScore = _targetScore;
        _scoreTxt.text = _currentScore.ToString();
        _moneyChange = 0;
        moneyChangeTxt.gameObject.SetActive(false); 
    }
}
