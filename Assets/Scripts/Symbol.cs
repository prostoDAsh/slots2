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

        private readonly float _symbolHeight = 200f;

        public new ParticleSystem particleSystem;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        { 
            particleSystem.Stop();
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
            float y = _symbolHeight * 2 - _symbolHeight * (float)position;
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