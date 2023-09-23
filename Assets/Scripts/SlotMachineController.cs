using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotMachineController : MonoBehaviour
{
    [SerializeField] private SlotWheel wheel1;
    
    [SerializeField] private SlotWheel wheel2;
    
    [SerializeField] private SlotWheel wheel3;

    [SerializeField] private FinalScreenData[] finalScreenData;
    
    [FormerlySerializedAs("BtnPnl")] [SerializeField] private ButtonsPanel btnPnl;
    
    [SerializeField] private ScoreTxt score;

    [SerializeField] private GameConfig config;

    [SerializeField] private FreeSpinsScore freeSpinsScore;

    [SerializeField] private NumbersConfig numbersConfig;
    
    private int currentFinalScreenIndex;
    
    private int totalScore;

    private int totalFreeSpinsScore;
    
    private Coroutine runningCoroutine;

    private PopupAfterFS popup;

    private List<int> winId;

    private int scoreAfterPopup;

    private bool isPopupCoroutineRunning = false;
    
    public bool isFreeSpinsRunning;

    private GameObject freeSpinsScorePanel;
    
    private void Awake()
    {
        popup = GetComponentInChildren<PopupAfterFS>();
        freeSpinsScorePanel = GameObject.FindGameObjectWithTag("FreeSpinsPanel");
    }

    private void Start()
    {
        btnPnl.stopButton.interactable = false;
        btnPnl.stopButton.transform.localScale = Vector3.zero;
        
        btnPnl.OnStartButtonClick += StartEveryWheelSpinning;
        btnPnl.OnStopButtonClick += StopEveryWheelSpinning;
        
        wheel1.Model.Starting += DisableStartButton;
        wheel3.Model.Started += EnableStopButton;
        wheel1.Model.Stopping += DisableStopButton;
        wheel3.Model.Stopped += EnableStartButton;
    }
    
    private void SetIndexes()
    {
        if (currentFinalScreenIndex >= 0 && currentFinalScreenIndex < finalScreenData.Length)
        {
            int[] winSymbols = finalScreenData[currentFinalScreenIndex].WinSymbols;

            if (finalScreenData[currentFinalScreenIndex].HaveWinLine)
            {
                wheel1.SetWinIndex(winSymbols[0]);
                wheel2.SetWinIndex(winSymbols[1]);
                wheel3.SetWinIndex(winSymbols[2]);
            }
        }
    }
    private void CalculateWin()
    {
        if (finalScreenData[currentFinalScreenIndex].WinSymbolsId != null 
            && finalScreenData[currentFinalScreenIndex].WinSymbolsId.Length >= 3)
        {
            winId = finalScreenData[currentFinalScreenIndex].WinSymbolsId.ToList();

            SymbolData symbol1 = config.Symbols[winId[0]];
            SymbolData symbol2 = config.Symbols[winId[1]];
            SymbolData symbol3 = config.Symbols[winId[2]];

            if (symbol1.SymbolCoast != 0 && symbol2.SymbolCoast != 0 && symbol3.SymbolCoast != 0 )
            {
                int winAmount = (int)symbol1.SymbolCoast
                                + (int)symbol2.SymbolCoast
                                + (int)symbol3.SymbolCoast;
        
                totalScore += winAmount;
                
                if (isFreeSpinsRunning)
                {
                    scoreAfterPopup += winAmount;
                    popup.UpdatePopupTxt(scoreAfterPopup);
                }
            }
        }
    }

    private void CheckForFreeSpins()
    {
        if (!finalScreenData[currentFinalScreenIndex].HaveThreeScatters) return;
        
        totalFreeSpinsScore += 3;
        isFreeSpinsRunning = true;
    }

    public void UpdateScoreText()
    {
        score.UpdateScore(totalScore);
    }

    public void UpdateFsScoreText()
    {
        freeSpinsScore.UpdateFreeSpinsScore(totalFreeSpinsScore);
    }

    public void UpdateScoreTextImmediately()
    {
       score.UpdateScoreImmediately(totalScore);
    }
    
    private void EnableStartButton()
    {
        if (finalScreenData[currentFinalScreenIndex].ShowPlayBtn)
        {
            isFreeSpinsRunning = false;
        }
        if (isFreeSpinsRunning) return;
        if (isPopupCoroutineRunning) return;

        btnPnl.playButton.transform.localScale = Vector3.one;
        btnPnl.playButton.interactable = true;
    }

    private void DisableStartButton()
    {
        btnPnl.playButton.interactable = false;
        btnPnl.playButton.transform.localScale = Vector3.zero;
    }

    private void EnableStopButton()
    {
        btnPnl.stopButton.interactable = true;
        btnPnl.stopButton.transform.localScale = Vector3.one;
    }

    private void DisableStopButton()
    {
        btnPnl.stopButton.interactable = false;
        btnPnl.stopButton.transform.localScale = Vector3.zero;
    }

    private void OnDestroy()
    {
        btnPnl.OnStartButtonClick -= StartEveryWheelSpinning;
        btnPnl.OnStopButtonClick -= StopEveryWheelSpinning;
        
        wheel1.Model.Starting -= DisableStartButton;
        wheel3.Model.Started -= EnableStopButton;
        wheel1.Model.Stopping -= DisableStopButton;
        wheel3.Model.Stopped -= EnableStartButton;
    }

    private void StartEveryWheelSpinning()
    {
        runningCoroutine = StartCoroutine(StartSpinning());
        
        SetIndexes();
    }

    private void StopEveryWheelSpinning()
    {
        StartCoroutine(StopSpinning());
    }
    
    private IEnumerator StartSpinning()
    {
        
        wheel1.StartMove();
        yield return new WaitForSeconds(numbersConfig.DelayBetweenStartWheels);
        wheel2.StartMove();
        yield return new WaitForSeconds(numbersConfig.DelayBetweenStartWheels);
        wheel3.StartMove();
        
        CalculateWin();
        CheckForFreeSpins();
        
        yield return new WaitForSeconds(numbersConfig.DelayAfterStartToStopWheels);
        
        StopEveryWheelSpinning();
    }

    private IEnumerator StopSpinning()
    {
        StopCoroutine(runningCoroutine);
        
        wheel1.StopMove();
        yield return new WaitForSeconds(numbersConfig.DelayBetweenStopWheels);
        wheel2.StopMove();
        yield return new WaitForSeconds(numbersConfig.DelayBetweenStopWheels);
        wheel3.StopMove();
        yield return new WaitForSeconds(numbersConfig.DelayAfterStopToStartScaleAnimation);
        
        ScaleWheels();
        
    }

    private void ScaleWheels()
    {
        if (finalScreenData[currentFinalScreenIndex].HaveWinLine)
        {
            wheel1.ScaleWin();
            wheel2.ScaleWin();
            wheel3.ScaleWin();
        }
        if (finalScreenData[currentFinalScreenIndex].FsScreen)
        {
            totalFreeSpinsScore -= 1;
            
            UpdateFsScoreText();
            Debug.Log(finalScreenData[currentFinalScreenIndex].FsScreen);
        }

        if (finalScreenData[currentFinalScreenIndex].LastFsScreen)
        {
            isPopupCoroutineRunning = true;
            StartCoroutine(ShowPopup());
            isFreeSpinsRunning = false;
        }
        
        AutoStartWheels();
        
        currentFinalScreenIndex = (currentFinalScreenIndex + 1) % finalScreenData.Length;
    }

    private void AutoStartWheels()
    {
        if (isFreeSpinsRunning
            && finalScreenData[currentFinalScreenIndex].HaveThreeScatters)
        {
            StartCoroutine(WaitForScatters());
        }
        
        if (isFreeSpinsRunning
            && finalScreenData[currentFinalScreenIndex].ScreenForFreeSpins)
        {
            StartCoroutine(WaitForMinusOneFsScore());
        }
    }

    private IEnumerator WaitForScatters()
    {
        yield return new WaitForSeconds(numbersConfig.DelayForAutoStartWithAnimation);
        
        StartEveryWheelSpinning();
    }

    private IEnumerator WaitForMinusOneFsScore()
    {
        switch (finalScreenData[currentFinalScreenIndex].HaveWinLine)
        {
            case true:
            {
                yield return new WaitForSeconds(numbersConfig.DelayForAutoStartWithAnimation);
                break;
            }
            case false:
            {
                yield return new WaitForSeconds(numbersConfig.DelayForAutoStartWithoutAnimation);
                break;
            }
        }
        
        StartEveryWheelSpinning();
    }

    private IEnumerator ShowPopup()
    {
        yield return new WaitForSeconds(numbersConfig.DelayForStartPopupAnimation);

        popup.transform.DOScale(1f, 1.5f).OnComplete((() =>
        {
            popup.transform.DOScale(0f, 1.5f).OnComplete((() =>
            {
                isPopupCoroutineRunning = false;

                btnPnl.playButton.transform.localScale = Vector3.one;
                btnPnl.playButton.interactable = true;
                
                if (totalFreeSpinsScore <= 0)
                {
                    freeSpinsScorePanel.gameObject.SetActive(false);
                }
            }));
        }));
    }
}
