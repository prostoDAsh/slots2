using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    // public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource fsMusicSource;

    [SerializeField] private AudioSource btnStartSource;

    [SerializeField] private AudioSource btnStopSource;

    [SerializeField] private AudioSource spinningSource;

    [SerializeField] private AudioSource winSource;

    [SerializeField] private AudioSource moneySource;

    [SerializeField] private AudioSource forceMoneySource;

    [SerializeField] private AudioSource stopWheelSource;

    [SerializeField] private AudioSource stopWheelWithScatterSource;

    [SerializeField] private AudioSource anticipationSource;

    [SerializeField] private AudioSource popupSource;

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    public void PlayBg()
    {
        musicSource.Play();
    }

    public void StopBg()
    {
        musicSource.Stop();
    }

    public void PlayFsMusic()
    {
        fsMusicSource.Play();
    }

    public void StopFsMusic()
    {
        fsMusicSource.Stop();
    }

    public void PlayBtnStartSound()
    {
        btnStartSource.Play();
    }
    
    public void PlayBtnStopSound()
    {
        btnStopSource.Play();
    }

    public void PlaySpinningSound()
    {
        
        spinningSource.Play();
    }

    public void StopSpinningSound()
    {
        spinningSource.Stop();
    }

    public void PlayWinSound()
    {
        winSource.Play();
    }

    public void StopWinSound()
    {
        winSource.Stop();
    }

    public void PlayMoneySound()
    {
        moneySource.Play();
    }

    public void PlayForceMoneySound()
    {
        forceMoneySource.Play();
    }

    public void PlayStopWheelSound()
    {
        stopWheelSource.Play();
    }

    public void PlayStopWheelWithScatterSound()
    {
        stopWheelWithScatterSource.Play();
    }

    public void PlayAnticipationSound()
    {
        anticipationSource.Play();
    }

    public void StopAnticipationSound()
    {
        anticipationSource.Stop();
    }

    public void PlayPopupSound()
    {
        popupSource.Play();
    }
}
