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

    private void Start()
    {
        BtnPnl.stopButton.interactable = false;
        BtnPnl.stopButton.transform.localScale = Vector3.zero;
        
        BtnPnl.OnStartButtonClick += StartEveryWheelSpinning;
        BtnPnl.OnStopButtonClick += StopEveryWheelSpinning;
    }

    private void OnDestroy()
    {
        BtnPnl.OnStartButtonClick -= StartEveryWheelSpinning;
        BtnPnl.OnStopButtonClick -= StopEveryWheelSpinning;
    }

    private void StartEveryWheelSpinning()
    {
        StartCoroutine(StartSpinning());
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
        
        BtnPnl.playButton.interactable = false;
        BtnPnl.playButton.transform.localScale = Vector3.zero;

        BtnPnl.stopButton.interactable = true;
        BtnPnl.stopButton.transform.localScale = Vector3.one;
        yield break;
    }


    private IEnumerator StopSpinning()
    {
        wheel1.StopMove();
        yield return new WaitForSeconds(0.5f);
        wheel2.StopMove();
        yield return new WaitForSeconds(0.5f);
        wheel3.StopMove();

        BtnPnl.stopButton.interactable = false;
        BtnPnl.stopButton.transform.localScale = Vector3.zero;
               
        BtnPnl.playButton.transform.localScale = Vector3.one;
        BtnPnl.playButton.interactable = true;
        
        yield break;
    }
    
}
