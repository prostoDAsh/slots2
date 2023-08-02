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
        
        private static Vector3 startPos = new Vector3(0f, 400f, 0f);
        private static Vector3 endPos = new Vector3(0f, -400f, 0f);
        
        private bool isMove;
        private Coroutine _coroutine;
       
       
       private void Awake()
       {
           _symbols = GetComponentsInChildren<Symbol>().ToList();
       }
       private void Start()
       {
          // btnPanel.OnStartButtonClick += StartMove;
         // btnPanel.OnStopButtonClick += StopMove;
           foreach (var symbol in _symbols)
           {
               SetRandom(symbol);
           }
       }
       
       private void OnDestroy()
       {
         //  btnPanel.OnStartButtonClick -= StartMove;
        // btnPanel.OnStopButtonClick -= StopMove;
       }
       
       private void Update()
       {
           if (isMove)
           {
               for (int i = 0; i < _symbols.Count; i++)
               {
                   _symbols[i].transform.Translate(Vector3.down * _speed, Space.Self);
                   if (_symbols[i].transform.localPosition.y <= endPos.y)
                   {
                       var pos = _symbols[i].transform.localPosition;
                       pos.y = startPos.y;
                       SetRandom(_symbols[i]);
                       _symbols[i].transform.localPosition = pos;
                   }
               }
           }
       }

       public void StartMove()
       {
           DOTween.To(() => _speed, x => _speed = x, BaseSpeed, 0.5f).OnStart((() =>
           {
               isMove = true;
               _coroutine = StartCoroutine(StopTimer());
           }));
       }
       
       public void StopMove()
       {
           if (_coroutine != null)
           {
               StopCoroutine(_coroutine);
               _coroutine = null;
           }
           DOTween.To(() => _speed, x => _speed = x, 0, 0.5f).OnComplete(() =>
           {
               isMove = false;
              
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
           StopMove();
       }
       
       private void SetRandom(Symbol symbol)
        {
            var randomm = Random.Range(0, sprites.Length);
            symbol.SetImage(sprites[randomm]);
        }
    }
}