using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DefaultNamespace
{
    public sealed class WheelModel
    {
        private readonly List<SymbolModel> _symbols = new();
        
        private readonly NotRunningWheel _notRunningWheel;

        private readonly StartingWheel _startingWheel;

        private readonly RunningWheel _runningWheel;

        private readonly StoppingWheel _stoppingWheel;

        private IWheelState _state;

        private double _position;

        public WheelModel()
        {
            _notRunningWheel = new NotRunningWheel(this);
            _startingWheel = new StartingWheel(this);
            _runningWheel = new RunningWheel(this);
            _stoppingWheel = new StoppingWheel(this);
            _state = _notRunningWheel;
        }

        private Stopwatch Stopwatch { get; } = new(); 

        private double Position => _position;

        private void ToStarting()
        {
            Starting?.Invoke();
            UpdatePosition(.0f);
            _state = _startingWheel;
            Stopwatch.Restart();
        }

        private void ToRunning()
        {
            Started?.Invoke();
            _state = _runningWheel;
            _state.Update();
        }

        private void ToStopping()
        {
            Stopping?.Invoke();
            _state = _stoppingWheel;
        }

        private void ToNotRunning()
        {
            Stopped?.Invoke();
            _state = _notRunningWheel;
            Stopwatch.Stop();
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
        }

        private void UpdatePosition(double newPosition)
        {
            _position = newPosition;
            foreach (SymbolModel symbol in _symbols)
            {
                symbol.UpdatePosition(_position);
            }
        }

        private void UpdateFinalPosition(double expectedPosition)
        {
            foreach (SymbolModel symbolModel in _symbols)
            {
                symbolModel.UpdateFinalPosition(expectedPosition);
            }
        }

        public event Action Starting;

        public event Action Started;

        public event Action Stopping;

        public event Action Stopped;
        
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
                TimeSpan elapsed = _model.Stopwatch.Elapsed;
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

            private TimeSpan _eventTime;

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