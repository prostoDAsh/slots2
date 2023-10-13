using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Slot2MachineController : MonoBehaviour
    {
        [SerializeField] private SlotWheel1 wheel1;

        [SerializeField] private SlotWheel1 wheel2;
        
        [SerializeField] private SlotWheel1 wheel3;
        
        [SerializeField] private SlotWheel1 wheel4;
        
        [SerializeField] private SlotWheel1 wheel5;
        
        [SerializeField] private FinalScreenData1[] finalScreenData;
    
        [FormerlySerializedAs("BtnPnl")] [SerializeField] private ButtonsPanel btnPnl;
    
        [SerializeField] public ScoreTxt score;

        [SerializeField] private GameConfig1 config;

        [SerializeField] private FreeSpinsScore1 freeSpinsScore;

        [SerializeField] private NumbersConfig numbersConfig;

        [SerializeField] private FreeSpinsScorePanel freeSpinsScorePanel;

        [SerializeField] private PopupAfterFS popup;

        [SerializeField] private AudioManager audioManager;

        [SerializeField] public Button menuButton;

        [SerializeField] private GameController gameController;

        [SerializeField] private Slot1MachineController slot1;
 
        private int currentFinalScreenIndex;
    
        public int totalScore;

        private int totalFreeSpinsScore;
    
        private Coroutine runningCoroutine;

        private List<int> winId;

        private int scoreAfterPopup;

        private bool isPopupCoroutineRunning = false;
    
        public bool isFreeSpinsRunning;

        private readonly int winSymbolsArrayQuantity = 3;
    
        private readonly int freeSpinsCount = 3;

        public event Action<int> ReturnMenu; 

        private void OnEnable()
        {
            score.CurrentScore = gameController.scoreBalance;
        }
        
        private void Start()
        {
            menuButton.onClick.AddListener(OnReturnMenu);
            freeSpinsScorePanel.gameObject.SetActive(false);
            btnPnl.stopButton.interactable = false;
            btnPnl.stopButton.transform.localScale = Vector3.zero;
        
            btnPnl.OnStartButtonClick += StartEveryWheelSpinning;
            btnPnl.OnStopButtonClick += StopEveryWheelSpinning;
            btnPnl.OnStartButtonClick += () => audioManager.PlaySound(AudioManager.SoundType.BtnStart);
            btnPnl.OnStopButtonClick += () => audioManager.PlaySound(AudioManager.SoundType.BtnStop);
            btnPnl.OnStartButtonClick += () => audioManager.StopSound(AudioManager.SoundType.Win);
        
            wheel1.Model.Starting += DisableStartButton;
            wheel5.Model.Started += EnableStopButton;
            wheel1.Model.Stopping += DisableStopButton;
            wheel5.Model.Stopped += EnableStartButton;

            wheel1.Model.Stopped += PlayStopWheelSource;
            wheel2.Model.Stopped += PlayStopWheelSource;
            wheel3.Model.Stopped += PlayStopWheelSource;
            wheel4.Model.Stopped += PlayStopWheelSource;
            wheel5.Model.Stopped += PlayStopWheelSource;
        }

        private void OnReturnMenu()
        {
            ReturnMenu?.Invoke(score.CurrentScore);
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
                    wheel4.SetWinIndex(winSymbols[3]);
                    wheel5.SetWinIndex(winSymbols[4]);
                }
            }
        }
        
        private void CalculateWin()
    {
        if (finalScreenData[currentFinalScreenIndex].WinSymbolsId != null 
            && finalScreenData[currentFinalScreenIndex].WinSymbolsId.Length >= winSymbolsArrayQuantity)
        {
            winId = finalScreenData[currentFinalScreenIndex].WinSymbolsId.ToList();

            SymbolData symbol1 = config.Symbols[winId[0]];
            SymbolData symbol2 = config.Symbols[winId[1]];
            SymbolData symbol3 = config.Symbols[winId[2]];
            SymbolData symbol4 = config.Symbols[winId[3]];
            SymbolData symbol5 = config.Symbols[winId[4]];

            if (symbol1.SymbolCoast != 0 && symbol2.SymbolCoast != 0 && symbol3.SymbolCoast != 0 && symbol4.SymbolCoast != 0 && symbol5.SymbolCoast != 0 )
            {
                int winAmount = (int)symbol1.SymbolCoast
                                + (int)symbol2.SymbolCoast
                                + (int)symbol3.SymbolCoast
                                + (int)symbol4.SymbolCoast
                                + (int)symbol5.SymbolCoast;
        
                totalScore += winAmount;
                slot1.totalScore = totalScore;
                
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

            totalFreeSpinsScore += freeSpinsCount;
            isFreeSpinsRunning = true;
        }
        
        public void UpdateScoreText()
        {
            score.UpdateScore(totalScore);
            StartCoroutine(ShowMenuBtn());
        }

        private IEnumerator ShowMenuBtn()
        {
            yield return new WaitForSeconds(numbersConfig.DelayShowMenuBtn);

            if (!isFreeSpinsRunning)
            {
                menuButton.transform.localScale = Vector3.one;
                menuButton.interactable = true;
            }

        }
        
        public void UpdateFsScoreText()
        {
            freeSpinsScorePanel.gameObject.SetActive(true);
            freeSpinsScore.UpdateFreeSpinsScore(totalFreeSpinsScore);
        }

        public void UpdateScoreTextImmediately()
        {
            if (totalScore != 0)
            {
                score.UpdateScoreImmediately(totalScore);
            }
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
        
            menuButton.transform.localScale = Vector3.zero;
            menuButton.interactable = false;
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
            menuButton.onClick.RemoveListener(OnReturnMenu);
        
            btnPnl.OnStartButtonClick -= StartEveryWheelSpinning;
            btnPnl.OnStopButtonClick -= StopEveryWheelSpinning;
            btnPnl.OnStartButtonClick -= () => audioManager.PlaySound(AudioManager.SoundType.BtnStart);
            btnPnl.OnStartButtonClick -= () => audioManager.PlaySound(AudioManager.SoundType.BtnStop);
            btnPnl.OnStartButtonClick -= () => audioManager.StopSound(AudioManager.SoundType.Win);
        
            wheel1.Model.Starting -= DisableStartButton;
            wheel5.Model.Started -= EnableStopButton;
            wheel1.Model.Stopping -= DisableStopButton;
            wheel5.Model.Stopped -= EnableStartButton;
        
            wheel1.Model.Stopped -= PlayStopWheelSource;
            wheel2.Model.Stopped -= PlayStopWheelSource;
            wheel3.Model.Stopped -= PlayStopWheelSource;
            wheel4.Model.Stopped -= PlayStopWheelSource;
            wheel5.Model.Stopped -= PlayStopWheelSource;
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
        audioManager.PlaySound(AudioManager.SoundType.WheelSpinning);
        wheel1.StartMove();
        yield return new WaitForSeconds(numbersConfig.DelayBetweenStartWheels);
        wheel2.StartMove();
        yield return new WaitForSeconds(numbersConfig.DelayBetweenStartWheels);
        wheel3.StartMove();
        yield return new WaitForSeconds(numbersConfig.DelayBetweenStartWheels);
        wheel4.StartMove();
        yield return new WaitForSeconds(numbersConfig.DelayBetweenStartWheels);
        wheel5.StartMove();
        
        CalculateWin();
        CheckForFreeSpins();
        
        yield return new WaitForSeconds(numbersConfig.DelayAfterStartToStopWheels);
        
        StopEveryWheelSpinning();
    }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator StopSpinning()
        {
            StopCoroutine(runningCoroutine);
        
            wheel1.StopMove();
            yield return new WaitForSeconds(numbersConfig.DelayBetweenStopWheels);
            wheel2.StopMove();
            yield return new WaitForSeconds(numbersConfig.DelayBetweenStopWheels);
            wheel3.StopMove();
            yield return new WaitForSeconds(numbersConfig.DelayBetweenStopWheels);
            wheel4.StopMove();

            if (!finalScreenData[currentFinalScreenIndex].HaveThreeScatters)
            {
                yield return new WaitForSeconds(numbersConfig.DelayBetweenStopWheels);
                wheel5.StopMove();
                yield return new WaitForSeconds(numbersConfig.DelayAfterStopToStartScaleAnimation);
                ScaleWheels();
                audioManager.StopSound(AudioManager.SoundType.WheelSpinning);
            
            }
            else
            {
                yield return new WaitForSeconds(numbersConfig.DelayBeforeIncrease);
                PlayParticleWithAlpha();
                audioManager.PlaySound(AudioManager.SoundType.Anticipation);
                wheel5.Model.IncreaseSpinSpeed();
                wheel5.Model.IncreaseTimeSpan();
            
                yield return new WaitForSeconds(numbersConfig.DelayBetweenIncreaseAndStop);
                wheel5.StopMove();
                wheel5.wheelParticleSystem.Stop();
            
                yield return new WaitForSeconds(numbersConfig.DelayAfterStopToStartScaleAnimation);
                wheel5.Model.ReturnSpinSpeedAndTimeSpan();
                ScaleWheels();
                audioManager.StopSound(AudioManager.SoundType.WheelSpinning);
                audioManager.StopSound(AudioManager.SoundType.Anticipation);
                audioManager.StopSound(AudioManager.SoundType.Background);
                audioManager.PlaySound(AudioManager.SoundType.FreeSpinBg);
            }
        }
        
        private void PlayParticleWithAlpha()
        {
            wheel5.wheelParticleSystem.Play();
            wheel5.wheelParticleSystem.gameObject.GetComponent<CanvasGroup>().DOFade(numbersConfig.AlphaValueForWheelParticle, numbersConfig.DurationForAlfaWheelParticle);
        }
        private void ScaleWheels()
        {
            if (finalScreenData[currentFinalScreenIndex].HaveWinLine)
            {
                audioManager.PlaySound(AudioManager.SoundType.Win);
                wheel1.ScaleWin();
                wheel2.ScaleWin();
                wheel3.ScaleWin();
                wheel4.ScaleWin();
                wheel5.ScaleWin();
            }
            if (finalScreenData[currentFinalScreenIndex].FsScreen)
            {
                totalFreeSpinsScore -= 1;
            
                UpdateFsScoreText();
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
            yield return new WaitForSeconds(1f);
        
            audioManager.PlaySound(AudioManager.SoundType.Popup);
            audioManager.StopSound(AudioManager.SoundType.FreeSpinBg);
            audioManager.PlaySound(AudioManager.SoundType.Background);

            popup.transform.DOScale(1f, numbersConfig.DurationForScalePopup).OnComplete((() =>
            {
                popup.transform.DOScale(0f, numbersConfig.DurationForScalePopup).OnComplete((() =>
                {
                    isPopupCoroutineRunning = false;

                    btnPnl.playButton.transform.localScale = Vector3.one;
                    btnPnl.playButton.interactable = true;
                
                    if (totalFreeSpinsScore <= 0)
                    {
                        freeSpinsScorePanel.gameObject.SetActive(false);
                    }
                    menuButton.transform.localScale = Vector3.one;
                    menuButton.interactable = true;
                }));
            }));
        }

        public void PlayMoneySource()
        {
            audioManager.PlaySound(AudioManager.SoundType.Money);
        }

        public void PlayForceMoneySource()
        {
            audioManager.PlaySound(AudioManager.SoundType.ForceMoney);
        }
    
        private void PlayStopWheelSource()
        {
            switch (finalScreenData[currentFinalScreenIndex].HaveThreeScatters)
            {
                case true:
                {
                    audioManager.PlaySound(AudioManager.SoundType.StopWheelWithScatter);
                    break;
                }
                case false:
                {
                    audioManager.PlaySound(AudioManager.SoundType.StopWheel);
                    break;
                }
            }
        }
    }
}
    
