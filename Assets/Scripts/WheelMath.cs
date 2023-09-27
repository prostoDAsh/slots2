using System;
using DefaultNamespace.Configs;

namespace DefaultNamespace
{
    public static class WheelMath //класс с расчетами, связанными с движением символа на колесе
    {
        private const double StartingAcceleration = 5.0; //начальное ускорение движения символа
        
        public static readonly TimeSpan StartingTime = TimeSpan.FromSeconds(2); // время разгона

        public static TimeSpan StoppingTime = TimeSpan.FromSeconds(1.5); // время остановки
        
        public static double Speed = StartingAcceleration * StartingTime.TotalSeconds; //скорость при равномерном движении в средней фазе, на основе начльного ускорения и времени

        private static readonly double StoppingAcceleration = -(Speed / StoppingTime.TotalSeconds); //ускорение остановки символа, на основе скорости(в средн. фазе) и времени остановки
        
        private static readonly double InitialPosition = GetStartingPath(StartingTime.TotalSeconds); //начальная позиция символа

        public static double GetStartingPath(double time) =>
            StartingAcceleration * time * time * 0.5;//возвращает начальную позицию в зависимости от времени (формула перемещения для равномерноускоренногг движения)
        
        public static double GetRunningPath(double time) =>
            InitialPosition + Speed * time;//возвращает путь во время дижения (используя начальную поз и скорость(в средн. фазе) для рассчета пути)

        public static double GetStoppingPath(
            double initialPosition,
            double stoppingAcceleration,
            double time) =>
            initialPosition + Speed * time + stoppingAcceleration * time * time * 0.5f; 
        //возвращает путь символа при остановке (исп. скорость(в средн. фазе), ускорение остановки и время для рассчета пути)

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