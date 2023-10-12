using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class FreeSpinsScore1 : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI freeSpinsScorePlus;
            
        [SerializeField] private Slot2MachineController slot2MachineController;
            
        [SerializeField] private TextMeshProUGUI freeSpinsScoreTxt;
            
        private const float Duration = 1.5f;
            
        private int currentFreeSpinsScore;
            
        private int targetFreeSpinsScore;
            
        private int fsScorePlus;
            
        private float elapsedTime;
            
        private void Awake()
        {
            freeSpinsScorePlus.gameObject.SetActive(false);
        }
            
        public void UpdateFreeSpinsScore(int newFreeSpinsScore)
        { 
            if (slot2MachineController.isFreeSpinsRunning)
            {
                targetFreeSpinsScore = newFreeSpinsScore;
                elapsedTime = 0.0f;
                fsScorePlus = targetFreeSpinsScore - currentFreeSpinsScore;
                StartCoroutine(AnimateFreeSpinsScoreChange());
            }
        }
            
        private IEnumerator AnimateFreeSpinsScoreChange()
        { 
            var startScore = currentFreeSpinsScore;
            
            while (elapsedTime < Duration)
            {
                elapsedTime += Time.deltaTime;
                var progress = elapsedTime / Duration;
                currentFreeSpinsScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetFreeSpinsScore, progress));
                freeSpinsScoreTxt.text = currentFreeSpinsScore.ToString();
        
                if (fsScorePlus != 0)
                {
                    freeSpinsScorePlus.gameObject.SetActive(true);
                    freeSpinsScorePlus.text = fsScorePlus.ToString();
                    freeSpinsScorePlus.alpha = Mathf.Lerp(1.0f, 0.0f, progress);
                }
                    
                yield return null;
            }
                
            currentFreeSpinsScore = targetFreeSpinsScore;
            freeSpinsScoreTxt.text = currentFreeSpinsScore.ToString();
        }
    }
}