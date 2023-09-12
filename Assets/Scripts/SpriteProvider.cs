using UnityEngine;
using DG.Tweening;

namespace DefaultNamespace
{
    public sealed class SpriteProvider //представляет собой спрайты и имеет функционал для получения случайных и финальных спрайтов
    {
        private readonly GameConfig _config;

        private readonly int _wheelIndex;

        private int _nextFinalSet = -1;

        private readonly Symbol _symbol;

        private const int SymbolsOnWheel = 3;

        public SpriteProvider(GameConfig config, int wheelIndex) //конструктор
        {
            _config = config;
            _wheelIndex = wheelIndex;
        }

        public Sprite GetFinalSprite(int finalIndex)
        {
            int[] finalScreen = _config.FinalScreens[_nextFinalSet].FinalScreen;
            int index = _wheelIndex * SymbolsOnWheel + finalIndex;
            SymbolData data = _config.Symbols[finalScreen[index]];

            return data.SymbolImage;
        }

        public Sprite GetNextRandomSprite()
        {
            var random = Random.Range(0, _config.Symbols.Length);
            return _config.Symbols[random].SymbolImage;
        }

        public void Reset() 
        {
            _nextFinalSet = (_nextFinalSet + 1) % _config.FinalScreens.Length;
        }
    }
}