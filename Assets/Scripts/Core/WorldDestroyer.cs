using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.Objects;
using Defaults;

namespace Core
{
    /// <summary>
    /// Компонент деактивирует объекты, попавшие в его зону.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class WorldDestroyer : AbstractBehavior
    {
        void OnTriggerEnter(Collider other)
        {
            IPoolable poolable = other.GetComponent<PoolableObject>();
            if (poolable != null)
            {
                poolable.Deactivate();
            }
        }

        protected override void OnStart() { }

        protected override void OnAwake() { }
    }
}
