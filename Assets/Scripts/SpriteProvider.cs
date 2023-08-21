using UnityEngine;

namespace DefaultNamespace
{
    public sealed class SpriteProvider
    {
        private readonly GameConfig _config;

        private readonly int _wheelIndex;

        private int _nextFinalSet = -1;

        public SpriteProvider(GameConfig config, int wheelIndex)
        {
            _config = config;
            _wheelIndex = wheelIndex;
        }

        public Sprite GetFinalSprite(int finalIndex)
        {
            int[] finalScreen = _config.FinalScreens[_nextFinalSet].FinalScreen;
            int index = _wheelIndex * 3 + finalIndex;
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