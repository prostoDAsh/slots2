using System;

namespace DefaultNamespace
{
    public sealed class SymbolModel
    {
        private const int WheelLength = 4;
        
        private readonly int _index;

        private double _position;

        private double? _finalSymbolPosition;

        public SymbolModel(int index)
        {
            _index = index;
            _position = _index;
        }

        public void UpdateFinalPosition(double finalPosition)
        {
            _finalSymbolPosition = finalPosition + _index - WheelLength + 1;
        }

        public void UpdatePosition(double position)
        {
            double newPosition = position + _index;

            if (IsLongChange(newPosition) || IsBelowScreen(newPosition))
            {
                if (TrySetFinalSymbol(newPosition, out int finalIndex))
                {
                    ShowFinalImage?.Invoke(finalIndex);
                }
                else
                {
                    ShowRandomImage?.Invoke();
                }
            }
            
            _position = newPosition;
            
            Moving?.Invoke(_position % WheelLength);
        }

        private bool IsLongChange(double newPosition) => 
            newPosition - _position > WheelLength;

        private bool IsBelowScreen(double newPosition) => 
            _position % WheelLength > newPosition % WheelLength;

        private bool TrySetFinalSymbol(double newPosition, out int finalIndex)
        {
            if (_finalSymbolPosition.HasValue &&
                newPosition >= _finalSymbolPosition.Value)
            {
                finalIndex = Math.Abs(_index -1);
                _finalSymbolPosition = null;
                
                return true;
            }

            finalIndex = 0;

            return false;
        }

        public event Action<double> Moving;

        public event Action ShowRandomImage;

        public event Action<int> ShowFinalImage;
    }
}