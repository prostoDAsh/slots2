using UnityEngine;
using UnityEngine.UI;
namespace DefaultNamespace
{
    public class Symbol1 : MonoBehaviour
    {
        private SpriteProvider1 spriteProvider;
        
        private Image image;

        private SymbolModel model;

        [SerializeField] public int symbolId;

        private readonly float symbolHeight = 200f;

        public new ParticleSystem particleSystem;
        
        private void Awake()
        {
            image = GetComponent<Image>();
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        { 
            particleSystem.Stop();
        }

        public void Initialize(SpriteProvider1 spriteProvider, SymbolModel model)
        {
            this.spriteProvider = spriteProvider;
            this.model = model;
            this.model.Moving += UpdatePosition;
            this.model.ShowRandomImage += UpdateImage;
            this.model.ShowFinalImage += UpdateFinalImage;
            UpdateImage();
        }

        private void UpdatePosition(double position)
        {
            float y = symbolHeight * 2 - symbolHeight * (float)position;
            transform.localPosition = new Vector3(0, y, 0);
        }

        private void UpdateImage() 
        {
            image.sprite = spriteProvider.GetNextRandomSprite(); 
        }

        private void UpdateFinalImage(int finalIndex) 
        {
            image.sprite = spriteProvider.GetFinalSprite(finalIndex);
        }
    }
}