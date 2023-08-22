using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class SlotWheel : MonoBehaviour
    {
        private SpriteProvider _spriteProvider;

        public Sprite[] sprites;
        
        private List<Symbol> _symbols;
        
        [SerializeField] private GameConfig gameConfig;
        
        [SerializeField] private int wheelId;
        
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