using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyChangeTxt;
    
    private TextMeshProUGUI scoreTxt;
    
    private const float Duration = 1.5f;
    
    private int currentScore;
    
    private int targetScore;
    
    private int moneyChange; 
    
    private float elapsedTime;

    private void Awake()
    {
        scoreTxt = GetComponentInChildren<TextMeshProUGUI>(); 
        moneyChangeTxt.gameObject.SetActive(false); 
    }

    private void Start()
    {
        UpdateScore(currentScore);
    }

    public void UpdateScore(int newScore)
    {
        targetScore = newScore;
        moneyChange = targetScore - currentScore; 
        elapsedTime = 0.0f;
        StartCoroutine(AnimateScoreChange());
    }
    
    public void UpdateScoreImmediately(int newScore)
    {
        scoreTxt.text = newScore.ToString();
        currentScore = newScore;
    }

    private IEnumerator AnimateScoreChange()
    {
        var startScore = currentScore;

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            var progress = elapsedTime / Duration;
            currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, progress));
            scoreTxt.text = currentScore.ToString();
            
            if (moneyChange != 0)
            {
                moneyChangeTxt.gameObject.SetActive(true);
                moneyChangeTxt.text = "+" + moneyChange;
                moneyChangeTxt.alpha = Mathf.Lerp(1.0f, 0.0f, progress); 
            }
            
            yield return null;
        }

        currentScore = targetScore;
        scoreTxt.text = currentScore.ToString();
        moneyChange = 0;
        moneyChangeTxt.gameObject.SetActive(false); 
    }
}
