using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects.Properties
{
    /// <summary>
    /// Компонент свойства, которое содержит целочисленное значение.
    /// </summary>
    public class Property : AbstractObject
    {
        [Header("Property")]
        [SerializeField]
        protected int value;
        /// <summary>
        /// Значение свойства.
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                var temp = this.value;
                ValueChange(this.value, value);
                if (ValueChanged != null)
                    ValueChanged.Invoke(this, temp);
            }
        }
        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event ValueChangedHandler ValueChanged;

        protected override void OnAwake()
        {
            base.OnAwake();
        }
        /// <summary>
        /// Процедура изменения значения свойства.
        /// </summary>
        /// <param name="from"> Прошлое значение. </param>
        /// <param name="to"> Новое значение. </param>
        protected virtual void ValueChange(int from, int to) 
        {
            value = to;
        }
        
    }

    public delegate void ValueChangedHandler(Property sender, int oldVal);
}
