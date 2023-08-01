using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ButtonsPanel : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        public event Action OnStartButtonClick;
        private void Start()
        {
            startButton.onClick.AddListener(StartButtonClick);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(StartButtonClick);
        }

        private void StartButtonClick()
        {
            OnStartButtonClick?.Invoke();
        }
    }
}