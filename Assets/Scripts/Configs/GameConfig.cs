using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Config", menuName = "Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private SymbolData[] symbols;
    [SerializeField] private FinalScreenData[] finalScreens;
    [SerializeField] private WinLineData[] winLines;
    [SerializeField] private int visibleSymbolsOnReel;

    public SymbolData[] Symbols => symbols;

    public FinalScreenData[] FinalScreens => finalScreens;

    public int VisibleSymbolsOnReel => visibleSymbolsOnReel;

    public WinLineData[] WinLines => winLines;
}
