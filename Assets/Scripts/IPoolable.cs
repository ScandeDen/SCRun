using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defaults
{
    /// <summary>
    /// Интерфейс объекта, который может использовать любой пул.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// Событие активации объекта.
        /// </summary>
        event PoolableEventHandler Activated;
        /// <summary>
        /// Событие деактивации объекта.
        /// </summary>
        event PoolableEventHandler Deactivated;
        /// <summary>
        /// Деактивирует объект.
        /// </summary>
        void Deactivate();
        /// <summary>
        /// Активирует объект.
        /// </summary>
        void Activate();
        /// <summary>
        /// Сбрасывает все изменения объекта с его прошлой активации.
        /// </summary>
        void ResetChanges();
    }

    public delegate void PoolableEventHandler(IPoolable sender);
}
