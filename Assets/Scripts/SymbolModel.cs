using System;

namespace DefaultNamespace
{
    public sealed class SymbolModel
    {
        private readonly int _index;

        public SymbolModel(int index)
        {
            _index = index;
            Position += _index;
        }
        
        public double Position { get; private set; }

        public void AllowShowingFinalNumbers()
        {
            ShowFinal?.Invoke();
        }

        public void UpdatePosition(double position)
        {
            double newPosition = (position + _index) % 4.0;
            if (newPosition < Position)
            {
                UpdateImage?.Invoke();
            }

            Position = newPosition;
        }

        public event Action UpdateImage;

        public event Action ShowFinal;
    }
}