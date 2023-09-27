using System;
using DefaultNamespace.Configs;

namespace DefaultNamespace
{
    public class WheelMath //класс с расчетами, связанными с движением символа на колесе
    {
        private static double startingAcceleration; //начальное ускорение движения символа
        
        public static TimeSpan StartingTime; // время разгона

        public static TimeSpan StoppingTime; // время остановки
        
        public static double Speed; //скорость при равномерном движении

        private static double stoppingAcceleration; //ускорение остановки символа
        
        private static double initialPosition; //начальная позиция символа

        private static readonly object syncRoot = new object();
        
        private static bool isInitialzied = false;
        
        public static void Initialize(NumbersConfig config)
        {
            if (!isInitialzied)
            {
                lock (syncRoot)
                {
                    if (!isInitialzied)
                    {
                        isInitialzied = true;
                    
                        startingAcceleration = config.StartingAcceleration;
                        
                        StartingTime = TimeSpan.FromSeconds(config.StartingTime);

                        StoppingTime = TimeSpan.FromSeconds(config.StoppingTime);
                        
                        Speed = startingAcceleration * StartingTime.TotalSeconds;
                        
                        stoppingAcceleration = -(Speed / StoppingTime.TotalSeconds);
                        
                        initialPosition = GetStartingPath(StartingTime.TotalSeconds);
                    }
                }
            }
        }


        public static double GetStartingPath(double time) =>
            startingAcceleration * time * time * 0.5;//возвращает начальную позицию в зависимости от времени (формула перемещения для равномерноускоренногг движения)
        
        public static double GetRunningPath(double time) =>
            initialPosition + Speed * time;//возвращает путь во время дижения (используя начальную поз и скорость(в средн. фазе) для рассчета пути)

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
            double expectedPosition = GetStoppingPath(initialPosition, stoppingAcceleration, stoppingTime);
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