using System;

namespace DefaultNamespace
{
    public static class WheelMath //класс с расчетами, связанными с движением символа на колесе
    {
        private const double StartingAcceleration = 5.0; //начальное ускорение движения символа
        
        public static readonly TimeSpan StartingTime = TimeSpan.FromSeconds(2); // время разгона

        public static readonly TimeSpan StoppingTime = TimeSpan.FromSeconds(3); // время остановки
        
        private static readonly double Speed = StartingAcceleration * StartingTime.TotalSeconds; //начльная скорость, на основе начльного ускорения и времени

        private static readonly double StoppingAcceleration = -(Speed / StoppingTime.TotalSeconds); //ускорение остановки символа, на основе начальной скорости и времени остановки
        
        private static readonly double InitialPosition = GetStartingPath(StartingTime.TotalSeconds); //начальная позиция символа

        public static double GetStartingPath(double time) =>
            StartingAcceleration * time * time * 0.5;//возвращает начальную позицию в зависимости от времени (формула перемещения для равномерноускоренногг движения)
        
        public static double GetRunningPath(double time) =>
            InitialPosition + Speed * time;//возвращает путь во время дижения (используя начальную поз и начальную скорость для рассчета пути)

        public static double GetStoppingPath(
            double initialPosition,
            double stoppingAcceleration,
            double time) =>
            initialPosition + Speed * time + stoppingAcceleration * time * time * 0.5f; 
        //возвращает путь символа при остановке (исп. нач. скорость, ускорение остановки и время для рассчета пути)

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
            //возвращает ожидаемую поз.  и ускорение остановки (исп. начальную поз. для определния ожидаемой позиции
            //после остановки и затем рассчитывает ускорение остановки)
        }
    }
}