using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSources> audioSources;
    public enum SoundType
    {
        Background,
        FreeSpinBg,
        BtnStart,
        BtnStop,
        WheelSpinning,
        Win,
        Money,
        ForceMoney,
        StopWheel,
        StopWheelWithScatter,
        Anticipation,
        Popup
    }

    public void PlaySound(SoundType soundType)
    {
        AudioSources source = audioSources.FirstOrDefault(s => s.soundType == soundType);
        
        source?.audioSource.Play();
    }

    public void StopSound(SoundType soundType)
    {
        AudioSources source = audioSources.FirstOrDefault(s => s.soundType == soundType);
        
        source?.audioSource.Stop();
    }
}
