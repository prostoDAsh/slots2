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

        private Func<Sprite> _nextSprite;
        
        private Func<Sprite>_nextRandomSprite;
        
        private Func<Sprite>_nextFinalSprite;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void Configure(SpriteProvider spriteProvider, SymbolModel model)
        {
            _spriteProvider = spriteProvider;
            _nextRandomSprite = () => _spriteProvider.GetNextRandomSprite();
            _nextFinalSprite = () => _spriteProvider.GetNextFinalSprite();
            _nextSprite = _nextRandomSprite;
            _model = model;
            _model.UpdateImage += UpdateImage;
            _model.ShowFinal += () => _nextSprite = _nextFinalSprite;
            UpdateImage();
            UpdatePosition();
        }

        private void UpdateImage()
        {
            _image.sprite = _nextSprite();
            _nextSprite = _nextRandomSprite;
        }

        public void UpdatePosition()
        {
            Vector3 current = transform.localPosition;
            double adjustedPosition = 400 - _model.Position * 200;
            transform.localPosition = new Vector3(current.x, (float)adjustedPosition, current.z);
        }
    }
}