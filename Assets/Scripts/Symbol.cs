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
        }

        public void SetImage(Sprite image)
        {
            _image.sprite = image;
        }
        
        
    }
}