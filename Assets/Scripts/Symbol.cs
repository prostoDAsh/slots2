using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Symbol : MonoBehaviour
    {
        private SpriteProvider _spriteProvider;
        
        private Image _image;

        private SymbolModel _model;

        [SerializeField] public int symbolId;

        // private CanvasGroup _symbolCanvas;

        private void Awake()
        {
            _image = GetComponent<Image>();
            // _symbolCanvas = GetComponent<CanvasGroup>();
        }

        // public void isDark(bool isOn)
        // {
        //     if (isOn)
        //     {
        //         _symbolCanvas.DOFade(0.2f, 1f);
        //     }
        //     else
        //     {
        //         _symbolCanvas.DOFade(1f, 2f);
        //     }
        // }
        
        public void Initialize(SpriteProvider spriteProvider, SymbolModel model)
        {
            _spriteProvider = spriteProvider;
            _model = model;
            _model.Moving += UpdatePosition;
            _model.ShowRandomImage += UpdateImage;
            _model.ShowFinalImage += UpdateFinalImage;
            UpdateImage();
            //метод initialize для инициализации принимает два параметра и сохраняет их в соответствующие приватные поля
            //еще метод подписывается на определенные события из класса SymbolModel
        }

        private void UpdatePosition(double position)
        {
            float y = 400f - 200f * (float)position;
            transform.localPosition = new Vector3(0, y, 0);
        }

        private void UpdateImage() //обновляет изображение символа, на случайный
        {
            _image.sprite = _spriteProvider.GetNextRandomSprite(); 
        }

        private void UpdateFinalImage(int finalIndex) //устанавлиает окончельное изображение на основе переданного finalIndex
        {
            _image.sprite = _spriteProvider.GetFinalSprite(finalIndex);
        }
    }
}