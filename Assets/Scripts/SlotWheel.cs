using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine.UI;
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
        
        private int _winIndex;

        private Sequence _sequence;

        [SerializeField] private ButtonsPanel btnPanel;

        private List<CanvasGroup> _symbolsCanvasGroup;

        public WheelModel Model { get; } = new();
        
        private void Start()
        {
            _symbolsCanvasGroup = GetComponentsInChildren<CanvasGroup>().ToList();
            _symbols = GetComponentsInChildren<Symbol>().ToList();
            _spriteProvider = new SpriteProvider(gameConfig, wheelId - 1);
            foreach (Symbol symbol in _symbols)
            {
                SymbolModel symbolModel = Model.AddSymbol();
                symbol.Initialize(_spriteProvider, symbolModel);
            }
            
            btnPanel.OnStartButtonClick += StopAnimation;
        }

        public void SetWinIndex(int index)
        {
            _winIndex = index;
            Debug.Log(index);
            var correctSymbol = _symbols.FirstOrDefault(o => o.symbolId == index);
            _winSymbol = correctSymbol;
        }

        public void ScaleWin()
        {
            
            _sequence = DOTween.Sequence();
            _sequence.Join(_winSymbol.gameObject.transform.DOScale(1.3f, 2f))
                //.Join(_dark.DOFade(1f, 2f))
                .Join(_winSymbol.gameObject.transform.DOShakePosition(2f, 8f))
                .Append(_winSymbol.gameObject.transform.DOScale(1f, 2f))
                .Join(_winSymbol.gameObject.transform.DOShakePosition(2f, 8f));
            //.Join((_dark.DOFade(0f, 2f)));
            
            // for (int i = 0; i < _symbolsCanvasGroup.Count; i++)
            // {
            //     if (i != _winIndex)
            //     {
            //         _symbolsCanvasGroup[i].DOFade(0.2f, 0.3f);
            //         Debug.Log("потемнели");
            //     }
            // }
        }

        private void StopAnimation()
        {
            _sequence.Kill();
            for (int i = 0; i < _symbols.Count; i++)
            {
                _symbols[i].transform.DOScale(1f, 0.1f);
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