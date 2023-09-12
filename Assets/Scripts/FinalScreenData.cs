using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu(fileName = "New Final Screen", menuName = "Final Screen")]
public class FinalScreenData : ScriptableObject
{
    [SerializeField] private int[] finalScreen;
    
    [SerializeField] private int[] winSymbols;

    [SerializeField] private bool haveWinLine;
    
    [SerializeField] private int[] winSymbolsId;
    public int[] FinalScreen => finalScreen;

    public int[] WinSymbols => winSymbols;
    
    public bool HaveWinLine
    { 
        get => haveWinLine;
        set => haveWinLine = value;
    }
    
    public int[] WinSymbolsId => winSymbolsId;
}
