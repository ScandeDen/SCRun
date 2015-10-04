using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects
{
    /// <summary>
    /// Компонент, описывающий сетку объекта для скриптов которые их используют. Такие как 
    /// генераторы игрового пространства.
    /// </summary>
    public class CellableObject : AbstractObject
    {
        [SerializeField]
        protected int leftEmptyCellsCount = 0;
        /// <summary>
        /// Количество клеток слева от клетки, в которой находится Transform объекта.
        /// </summary>
        public int LeftEmptyCellsCount
        {
            get
            {
                return leftEmptyCellsCount;
            }
        }
        
        [SerializeField]
        protected int rightEmptyCellsCount = 0;
        /// <summary>
        /// Количество клеток справа от клетки, в которой находится Transform объекта.
        /// </summary>
        public int RightEmptyCellsCount
        {
            get
            {
                return rightEmptyCellsCount;
            }
        }
        /// <summary>
        /// Длина объекта в клетках.
        /// </summary>
        public int WidthInCells
        {
            get
            {
                return LeftEmptyCellsCount + 1 + RightEmptyCellsCount;
            }
        }
    }
}
