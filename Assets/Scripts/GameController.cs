using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum PanelsType
    {
        None,
        Slot1,
        Slot2,
        Menu
    }

    [SerializeField] private Slot2MachineController slot2;
    
    [SerializeField] private Slot1MachineController slot1;

    [SerializeField] private MenuPanel menuPanel;

    private void Start()
    {
        slot2.ReturnMenu += OnReturnMenu;
        slot1.ReturnMenu += OnReturnMenu;
        menuPanel.Slot1 += StartSlot1;
        menuPanel.Slot2 += StartSlot2;
    }

    private void OnReturnMenu()
    {
        SwitchPanel(PanelsType.Menu);
    }

    private void SwitchPanel(PanelsType panelType)
    {
        menuPanel.gameObject.SetActive(panelType == PanelsType.Menu);
        slot1.gameObject.SetActive(panelType == PanelsType.Slot1);
        slot2.gameObject.SetActive(panelType == PanelsType.Slot2);
    }

    private void StartSlot1()
    {
       SwitchPanel(PanelsType.Slot1); 
    }

    private void StartSlot2()
    {
        SwitchPanel(PanelsType.Slot2);
    }

    private void OnDestroy()
    {
        slot2.ReturnMenu -= OnReturnMenu;
        slot1.ReturnMenu -= OnReturnMenu;
        menuPanel.Slot1 -= StartSlot1;
        menuPanel.Slot2 -= StartSlot2;
    }
}