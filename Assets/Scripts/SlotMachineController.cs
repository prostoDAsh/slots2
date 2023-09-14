using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotMachineController : MonoBehaviour
{
    [SerializeField] private SlotWheel wheel1;
    
    [SerializeField] private SlotWheel wheel2;
    
    [SerializeField] private SlotWheel wheel3;

    [SerializeField] private FinalScreenData[] finalScreenData;
    
    [FormerlySerializedAs("BtnPnl")] [SerializeField] private ButtonsPanel btnPnl;

    private int _currentFinalScreenIndex;

    private readonly float _delayBetweenStartWheels = 0.5f;

    private readonly float _delayBetweenStopWheels = 0.2f;

    [SerializeField] private ScoreTxt score;

    [SerializeField] private GameConfig config;

    [SerializeField] private FreeSpinsScore freeSpinsScore;
    
    private int _totalScore;

    private int _totalFreeSpinsScore;
    
    private Coroutine _runningCoroutine;

    public bool isFreeSpinsRunning;

    private PopupAfterFS _popup;

    private List<int> _winId;

    private int _scoreAfterPopup;

    private bool _isPopupCoroutineRunning = false;
    private void Awake()
    {
        _popup = GetComponentInChildren<PopupAfterFS>();
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
        if (_currentFinalScreenIndex >= 0 && _currentFinalScreenIndex < finalScreenData.Length)
        {
            int[] winSymbols = finalScreenData[_currentFinalScreenIndex].WinSymbols;

            if (finalScreenData[_currentFinalScreenIndex].HaveWinLine)
            {
                wheel1.SetWinIndex(winSymbols[0]);
                wheel2.SetWinIndex(winSymbols[1]);
                wheel3.SetWinIndex(winSymbols[2]);
            }
        }
    }
    private void CalculateWin()
    {
        if (finalScreenData[_currentFinalScreenIndex].WinSymbolsId != null 
            && finalScreenData[_currentFinalScreenIndex].WinSymbolsId.Length >= 3)
        {
            _winId = finalScreenData[_currentFinalScreenIndex].WinSymbolsId.ToList();

            SymbolData symbol1 = config.Symbols[_winId[0]];
            SymbolData symbol2 = config.Symbols[_winId[1]];
            SymbolData symbol3 = config.Symbols[_winId[2]];

            if (symbol1.SymbolCoast != 0 && symbol2.SymbolCoast != 0 && symbol3.SymbolCoast != 0 )
            {
                int winAmount = (int)symbol1.SymbolCoast
                                + (int)symbol2.SymbolCoast
                                + (int)symbol3.SymbolCoast;
        
                _totalScore += winAmount;
                
                if (isFreeSpinsRunning)
                {
                    _scoreAfterPopup += winAmount;
                    _popup.UpdatePopupTxt(_scoreAfterPopup);
                }
            }
        }
    }

    private void CheckForFreeSpins()
    {
        if (!finalScreenData[_currentFinalScreenIndex].HaveThreeScatters) return;
        
        _totalFreeSpinsScore += 3;
        isFreeSpinsRunning = true;
    }

    public void UpdateScoreText()
    {
        score.UpdateScore(_totalScore);
    }

    public void UpdateFsScoreText()
    {
        freeSpinsScore.UpdateFreeSpinsScore(_totalFreeSpinsScore);
    }

    public void UpdateScoreTextImmediately()
    {
       score.UpdateScoreImmediately(_totalScore);
    }
    
    private void EnableStartButton()
    {
        if (finalScreenData[_currentFinalScreenIndex].ShowPlayBtn)
        {
            isFreeSpinsRunning = false;
        }
        if (isFreeSpinsRunning) return;
        if (_isPopupCoroutineRunning) return;

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
        _runningCoroutine = StartCoroutine(StartSpinning());
        
        SetIndexes();
    }

    private void StopEveryWheelSpinning()
    {
        StartCoroutine(StopSpinning());
    }
    
    private IEnumerator StartSpinning()
    {
        
        wheel1.StartMove();
        yield return new WaitForSeconds(_delayBetweenStartWheels);
        wheel2.StartMove();
        yield return new WaitForSeconds(_delayBetweenStartWheels);
        wheel3.StartMove();
        
        CalculateWin();
        CheckForFreeSpins();
        
        yield return new WaitForSeconds(6.0f);
        
        StopEveryWheelSpinning();
    }

    private IEnumerator StopSpinning()
    {
        StopCoroutine(_runningCoroutine);
        
        wheel1.StopMove();
        yield return new WaitForSeconds(_delayBetweenStopWheels);
        wheel2.StopMove();
        yield return new WaitForSeconds(_delayBetweenStopWheels);
        wheel3.StopMove();
        yield return new WaitForSeconds(1.5f);
        
        ScaleWheels();
        
    }

    private void ScaleWheels()
    {
        if (finalScreenData[_currentFinalScreenIndex].HaveWinLine)
        {
            wheel1.ScaleWin();
            wheel2.ScaleWin();
            wheel3.ScaleWin();
        }
        if (finalScreenData[_currentFinalScreenIndex].FsScreen)
        {
            _totalFreeSpinsScore -= 1;
            
            UpdateFsScoreText();
            Debug.Log(finalScreenData[_currentFinalScreenIndex].FsScreen);
        }

        if (finalScreenData[_currentFinalScreenIndex].LastFsScreen)
        {
            _isPopupCoroutineRunning = true;
            StartCoroutine(ShowPopup());
            isFreeSpinsRunning = false;
        }
        
        AutoStartWheels();
        
        _currentFinalScreenIndex = (_currentFinalScreenIndex + 1) % finalScreenData.Length;
    }

    private void AutoStartWheels()
    {
        if (isFreeSpinsRunning
            && finalScreenData[_currentFinalScreenIndex].HaveThreeScatters)
        {
            StartCoroutine(WaitForScatters());
        }
        
        if (isFreeSpinsRunning
            && finalScreenData[_currentFinalScreenIndex].ScreenForFreeSpins)
        {
            StartCoroutine(WaitForMinusOneFsScore());
        }
    }

    private IEnumerator WaitForScatters()
    {
        yield return new WaitForSeconds(4f);
        
        StartEveryWheelSpinning();
    }

    private IEnumerator WaitForMinusOneFsScore()
    {
        switch (finalScreenData[_currentFinalScreenIndex].HaveWinLine)
        {
            case true:
            {
                yield return new WaitForSeconds(4f);
                break;
            }
            case false:
            {
                yield return new WaitForSeconds(1);
                break;
            }
        }
        
        StartEveryWheelSpinning();
    }

    private IEnumerator ShowPopup()
    {
        yield return new WaitForSeconds(4f);
        
        _popup.transform.DOScale(1f, 1.5f);

        yield return new WaitForSeconds(2f);

        _popup.transform.DOScale(0f, 1.5f).OnComplete((() =>
        {
            _isPopupCoroutineRunning = false;
        
            btnPnl.playButton.transform.localScale = Vector3.one;
            btnPnl.playButton.interactable = true;
        }));
    }
}
