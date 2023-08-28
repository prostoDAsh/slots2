using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using DG.Tweening;

namespace DefaultNamespace
{
    public class SlotWheel : MonoBehaviour
    {
        private SpriteProvider _spriteProvider;

        public Sprite[] sprites;
        
        private List<Symbol> _symbols;

        [SerializeField] private GameConfig gameConfig;
        
        [SerializeField] private int wheelId;

        public Symbol winSymbol;
        
        private int _winIndex;
        
        public WheelModel Model { get; } = new();
        
        private void Awake()
        {
            _symbols = GetComponentsInChildren<Symbol>().ToList();
            _spriteProvider = new SpriteProvider(gameConfig, wheelId - 1);
            foreach (Symbol symbol in _symbols)
            {
                SymbolModel symbolModel = Model.AddSymbol();
                symbol.Initialize(_spriteProvider, symbolModel);
            }
        }

        public void SetWinIndex(int index)
        {
            _winIndex = index;
            Debug.Log(index);
            var correctSymbol = _symbols.FirstOrDefault(o => o.symbolId == index);
            winSymbol = correctSymbol;
        }

        public void ScaleWin()
        {
            winSymbol.gameObject.transform.DOScale(1.5f, 1.5f)
                .OnComplete(() =>
                {
                    winSymbol.gameObject.transform.DOScale(1, 1.5f);
                });
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
        
    }
}