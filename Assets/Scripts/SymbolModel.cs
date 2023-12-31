using System;
using DG.Tweening;

namespace DefaultNamespace
{
    public sealed class SymbolModel //общий функционал заключается в управлении позицией символа и определения, какое изображение показать
    {
        private const int WheelLength = 4;
        
        private readonly int index; //индекс символа, используется для определения позиции симовла на колесе и для вычислений в методах

        private double position; //текущая поз символа на колесе

        private double? finalSymbolPosition; //поз окончателного символа, ? означает что может быть равна null

        public SymbolModel(int index) //конструктор класса, инициализирует _index и _position начальным значением _index
        {
            this.index = index;
            position = this.index;
        }

        public void UpdateFinalPosition(double finalPosition)
        {
            finalSymbolPosition = finalPosition + index - WheelLength + 1;
        }

        public void UpdatePosition(double position)
        {
            double newPosition = position + index;

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

            this.position = newPosition;
            
            Moving?.Invoke(this.position % WheelLength);
        }

        private bool IsLongChange(double newPosition) => 
            newPosition - position > WheelLength; //возвращает тру, если разница между новой и текущ позициями больше длины колеса

        private bool IsBelowScreen(double newPosition) => 
            position % WheelLength > newPosition % WheelLength; //определяет, находится ли новая позиция ниже текущей позиции на колесе

        private bool TrySetFinalSymbol(double newPosition, out int finalIndex)
        {
            if (finalSymbolPosition.HasValue &&
                newPosition >= finalSymbolPosition.Value)
            {
                finalIndex = Math.Abs(index -1);
                finalSymbolPosition = null;
                
                return true;
            }

            finalIndex = 0;

            return false;
            //метож пытется устсновить окончательный симол, если новая поз достигла позиции окончательного символа, 
            // если все гуд, возвращает тру и указывает индекс окончательного символа (out int finalIndex)
        }

        public event Action<double> Moving;

        public event Action ShowRandomImage;

        public event Action<int> ShowFinalImage;
    }
}