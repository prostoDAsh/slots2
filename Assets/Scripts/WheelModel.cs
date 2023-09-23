using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;

namespace DefaultNamespace
{
    public sealed class WheelModel //класс, от которого нельзя наследоваться, представляет модель вращающегося колеса с разными состояниями
    {
        private readonly List<SymbolModel> symbols = new();
        
        private readonly NotRunningWheel notRunningWheel; //колесо не крутиться, скорость = 0

        private readonly StartingWheel startingWheel; //колесо разгоняется

        private readonly RunningWheel runningWheel; //колесо крутиться, скорость постоянная

        private readonly StoppingWheel stoppingWheel; //колесо останавливается
        
        private SpriteProvider spriteProvider;

        private FinalScreenData finalScreenData;

        private Symbol symbol;

        private IWheelState state;

        private double position;

        public WheelModel() //конструктор класса, + устанавливает начальное состояние
        {
            notRunningWheel = new NotRunningWheel(this);
            startingWheel = new StartingWheel(this);
            runningWheel = new RunningWheel(this);
            stoppingWheel = new StoppingWheel(this);
            state = notRunningWheel;
        }
        private Stopwatch Stopwatch { get; } = new(); // таймер

        private double Position => position; 

        private void ToStarting() //метод устанавливает состояние разгона, обновляет поз колеса, запускает таймер
        {
            Starting?.Invoke();
            UpdatePosition(.0);
            state = startingWheel;
            Stopwatch.Restart();  
        }

        private void ToRunning() //метод устанавливает состояние колеса - крутящееся и вызыввакт update текущего состояния
        {
            Started?.Invoke();
            state = runningWheel;
            state.Update();
        }

        private void ToStopping() //метод устанавливает состояние змедления
        {
            Stopping?.Invoke();
            state = stoppingWheel;
        }

        private void ToNotRunning() //устанавливает состояние - колесо не крутиться, останавливает таймер
        {
            Stopped?.Invoke();
            state = notRunningWheel;
            Stopwatch.Stop();
        }

        public SymbolModel AddSymbol() //метод создает новую модель симовла, добавляет в список и возвращает созданный символ
        {
            var symbol = new SymbolModel(symbols.Count);
            symbols.Add(symbol);

            return symbol;
        }

        public void Start() 
        {
            state.Start();
        }

        public void Stop()
        {
            state.Stop();
        }
        
        public void Update()
        {
            state.Update();
        }

        private void UpdatePosition(double newPosition)
        {
            position = newPosition; //обновляет позицию колеса
            foreach (SymbolModel symbol in symbols) //одновляет позицию всех символов
            {
                symbol.UpdatePosition(position);
            }
        }

        private void UpdateFinalPosition(double expectedPosition) //обновляет фин позицию всех символов на колесе до нужной позиции
        {
            foreach (SymbolModel symbolModel in symbols)
            {
                symbolModel.UpdateFinalPosition(expectedPosition);
            }
        }

        public event Action Starting; //в момент клика кнопки старт ,  скорость 0 

        public event Action Started; //разгон окончен, скорость равномрная

        public event Action Stopping; //момент когда колесо только начинает замедление

        public event Action Stopped; // момент когда колесо остановилось скорость 0
        
        
        
        private interface IWheelState //интерфейс определяет методы, которые должны быть реализованы в каждом состоянии колеса
        {
            void Start() => throw new NotSupportedException(
                $"Start operation is not supported for {this.GetType()}."); 
            
            void Stop() => throw new NotSupportedException(
                $"Stop operation is not supported for {this.GetType()}."); 
            
            void Update();//метод, вызываемый для обновления состояния колес
        }
        
        private sealed class NotRunningWheel : IWheelState
        {
            private readonly WheelModel _model;
            
            public NotRunningWheel(WheelModel model) => _model = model;

            public void Start()
            {
                _model.ToStarting();
            }

            public void Update()
            {
            }
        }

        private sealed class StartingWheel : IWheelState
        {
            private readonly WheelModel _model;

            public StartingWheel(WheelModel model) => _model = model;

            public void Update()
            {
                TimeSpan elapsed = _model.Stopwatch.Elapsed; // время после старта
                if (elapsed > WheelMath.StartingTime)
                {
                    _model.ToRunning();
                }
                else
                {
                    double newPosition = WheelMath.GetStartingPath(elapsed.TotalSeconds);
                    _model.UpdatePosition(newPosition);
                }
            }
        }

        private sealed class RunningWheel : IWheelState
        {
            private readonly WheelModel _model;

            public RunningWheel(WheelModel model) => _model = model;

            public void Stop()
            {
                _model.ToStopping();
            }

            public void Update()
            {
                TimeSpan elapsed = _model.Stopwatch.Elapsed - WheelMath.StartingTime;
                double newPosition = WheelMath.GetRunningPath(elapsed.TotalSeconds);
                _model.UpdatePosition(newPosition);
            }
        }

        private sealed class StoppingWheel : IWheelState
        {
            private readonly WheelModel _model;

            private TimeSpan eventTime;// время начала остановки

            private double initialPosition;

            private double expectedPosition;

            private double acceleration;

            private bool isReset = true;
            
            public StoppingWheel(WheelModel model) => _model = model;

            private void CalculateStopping()
            {
                eventTime = _model.Stopwatch.Elapsed;
                initialPosition = _model.Position;
                
                (expectedPosition, acceleration) = WheelMath.GetStoppingAcceleration(initialPosition);
                
                _model.UpdateFinalPosition(expectedPosition);
            }

            public void Update()
            {
                if (isReset)
                {
                    CalculateStopping();
                    isReset = false;
                }
                
                TimeSpan stoppingDuration = _model.Stopwatch.Elapsed - eventTime;
                double time;
                double newPosition;
                if (stoppingDuration > WheelMath.StoppingTime)
                {
                    _model.ToNotRunning();
                    newPosition = expectedPosition;
                    isReset = true;
                    
                }
                else
                {
                    time = stoppingDuration.TotalSeconds;
                    newPosition = WheelMath.GetStoppingPath(initialPosition, acceleration, time);

                    if (newPosition > expectedPosition)
                    {
                        newPosition = expectedPosition;
                    }
                }

                _model.UpdatePosition(newPosition);
            }
        }
    }
}