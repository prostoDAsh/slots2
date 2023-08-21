using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class SlotWheel : MonoBehaviour
    {
        private readonly WheelModel _model = new();

        public Sprite[] sprites;
        private List<Symbol> _symbols;
        [SerializeField] private ButtonsPanel btnPanel;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private int wheelId;
        
        private void Awake()
       {
           _symbols = GetComponentsInChildren<Symbol>().ToList();
           var spriteProvider = new SpriteProvider(gameConfig, wheelId - 1);
           foreach (Symbol symbol in _symbols)
           {
               SymbolModel symbolModel = _model.AddSymbol();
               symbol.Configure(spriteProvider, symbolModel);
           }
       }
        
        private void Start()
        {
        }

        private void OnDestroy()
        {
        }

       private void Update()
       {
           _model.Update();
           foreach (Symbol symbol in _symbols)
           {
               symbol.UpdatePosition();
           }
       }

       public void StartMove()
       {
            _model.Start();
       }

       public void StopMove()
       {
           _model.Stop();
       }
    }
}