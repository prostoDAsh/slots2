using UnityEngine;
using DG.Tweening;

namespace DefaultNamespace
{
    public sealed class SpriteProvider //представляет собой спрайты и имеет функционал для получения случайных и финальных спрайтов
    {
        private readonly GameConfig config;

        private readonly int wheelIndex;

        private int nextFinalSet = -1;

        private readonly Symbol symbol;

        private const int SymbolsOnWheel = 3;

        public SpriteProvider(GameConfig config, int wheelIndex) //конструктор
        {
            this.config = config;
            this.wheelIndex = wheelIndex;
        }

        public Sprite GetFinalSprite(int finalIndex)
        {
            int[] finalScreen = config.FinalScreens[nextFinalSet].FinalScreen;
            int index = wheelIndex * SymbolsOnWheel + finalIndex;
            SymbolData data = config.Symbols[finalScreen[index]];

            return data.SymbolImage;
        }

        public Sprite GetNextRandomSprite()
        {
            var random = Random.Range(0, config.Symbols.Length);
            return config.Symbols[random].SymbolImage;
        }

        public void Reset() 
        {
            nextFinalSet = (nextFinalSet + 1) % config.FinalScreens.Length;
        }
    }
}