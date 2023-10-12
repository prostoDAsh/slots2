using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(fileName = "New Game Config1", menuName = "Game Config1")]

    public class GameConfig1 : ScriptableObject
    {
        [SerializeField] private SymbolData[] symbols;
        [SerializeField] private FinalScreenData1[] finalScreens;
        [SerializeField] private int visibleSymbolsOnReel;

        public SymbolData[] Symbols => symbols;

        public FinalScreenData1[] FinalScreens => finalScreens;

        public int VisibleSymbolsOnReel => visibleSymbolsOnReel;

    }
}