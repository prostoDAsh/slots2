using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

namespace DefaultNamespace
{
    public class SlotWheel : MonoBehaviour
    {
        private SpriteProvider _spriteProvider;

        public Sprite[] sprites;
        
        private List<Symbol> _symbols;

        [SerializeField] private GameConfig gameConfig;
        
        [SerializeField] private int wheelId;

        private Symbol _winSymbol;

        private readonly List<Symbol> _symbolsForDark = new List<Symbol>(3);
        
        private int _winIndex;

        private Sequence _sequence;

        [SerializeField] private ButtonsPanel btnPanel;

        [SerializeField] private FreeSpinsScore freeSpinsScore;

        private bool _isCoroutineRunning;

        private SlotMachineController _slotMachineController;
        
        public WheelModel Model { get; } = new();

        private void Awake()
        {
            _slotMachineController = GetComponentInParent<SlotMachineController>();
        }

        private void Start()
        {
            _symbols = GetComponentsInChildren<Symbol>().ToList();

            _spriteProvider = new SpriteProvider(gameConfig, wheelId - 1);
            foreach (Symbol symbol in _symbols)
            {
                SymbolModel symbolModel = Model.AddSymbol();
                symbol.Initialize(_spriteProvider, symbolModel);
            }
            
            btnPanel.OnStartButtonClick += StopAnimation;
        }

        public async void SetWinIndex(int index)
        {
            _winIndex = index;
            var correctSymbol = _symbols.FirstOrDefault(o => o.symbolId == index);
            _winSymbol = correctSymbol;

            await UniTask.WaitUntil((() => !_isCoroutineRunning));
            _symbolsForDark?.Clear();

            foreach (var symbol in _symbols)
            {
                if (symbol.symbolId != index)
                {
                   _symbolsForDark?.Add(symbol);
                }
            }
        }
        
        public void ScaleWin()
        {
            StartCoroutine(DarkSymbols());
            
            _sequence = DOTween.Sequence();
            
            for (int i = 0; i < _symbols.Count; i++)
            {
                if (_symbols[i] == _winSymbol)
                {
                    _symbols[i].particleSystem.Play();
                }
            }
            
            _sequence.Join(_winSymbol.gameObject.transform.DOScale(1.3f, 2f))
                .Join(_winSymbol.gameObject.transform.DOShakePosition(2f, 8f))
                .Append(_winSymbol.gameObject.transform.DOScale(1f, 2f))
                .Join(_winSymbol.gameObject.transform.DOShakePosition(2f, 8f)).OnComplete(()=>
            {
                _slotMachineController.UpdateScoreText();
                _slotMachineController.UpdateFsScoreText();
            });
        }

        private IEnumerator DarkSymbols()
        {
            _isCoroutineRunning = true;
            
            for (int i = 0; i < _symbolsForDark.Count; i++)
            {
                _symbolsForDark[i].gameObject.GetComponent<CanvasGroup>().alpha = 0.4f;
            }
            
            yield return new WaitForSeconds(3.5f);
            
            for (int i = 0; i < _symbolsForDark.Count; i++)
            {
                _symbolsForDark[i].gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            }
            
            for (int i = 0; i < _symbols.Count; i++) 
            {
                if (_symbols[i] == _winSymbol)
                {
                    _symbols[i].particleSystem.Stop();
                }
            }
            
            _isCoroutineRunning = false;
        }

        private IEnumerator ForceDarkSymbols()
        {
            for (int i = 0; i < _symbolsForDark.Count; i++)
            {
                _symbolsForDark[i].gameObject.GetComponent<CanvasGroup>().DOFade(1f, 0.1f);
            }
            
            yield break;
        }

        private void StopAnimation()
        {
            for (int i = 0; i < _symbols.Count; i++) 
            {
                _symbols[i].particleSystem.Clear();
                _symbols[i].particleSystem.Stop();
            }
            
            _sequence.Kill();
            
            StopCoroutine(DarkSymbols());
            
            StartCoroutine(ForceDarkSymbols());
            
            _slotMachineController.UpdateScoreTextImmediately();

            foreach (var t in _symbols)
            {
                t.transform.DOScale(1f, 0.1f);
            }
        }
        
        private void Update()
        {
            Model.Update();// обновление позиций всех символов
        }

        public void StartMove()
        {
            _spriteProvider.Reset();
            
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