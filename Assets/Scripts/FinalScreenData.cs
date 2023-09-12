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
    
    [SerializeField] private int[] winSymbolsId;
    
    [SerializeField] private bool haveWinLine;
        
    [SerializeField] private bool haveThreeScatters;
    public int[] FinalScreen => finalScreen;

    public int[] WinSymbols => winSymbols;
    
    public bool HaveWinLine
    { 
        get => haveWinLine;
        set => haveWinLine = value;
    }
    
    public int[] WinSymbolsId => winSymbolsId;

    public bool HaveThreeScatters
    {
        get => haveThreeScatters;
        set => haveThreeScatters = value;
    }
}
