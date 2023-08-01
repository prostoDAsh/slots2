using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class SlotMachineController : MonoBehaviour
{
    public Transform[] wheels;
    [SerializeField] private SlotWheel wheel1;
    [SerializeField] private SlotWheel wheel2;
    [SerializeField] private SlotWheel wheel3;

    private void Start()
    {
      //  wheel1.OnPlayBtnClick();
    }
}
