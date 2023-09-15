using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(fileName = "New Numbers Config", menuName = "Numbers Config")]
    public class NumbersConfig : ScriptableObject
    {
        [SerializeField] private float delayBetweenStartWheels = 0.5f;

        [SerializeField] private float delayBetweenStopWheels = 0.2f;

        [SerializeField] private float delayAfterStartToStopWheels = 6f;

        [SerializeField] private float delayAfterStopToStartScaleAnimation = 1.5f;

        [SerializeField] private float delayForAutoStartWithAnimation = 4f;

        [SerializeField] private float delayForAutoStartWithoutAnimation = 1f;

        [SerializeField] private float delayForStartPopupAnimation = 4f;
        public float DelayBetweenStartWheels => delayBetweenStartWheels;

        public float DelayBetweenStopWheels => delayBetweenStopWheels;

        public float DelayAfterStartToStopWheels => delayAfterStartToStopWheels;

        public float DelayAfterStopToStartScaleAnimation => delayAfterStopToStartScaleAnimation;

        public float DelayForAutoStartWithAnimation => delayForAutoStartWithAnimation;

        public float DelayForAutoStartWithoutAnimation => delayForAutoStartWithoutAnimation;

        public float DelayForStartPopupAnimation => delayForStartPopupAnimation;
    }
}