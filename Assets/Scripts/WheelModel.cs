using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DefaultNamespace
{
    public sealed class WheelModel
    {
        private const double StartingAcceleration = 5.0;
        
        private static readonly TimeSpan StartingTime = TimeSpan.FromSeconds(2);
        
        private static readonly TimeSpan RunningTime = TimeSpan.FromSeconds(6);

        private static readonly TimeSpan StoppingTime = TimeSpan.FromSeconds(3);
        
        private static readonly double Speed = StartingAcceleration * StartingTime.TotalSeconds;

        private readonly List<SymbolModel> _symbols = new();

        private readonly Stopwatch _time = new();

        private readonly NotRunningWheel _notRunningWheel;

        private readonly StartingWheel _startingWheel;

        private readonly RunningWheel _runningWheel;

        private readonly StoppingWheel _stoppingWheel;

        private IWheelState _state;

        private double _position = .0f;
        
        public WheelModel()
        {
            _notRunningWheel = new NotRunningWheel(this);
            _startingWheel = new StartingWheel(this);
            _runningWheel = new RunningWheel(this);
            _stoppingWheel = new StoppingWheel(this);
            _state = _notRunningWheel;
        }

        public SymbolModel AddSymbol()
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
            foreach (SymbolModel symbol in _symbols)
            {
                symbol.UpdatePosition(_position);
            }
        }
        
        private interface IWheelState
        {
            void Start() => throw new NotSupportedException(
                $"Start operation is not supported for {this.GetType()}.");
            
            void Stop() => throw new NotSupportedException(
                $"Stop operation is not supported for {this.GetType()}.");
            
            void Update();
        }
        
        private sealed class NotRunningWheel : IWheelState
        {
            private readonly WheelModel _model;

            public NotRunningWheel(WheelModel model) => _model = model;

            public void Start()
            {
                _model._position = .0f;
                _model._state = _model._startingWheel;
                _model._time.Restart();
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
                TimeSpan elapsed = _model._time.Elapsed;
                if (elapsed > StartingTime)
                {
                    _model._state = _model._runningWheel;
                    _model._state.Update();
                }
                else
                {
                    double time = elapsed.TotalSeconds;
                    _model._position = StartingAcceleration * time * time * 0.5;
                }
            }
        }

        private sealed class RunningWheel : IWheelState
        {
            private static readonly double InitialPosition;
            
            private readonly WheelModel _model;

            static RunningWheel()
            {
                double time = StartingTime.TotalSeconds;
                InitialPosition = StartingAcceleration * time * time * 0.5;
            }

            public RunningWheel(WheelModel model) => _model = model;

            public void Stop()
            {
                _model._stoppingWheel.CalculateStopping();
                _model._state = _model._stoppingWheel;
            }

            public void Update()
            {
                TimeSpan elapsed = _model._time.Elapsed - StartingTime;
                if (elapsed > RunningTime)
                {
                    _model._stoppingWheel.CalculateStopping();
                    _model._state = _model._stoppingWheel;
                    _model._state.Update();
                }
                else
                {
                    _model._position = InitialPosition + Speed * elapsed.TotalSeconds;
                }
            }
        }

        private sealed class StoppingWheel : IWheelState
        {
            private readonly WheelModel _model;

            private TimeSpan _eventTime;

            private double _initialPosition;

            private double? _showFinalNumbersPosition;

            private double _acceleration;

            public StoppingWheel(WheelModel model) => _model = model;

            public void CalculateStopping()
            {
                double stoppingTime = StoppingTime.TotalSeconds;
                double acceleration = -(Speed / StoppingTime.TotalSeconds);
                double expectedPosition =
                    _model._position +
                    Speed * stoppingTime +
                    acceleration * stoppingTime * stoppingTime * 0.5;

                double expectedPositionFixed = Math.Round(expectedPosition);
                _acceleration =
                    2 * (expectedPositionFixed - _model._position - Speed * stoppingTime) /
                    (stoppingTime * stoppingTime);

                _showFinalNumbersPosition = expectedPositionFixed - 3;

                _eventTime = _model._time.Elapsed;
                _initialPosition = _model._position;
            }

            public void Update()
            {
                TimeSpan stoppingDuration = _model._time.Elapsed - _eventTime;
                double time;
                if (stoppingDuration > StoppingTime)
                {
                    time = StoppingTime.TotalSeconds;
                    _model._state = _model._notRunningWheel;
                    _model._time.Stop();
                }
                else
                {
                    time = stoppingDuration.TotalSeconds;
                }

                double newPosition = _initialPosition + Speed * time + _acceleration * time * time * 0.5f;
                CheckFinalNumbersPosition(newPosition);
                
                _model._position = newPosition;
            }

            private void CheckFinalNumbersPosition(double newPosition)
            {
                if (_showFinalNumbersPosition.HasValue &&
                    newPosition >= _showFinalNumbersPosition)
                {
                    foreach (SymbolModel symbolModel in _model._symbols)
                    {
                        symbolModel.AllowShowingFinalNumbers();
                    }

                    _showFinalNumbersPosition = null;
                }
            }
        }
    }
}