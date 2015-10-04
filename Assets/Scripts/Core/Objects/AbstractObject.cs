using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects
{
    /// <summary>
    /// Абстрактный объект игрового мира.
    /// </summary>
    public abstract class AbstractObject : Defaults.AbstractBehavior
    {
        protected override void OnAwake()
        {
            CashComponent<Transform>();
        }

        protected override void OnStart() { }
    }
}
