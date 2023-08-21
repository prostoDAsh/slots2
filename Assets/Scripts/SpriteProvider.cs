using UnityEngine;

namespace DefaultNamespace
{
    public sealed class SpriteProvider
    {
        private const int CurrentFinalSet = 0;
        
        private readonly GameConfig _config;
        
        private readonly int _wheelIndex;

        private int _finalIndex;

        public SpriteProvider(GameConfig config, int wheelIndex)
        {
            _config = config;
            _wheelIndex = wheelIndex;
        }

        public Sprite GetNextFinalSprite()
        {
            int[] finalScreen = _config.FinalScreens[CurrentFinalSet].FinalScreen;
            int index = _wheelIndex * 3 + _finalIndex % 3;
            SymbolData data = _config.Symbols[finalScreen[index]];
            _finalIndex++;

            return data.SymbolImage;
        }

        public Sprite GetNextRandomSprite()
        {
            var random = Random.Range(0, _config.Symbols.Length);
            return _config.Symbols[random].SymbolImage;
        }
    }
}