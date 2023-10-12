using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyChangeTxt;

    [SerializeField] private TextMeshProUGUI scoreTxt;

    private const float Duration = 1.5f;

    private int targetScore;
    
    private int moneyChange; 
    
    private float elapsedTime;

    public int CurrentScore { get; set; }


    private void Awake()
    {
        moneyChangeTxt.gameObject.SetActive(false); 
    }

    private void OnEnable()
    {
        scoreTxt.text = CurrentScore.ToString();
    }

    public void UpdateScore(int newScore)
    {
        targetScore = newScore;
        moneyChange = targetScore - CurrentScore; 
        elapsedTime = 0.0f;
        StartCoroutine(AnimateScoreChange());
    }
    
    public void UpdateScoreImmediately(int newScore)
    {
        scoreTxt.text = newScore.ToString();
        CurrentScore = newScore;
    }

    private IEnumerator AnimateScoreChange()
    {
        var startScore = CurrentScore;

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            var progress = elapsedTime / Duration;
            CurrentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, progress));
            scoreTxt.text = CurrentScore.ToString();
            
            if (moneyChange != 0)
            {
                moneyChangeTxt.gameObject.SetActive(true);
                moneyChangeTxt.text = "+" + moneyChange;
                moneyChangeTxt.alpha = Mathf.Lerp(1.0f, 0.0f, progress); 
            }
            
            yield return null;
        }

        CurrentScore = targetScore;
        scoreTxt.text = CurrentScore.ToString();
        moneyChange = 0;
        moneyChangeTxt.gameObject.SetActive(false); 
    }
}
