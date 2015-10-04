using Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects
{
    /// <summary>
    /// Компонент объекта для регистрации и хранения его в пуле игровых объектов.
    /// </summary>
    public class PoolableObject : AbstractObject, IPoolable
    {
        /// <summary>
        /// Событие активации объекта.
        /// </summary>
        public event PoolableEventHandler Activated;
        /// <summary>
        /// Событие деактивации объекта.
        /// </summary>
        public event PoolableEventHandler Deactivated;

        /// <summary>
        /// Деактивирует объект.
        /// </summary>
        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            if (Deactivated != null)
                Deactivated.Invoke(this);
        }
        /// <summary>
        /// Активирует объект.
        /// </summary>
        public virtual void Activate()
        {
            gameObject.SetActive(true);
            if (Activated != null)
                Activated.Invoke(this);
        }
        /// <summary>
        /// Сбрасывает все изменения объекта с его прошлой активации.
        /// </summary>
        public virtual void ResetChanges()
        {

        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }
    }
}
