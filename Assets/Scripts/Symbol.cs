using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Symbol : MonoBehaviour
    {
        //скрипт отвечает за картинки (что будет в них назначено)

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            //_rectTransform = GetComponent<RectTransform>();
        }

        public void SetImage(Sprite image)
        {
            _image.sprite = image;
        }

        // public void Move(Vector3 targetPos, float speed)
        // {
        //     _rectTransform.DOLocalMove(targetPos, speed).SetEase(Ease.Linear)
        //         .OnComplete(() =>
        //         {
        //             _rectTransform.localPosition = new Vector3(0f, 400f, 0f);
        //         });
        // }
        
    }
}