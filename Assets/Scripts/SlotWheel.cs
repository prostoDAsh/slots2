using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Configs;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

namespace DefaultNamespace
{
    public class SlotWheel : MonoBehaviour
    {
        private SpriteProvider spriteProvider;

        private List<Symbol> symbols;
        
        private readonly List<Symbol> symbolsForDark = new List<Symbol>(3);

        [SerializeField] private GameConfig gameConfig;
        
        [SerializeField] private int wheelId;
        
        [SerializeField] private ButtonsPanel btnPanel;
        
        [SerializeField] private NumbersConfig numbersConfig;

        [SerializeField] public ParticleSystem wheelParticleSystem;

        private Symbol winSymbol;
        
        private Sequence sequence;
        
        private SlotMachineController slotMachineController;
        
        private bool isAnimationRunning;

        public WheelModel Model { get; } = new();
        
        private void Awake()
        {
            slotMachineController = GetComponentInParent<SlotMachineController>();
            wheelParticleSystem.Stop();
            WheelMath.Initialize(numbersConfig);
        }
        
        private void Start()
        {
            symbols = GetComponentsInChildren<Symbol>().ToList();

            spriteProvider = new SpriteProvider(gameConfig, wheelId - 1);
            foreach (Symbol symbol in symbols)
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
                slotMachineController.UpdateScoreText();
                if(!slotMachineController.isFreeSpinsRunning) return;
                slotMachineController.UpdateFsScoreText();
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
            for (int i = 0; i < symbols.Count; i++) 
            {
                symbols[i].particleSystem.Clear();
                symbols[i].particleSystem.Stop();
            }
            
            sequence.Kill();
            
            StopCoroutine(DarkSymbols());
            
            StartCoroutine(ForceDarkSymbols());
            
            slotMachineController.UpdateScoreTextImmediately();

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