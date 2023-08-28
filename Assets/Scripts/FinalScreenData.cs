using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu(fileName = "New Final Screen", menuName = "Final Screen")]
public class FinalScreenData : ScriptableObject
{
    [SerializeField] private int[] finalScreen;
    public int[] FinalScreen => finalScreen;

    public int[] WinSymbols => winSymbols;

    [SerializeField] private int[] winSymbols;

}
