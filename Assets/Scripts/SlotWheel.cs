using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace DefaultNamespace
{
    public class SlotWheel : MonoBehaviour
    {
       public Sprite[] sprites;
        private List<Symbol> _symbols;
        [SerializeField] private ButtonsPanel btnPanel;
        
        private float _speed;
        private const float BaseSpeed = 4f;
        private float _distanceCell;
        
        private static Vector3 endPos = new Vector3(0f, -400f, 0f);
        private float symbolHeight = 200;
        
        public bool isMove;
        private Coroutine _coroutine;
        private WheelsStates wheelStates = WheelsStates.notMoving;
        public event Action OnStopSpinning;
        
        private void Awake()
       {
           _symbols = GetComponentsInChildren<Symbol>().ToList();
       }
        
       private void Start()
       {
           OnStopSpinning += StopMove;
           
           foreach (var symbol in _symbols)
           {
               SetRandom(symbol);
           }
       }

       private void OnDestroy()
       {
           OnStopSpinning -= StopMove;
       }

       private void Update()
       {
           if (isMove)
           {
               if (wheelStates == WheelsStates.stopping)
               {
                   var distLocal = _distanceCell + 400; //
                   foreach (Symbol symbol in _symbols)
                   {
                       var y = distLocal;
                       while (y <= endPos.y)
                       {
                           y += (symbolHeight * 4); //
                       }

                       var d = new Vector3(
                           symbol.transform.localPosition.x, 
                           y, 
                           symbol.transform.localPosition.z);
                       symbol.transform.localPosition = d;
                       
                       distLocal -= symbolHeight;
                   }
               }
               else
               {
                   for (int i = 0; i < _symbols.Count; i++)
                   {
                       _symbols[i].transform.Translate(Vector3.down * _speed, Space.Self);
                       if (_symbols[i].transform.localPosition.y <= endPos.y)
                       {
                           var pos = _symbols[i].transform.localPosition;
                           pos.y = pos.y + 800;
                           SetRandom(_symbols[i]);
                           _symbols[i].transform.localPosition = pos;
                       }
                   }
               }
           }
       }


       public void StartMove()
       {
            wheelStates = WheelsStates.moving;
            DOTween.To(() => _speed, x => _speed = x, BaseSpeed, 0.5f).OnStart((() =>
           {
               isMove = true;
               _coroutine = StartCoroutine(StopTimer());
           }));
       }
       
       public void StopMove()
       {
           wheelStates = WheelsStates.stopping;
           
           if (_coroutine != null)
           {
               StopCoroutine(_coroutine);
               _coroutine = null;
           }

           float start = _symbols[0].transform.localPosition.y - 400; //позиция символа на старте остановки 
           float end = start - symbolHeight * 3; // позиция символа в которой он должен остановиться (неровная позиция)
           float endAdjusted = (float)Math.Round(end / symbolHeight) * symbolHeight; // выровненная позиция 
           _distanceCell = start; //ячейка для dotweena для хранения текущей позиции самого верхнего символа 
           
           DOTween.To(() => _distanceCell, x => _distanceCell = x, endAdjusted, 0.5f).OnComplete(() =>
           {
               Debug.Log($"{start} {end} {endAdjusted} {symbolHeight}");
               
               isMove = false;
               wheelStates = WheelsStates.notMoving;
               
               btnPanel.stopButton.interactable = false;
               btnPanel.stopButton.transform.localScale = Vector3.zero;
               
               btnPanel.playButton.transform.localScale = Vector3.one;
               btnPanel.playButton.interactable = true;
               //DoAction;
           });
       }
       
       private void DoAction()
       {
           //set reward
       }
       
       private IEnumerator StopTimer()
       {
           yield return new WaitForSeconds(5.0f);
           _coroutine = null;
           OnStopSpinning?.Invoke();
          
       }
       
       private void SetRandom(Symbol symbol)
        {
            var randomm = Random.Range(0, sprites.Length);
            symbol.SetImage(sprites[randomm]);
        }
    }
}