using System;

namespace DefaultNamespace
{
    public static class WheelMath
    {
        private const double StartingAcceleration = 5.0;
        
        public static readonly TimeSpan StartingTime = TimeSpan.FromSeconds(2);

        public static readonly TimeSpan StoppingTime = TimeSpan.FromSeconds(3);
        
        private static readonly double Speed = StartingAcceleration * StartingTime.TotalSeconds;

        private static readonly double StoppingAcceleration = -(Speed / StoppingTime.TotalSeconds);
        
        private static readonly double InitialPosition = GetStartingPath(StartingTime.TotalSeconds);

        public static double GetStartingPath(double time) =>
            StartingAcceleration * time * time * 0.5;
        
        public static double GetRunningPath(double time) =>
            InitialPosition + Speed * time;

        public static double GetStoppingPath(
            double initialPosition,
            double stoppingAcceleration,
            double time) =>
            initialPosition + Speed * time + stoppingAcceleration * time * time * 0.5f;

        public static (double ExpectedPosition, double Acceleration) GetStoppingAcceleration(
            double initialPosition)
        {
            double stoppingTime = StoppingTime.TotalSeconds;
            double expectedPosition = GetStoppingPath(initialPosition, StoppingAcceleration, stoppingTime);
            double expectedPositionFixed = Math.Ceiling(expectedPosition / 4) * 4;
            double finalAcceleration =
                2 * (expectedPositionFixed - initialPosition - Speed * stoppingTime) /
                (stoppingTime * stoppingTime);

            return (expectedPositionFixed, finalAcceleration);
        }
    }
}