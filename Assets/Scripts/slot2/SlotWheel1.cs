using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Configs;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class SlotWheel1 : MonoBehaviour
    {
        private SpriteProvider1 spriteProvider;

        private List<Symbol1> symbols;
        
        private readonly List<Symbol1> symbolsForDark = new List<Symbol1>(3);

        [SerializeField] private GameConfig1 gameConfig;
        
        [SerializeField] private int wheelId;
        
        [SerializeField] private ButtonsPanel btnPanel;
        
        [SerializeField] private NumbersConfig numbersConfig;

        [SerializeField] public ParticleSystem wheelParticleSystem;

        private Symbol1 winSymbol;
        
        private Sequence sequence;
        
        private Slot2MachineController slot2MachineController;
        
        private bool isAnimationRunning;

        public WheelModel1 Model { get; } = new();
        
        private void Awake()
        {
            slot2MachineController = GetComponentInParent<Slot2MachineController>();
            wheelParticleSystem.Stop();
            WheelMath.Initialize(numbersConfig);
        }
        
        private void Start()
        {
            symbols = GetComponentsInChildren<Symbol1>().ToList();

            spriteProvider = new SpriteProvider1(gameConfig, wheelId - 1);
            foreach (Symbol1 symbol in symbols)
            {
                SymbolModel symbolModel = Model.AddSymbol();
                symbol.Initialize(spriteProvider, symbolModel);
            }
            
            btnPanel.OnStartButtonClick += StopAnimation;
        }

        public void SetWinIndex(int index)
        {
            StartCoroutine(SetWinIndexC(index));
        }

        private IEnumerator SetWinIndexC(int index)
        {
            var correctSymbol = symbols.FirstOrDefault(o => o.symbolId == index);
            winSymbol = correctSymbol;

            yield return new WaitUntil((() => !isAnimationRunning));
            
            symbolsForDark?.Clear();

            foreach (var symbol in symbols)
            {
                if (symbol.symbolId != index)
                {
                   symbolsForDark?.Add(symbol);
                }
            }
        }
        
        public void ScaleWin()
        {
            StartCoroutine(DarkSymbols());
            
            sequence = DOTween.Sequence();
            
            for (int i = 0; i < symbols.Count; i++)
            {
                if (symbols[i] == winSymbol)
                {
                    symbols[i].particleSystem.Play();
                }
            }

            sequence.Join(winSymbol.gameObject.transform.DOScale(numbersConfig.EndValueForScaleAnimation, numbersConfig.DurationForScaleAnimation))
                .Join(winSymbol.gameObject.transform.DOShakePosition(numbersConfig.DurationForScaleAnimation, numbersConfig.StrengthForShake))
                .Append(winSymbol.gameObject.transform.DOScale(1f, numbersConfig.DurationForScaleAnimation))
                .Join(winSymbol.gameObject.transform.DOShakePosition(numbersConfig.DurationForScaleAnimation, numbersConfig.StrengthForShake)).OnComplete(()=>
            {
                slot2MachineController.UpdateScoreText();
                slot2MachineController.PlayMoneySource();
                if(!slot2MachineController.isFreeSpinsRunning) return;
                slot2MachineController.UpdateFsScoreText();
            });
        }
        
        private IEnumerator DarkSymbols()
        {
            isAnimationRunning = true;
            
            CanvasGroup[] canvasGroups = new CanvasGroup[symbolsForDark.Count];
            for (int i = 0; i < symbolsForDark.Count; i++)
            {
                canvasGroups[i] = symbolsForDark[i].GetComponent<CanvasGroup>();
                canvasGroups[i].alpha = numbersConfig.EndValueForAlpha;
            }

            yield return new WaitForSeconds(numbersConfig.DelayBetweenDarken);
            
            for (int i = 0; i < symbolsForDark.Count; i++)
            {
                canvasGroups[i].alpha = 1f;
            }
            
            for (int i = 0; i < symbols.Count; i++)
            {
                if (symbols[i] == winSymbol)
                {
                    symbols[i].particleSystem.Stop();
                }
            }

            isAnimationRunning = false;
        }
        
        private void StopAnimation()
        {
            if (isAnimationRunning)
            {
                slot2MachineController.PlayForceMoneySource();
            }
            for (int i = 0; i < symbols.Count; i++) 
            {
                symbols[i].particleSystem.Clear();
                symbols[i].particleSystem.Stop();
            }

            sequence.Kill();
            
            StopCoroutine(DarkSymbols());
            
            StartCoroutine(ForceDarkSymbols());
            
            slot2MachineController.UpdateScoreTextImmediately();

            foreach (var t in symbols)
            {
                t.transform.DOScale(1f, 0.1f);
            }
        }
        
        private IEnumerator ForceDarkSymbols()
        {
            for (int i = 0; i < symbolsForDark.Count; i++)
            {
                symbolsForDark[i].gameObject.GetComponent<CanvasGroup>().DOFade(1f, 0.1f);
            }
            
            yield break; 
        }
        
        private void Update()
        {
            Model.Update();// обновление позиций всех символов
        }

        public void StartMove()
        {
            spriteProvider.Reset();
            
            Model.Start();
        }

        public void StopMove()
        {
            Model.Stop();
        }

        private void OnDestroy()
        {
            btnPanel.OnStartButtonClick -= StopAnimation;
        }
    }
}