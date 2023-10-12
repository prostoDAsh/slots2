using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Button startSlot1Btn;

    [SerializeField] private Button startSlot2Btn;

    public event Action Slot1;
    public event Action Slot2;

    private void Start()
    {
        startSlot1Btn.onClick.AddListener(OnClickSlot1);
        startSlot2Btn.onClick.AddListener(OnClickSlot2);
    }

    private void OnClickSlot2()
    {
        Slot2?.Invoke();
    }

    private void OnClickSlot1()
    {
        Slot1?.Invoke();
    }
}
