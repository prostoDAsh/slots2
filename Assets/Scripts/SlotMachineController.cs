using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SlotMachineController : MonoBehaviour
{
    public Transform[] wheels;
    
    [SerializeField] private SlotWheel wheel1;
    
    [SerializeField] private SlotWheel wheel2;
    
    [SerializeField] private SlotWheel wheel3;
    
    [SerializeField] private ButtonsPanel BtnPnl;

    private Coroutine _runningCoroutine;

    private void Start()
    {
        BtnPnl.stopButton.interactable = false;
        BtnPnl.stopButton.transform.localScale = Vector3.zero;
        
        BtnPnl.OnStartButtonClick += StartEveryWheelSpinning;
        BtnPnl.OnStopButtonClick += StopEveryWheelSpinning;
        
        wheel1.Model.Starting += DisableStartButton;
        wheel3.Model.Started += EnableStopButton;
        wheel1.Model.Stopping += DisableStopButton;
        wheel3.Model.Stopped += EnableStartButton;
    }

    private void EnableStartButton()
    {
        BtnPnl.playButton.transform.localScale = Vector3.one;
        BtnPnl.playButton.interactable = true;
    }

    private void DisableStartButton()
    {
        BtnPnl.playButton.interactable = false;
        BtnPnl.playButton.transform.localScale = Vector3.zero;
    }

    private void EnableStopButton()
    {
        BtnPnl.stopButton.interactable = true;
        BtnPnl.stopButton.transform.localScale = Vector3.one;
    }

    private void DisableStopButton()
    {
        BtnPnl.stopButton.interactable = false;
        BtnPnl.stopButton.transform.localScale = Vector3.zero;
    }

    private void OnDestroy()
    {
        BtnPnl.OnStartButtonClick -= StartEveryWheelSpinning;
        BtnPnl.OnStopButtonClick -= StopEveryWheelSpinning;
        
        wheel1.Model.Starting -= DisableStartButton;
        wheel3.Model.Started -= EnableStopButton;
        wheel1.Model.Stopping -= DisableStopButton;
        wheel3.Model.Stopped -= EnableStartButton;
    }

    private void StartEveryWheelSpinning()
    {
        _runningCoroutine = StartCoroutine(StartSpinning());
    }

    private void StopEveryWheelSpinning()
    {
        StartCoroutine(StopSpinning());
    }
    
    private IEnumerator StartSpinning()
    {
        wheel1.StartMove();
        yield return new WaitForSeconds(0.5f);
        wheel2.StartMove();
        yield return new WaitForSeconds(0.5f);
        wheel3.StartMove();

        yield return new WaitForSeconds(6.0f);

        StopEveryWheelSpinning();
    }

    private IEnumerator StopSpinning()
    {
        StopCoroutine(_runningCoroutine);
        
        wheel1.StopMove();
        yield return new WaitForSeconds(0.5f);
        wheel2.StopMove();
        yield return new WaitForSeconds(0.5f);
        wheel3.StopMove();
    }
}
