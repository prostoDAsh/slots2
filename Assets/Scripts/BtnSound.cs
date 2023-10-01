using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BtnSound : MonoBehaviour
{
    [SerializeField] private AudioSource btnSound;

    private ButtonsPanel btnPanel;

    private void Awake()
    {
        btnPanel = GetComponentInParent<ButtonsPanel>();
    }

    private void Start()
    {
        btnPanel.OnStartButtonClick += ButtonClickSound;
        btnPanel.OnStopButtonClick += ButtonClickSound;
    }

    private void ButtonClickSound()
    {
        btnSound.Play();
    }

    private void OnDestroy()
    {
        btnPanel.OnStartButtonClick -= ButtonClickSound;
        btnPanel.OnStopButtonClick -= ButtonClickSound;
    }
}
