using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ButtonsPanel : MonoBehaviour
    {
        [SerializeField] public Button playButton;
        [SerializeField] public Button stopButton;

        public event Action OnStartButtonClick;
        public event Action OnStopButtonClick;
        
        private void Start()
        {
            playButton.onClick.AddListener(StartButtonClick);
            stopButton.onClick.AddListener(StopButtonClick);
            
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(StartButtonClick);
            stopButton.onClick.RemoveListener(StopButtonClick);
        }

        private void StartButtonClick()
        {
            OnStartButtonClick?.Invoke();
        }

        private void StopButtonClick()
        {
            OnStopButtonClick?.Invoke();
        }
    }
}