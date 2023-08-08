using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New window config", menuName = "window Config")]
public class WindowsConfig : ScriptableObject
{
   [SerializeField] private int[] window;


   public int[] Window
   {
      get => window;
   }
}
