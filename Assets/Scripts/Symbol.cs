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

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Initialize(SpriteProvider spriteProvider, SymbolModel model)
        {
            _spriteProvider = spriteProvider;
            _model = model;
            _model.Moving += UpdatePosition;
            _model.ShowRandomImage += UpdateImage;
            _model.ShowFinalImage += UpdateFinalImage;
            UpdateImage();
        }

        private void UpdatePosition(double position)
        {
            float y = 400f - 200f * (float)position;
            transform.localPosition = new Vector3(0, y, 0);
        }

        private void UpdateImage()
        {
            _image.sprite = _spriteProvider.GetNextRandomSprite();
        }

        private void UpdateFinalImage(int finalIndex)
        {
            _image.sprite = _spriteProvider.GetFinalSprite(finalIndex);
        }
    }
}