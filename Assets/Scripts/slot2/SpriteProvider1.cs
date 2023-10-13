using DefaultNamespace.Configs;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpriteProvider1
    {
        private readonly GameConfig1 config;

        private readonly int wheelIndex;

        private int nextFinalSet = -1;

        private readonly Symbol symbol;

        private const int SymbolsOnWheel = 3;

        public SpriteProvider1(GameConfig1 config, int wheelIndex) //конструктор
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