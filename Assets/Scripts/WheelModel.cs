using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;

namespace DefaultNamespace
{
    public sealed class WheelModel //класс, от которого нельзя наследоваться, представляет модель вращающегося колеса с разными состояниями
    {
        private readonly List<SymbolModel> _symbols = new();
        
        private readonly NotRunningWheel _notRunningWheel; //колесо не крутиться, скорость = 0

        private readonly StartingWheel _startingWheel; //колесо разгоняется

        private readonly RunningWheel _runningWheel; //колесо крутиться, скорость постоянная

        private readonly StoppingWheel _stoppingWheel; //колесо останавливается
        
        private SpriteProvider _spriteProvider;

        private FinalScreenData _finalScreenData;

        private Symbol _symbol;

        private IWheelState _state;

        private double _position;

        public WheelModel() //конструктор класса, + устанавливает начальное состояние
        {
            _notRunningWheel = new NotRunningWheel(this);
            _startingWheel = new StartingWheel(this);
            _runningWheel = new RunningWheel(this);
            _stoppingWheel = new StoppingWheel(this);
            _state = _notRunningWheel;
        }
        private Stopwatch Stopwatch { get; } = new(); // таймер

        private double Position => _position; 

        private void ToStarting() //метод устанавливает состояние разгона, обновляет поз колеса, запускает таймер
        {
            Starting?.Invoke();
            UpdatePosition(.0);
            _state = _startingWheel;
            Stopwatch.Restart();  
        }

        private void ToRunning() //метод устанавливает состояние колеса - крутящееся и вызыввакт update текущего состояния
        {
            Started?.Invoke();
            _state = _runningWheel;
            _state.Update();
        }

        private void ToStopping() //метод устанавливает состояние змедления
        {
            Stopping?.Invoke();
            _state = _stoppingWheel;
        }

        private void ToNotRunning() //устанавливает состояние - колесо не крутиться, останавливает таймер
        {
            Stopped?.Invoke();
            _state = _notRunningWheel;
            Stopwatch.Stop();
        }

        public SymbolModel AddSymbol() //метод создает новую модель симовла, добавляет в список и возвращает созданный символ
        {
            var symbol = new SymbolModel(_symbols.Count);
            _symbols.Add(symbol);

            return symbol;
        }

        public void Start() 
        {
            _state.Start();
        }

        public void Stop()
        {
            _state.Stop();
        }
        
        public void Update()
        {
            _state.Update();
        }

        private void UpdatePosition(double newPosition)
        {
            _position = newPosition; //обновляет позицию колеса
            foreach (SymbolModel symbol in _symbols) //одновляет позицию всех символов
            {
                symbol.UpdatePosition(_position);
            }
        }

        private void UpdateFinalPosition(double expectedPosition) //обновляет фин позицию всех символов на колесе до нужной позиции
        {
            foreach (SymbolModel symbolModel in _symbols)
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
                $"Start operation is not supported for {this.GetType()}."); //вызывается при старте колеса
            
            void Stop() => throw new NotSupportedException(
                $"Stop operation is not supported for {this.GetType()}."); //вызывается при остановке колеса
            
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

            private TimeSpan _eventTime;// время начала остановки

            private double _initialPosition;

            private double _expectedPosition;

            private double _acceleration;

            private bool _isReset = true;
            
            public StoppingWheel(WheelModel model) => _model = model;

            private void CalculateStopping()
            {
                _eventTime = _model.Stopwatch.Elapsed;
                _initialPosition = _model.Position;
                
                (_expectedPosition, _acceleration) = WheelMath.GetStoppingAcceleration(_initialPosition);
                
                _model.UpdateFinalPosition(_expectedPosition);
            }

            public void Update()
            {
                if (_isReset)
                {
                    CalculateStopping();
                    _isReset = false;
                }
                
                TimeSpan stoppingDuration = _model.Stopwatch.Elapsed - _eventTime;
                double time;
                double newPosition;
                if (stoppingDuration > WheelMath.StoppingTime)
                {
                    _model.ToNotRunning();
                    newPosition = _expectedPosition;
                    _isReset = true;
                    
                }
                else
                {
                    time = stoppingDuration.TotalSeconds;
                    newPosition = WheelMath.GetStoppingPath(_initialPosition, _acceleration, time);

                    if (newPosition > _expectedPosition)
                    {
                        newPosition = _expectedPosition;
                    }
                }

                _model.UpdatePosition(newPosition);
            }
        }
    }
}