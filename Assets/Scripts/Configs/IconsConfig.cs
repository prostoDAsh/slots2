using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New icon config", menuName = "Icon Config")]
public class IconsConfig : ScriptableObject
{
    [SerializeField] private int[] IconId;
    [SerializeField] private Sprite[] spritesForIcons;
}
