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
    
    [FormerlySerializedAs("BtnPnl")] [SerializeField] private ButtonsPanel btnPnl;

    private Coroutine _runningCoroutine;

    private void Start()
    {
        btnPnl.stopButton.interactable = false;
        btnPnl.stopButton.transform.localScale = Vector3.zero;
        
        btnPnl.OnStartButtonClick += StartEveryWheelSpinning;
        btnPnl.OnStopButtonClick += StopEveryWheelSpinning;
        
        wheel1.Model.Starting += DisableStartButton;
        wheel3.Model.Started += EnableStopButton;
        wheel1.Model.Stopping += DisableStopButton;
        wheel3.Model.Stopped += EnableStartButton;
    }

    private void EnableStartButton()
    {
        btnPnl.playButton.transform.localScale = Vector3.one;
        btnPnl.playButton.interactable = true;
    }

    private void DisableStartButton()
    {
        btnPnl.playButton.interactable = false;
        btnPnl.playButton.transform.localScale = Vector3.zero;
    }

    private void EnableStopButton()
    {
        btnPnl.stopButton.interactable = true;
        btnPnl.stopButton.transform.localScale = Vector3.one;
    }

    private void DisableStopButton()
    {
        btnPnl.stopButton.interactable = false;
        btnPnl.stopButton.transform.localScale = Vector3.zero;
    }

    private void OnDestroy()
    {
        btnPnl.OnStartButtonClick -= StartEveryWheelSpinning;
        btnPnl.OnStopButtonClick -= StopEveryWheelSpinning;
        
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
