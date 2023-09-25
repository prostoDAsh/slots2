using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(fileName = "New Numbers Config", menuName = "Numbers Config")]
    public class NumbersConfig : ScriptableObject
    {
        [Header("Slot Machine Controller")]
        
        [SerializeField] private float delayBetweenStartWheels = 0.5f;

        [SerializeField] private float delayBetweenStopWheels = 0.2f;

        [SerializeField] private float delayAfterStartToStopWheels = 6f;

        [SerializeField] private float delayAfterStopToStartScaleAnimation = 1.5f;

        [SerializeField] private float delayForAutoStartWithAnimation = 4f;

        [SerializeField] private float delayForAutoStartWithoutAnimation = 1f;

        [SerializeField] private float delayForStartPopupAnimation = 4f;

        [SerializeField] private float durationForScalePopup = 1.5f;
        
        [Header("Wheel Math")]

        [SerializeField] private double startingTime = 2;

        [SerializeField] private double stoppingTime = 1.5;

        [Header("Slot Wheels")] 
        
        [SerializeField] private float delayBetweenDarken = 3.5f;

        [FormerlySerializedAs("durationForScale")] [SerializeField] private float durationForScaleAnimation = 2f;

        [SerializeField] private float strengthForShake = 8f;

        [FormerlySerializedAs("endValueForScale")] [SerializeField] private float endValueForScaleAnimation = 1.3f;

        [SerializeField] private float endValueForAlpha = 0.4f;
        public float DelayBetweenStartWheels => delayBetweenStartWheels;

        public float DelayBetweenStopWheels => delayBetweenStopWheels;

        public float DelayAfterStartToStopWheels => delayAfterStartToStopWheels;

        public float DelayAfterStopToStartScaleAnimation => delayAfterStopToStartScaleAnimation;

        public float DelayForAutoStartWithAnimation => delayForAutoStartWithAnimation;

        public float DelayForAutoStartWithoutAnimation => delayForAutoStartWithoutAnimation;

        public float DelayForStartPopupAnimation => delayForStartPopupAnimation;

        public float DelayBetweenDarken => delayBetweenDarken;

        public double StoppingTime => stoppingTime;

        public double StartingTime => startingTime;

        public float DurationForScaleAnimation => durationForScaleAnimation;

        public float StrengthForShake => strengthForShake;

        public float EndValueForScaleAnimation => endValueForScaleAnimation;

        public float EndValueForAlpha => endValueForAlpha;

        public float DurationForScalePopup => durationForScalePopup;
    }
}