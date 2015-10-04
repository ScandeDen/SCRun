using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects
{
    /// <summary>
    /// Абстрактная зона, которая имеет возможность накладывать состояния на компонент всех 
    /// объектов, которые попали в нее и отмечены соответсвующим тегом.
    /// </summary>
    /// <typeparam name="TEffect"> Тип эффекта. </typeparam>
    [RequireComponent(typeof(Collider))]
    public abstract class AbstractZone<TEffect> : AbstractObject
        where TEffect : AbstractState
    {
        /// <summary>
        /// Эффект зоны, накладываемое состояние.
        /// </summary>
        [Header("Zone")]
        public TEffect Effect;
        /// <summary>
        /// Список тэгов объектов, на которых будет накладываться состояние.
        /// </summary>
        public List<string> TagsForTakingInfluence = new List<string>();
        /// <summary>
        /// Список объектов под влиянием зоны.
        /// </summary>
        protected List<Transform> influenced = new List<Transform>();

        protected override void OnAwake()
        {
            base.OnAwake();
            CashComponent<PoolableObject>();
            var p = GetCashedComponent<PoolableObject>();
            if (p != null)
                p.Deactivated += ThisDeactivated;
        }
        /// <summary>
        /// Обрабатывает деактивацию компонента абстрактной зоны.
        /// </summary>
        /// <param name="sender"> Если зона была IPoolable, то это ссылка на нее. </param>
        protected virtual void ThisDeactivated(Defaults.IPoolable sender)
        {
            OnDestroy();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var otherS = other.GetComponent<StatableObject>();
            if (otherS == null)
                return;
            if (!TagsForTakingInfluence.Contains(other.tag))
                return;
            var otherT = other.GetComponent<Transform>();
            if (!influenced.Contains(otherT))
            {
                var otherP = other.GetComponent<PoolableObject>();
                if (otherP != null)
                    otherP.Deactivated += OtherDeactivated;
                influenced.Add(otherT);
            }
            IncludeInfluence(otherS);
        }
        /// <summary>
        /// Обрабатывает деактивацию объекта, находящегося под влиянием зоны.
        /// </summary>
        /// <param name="sender"> Если объект был IPoolable, то это ссылка на него. </param>
        protected virtual void OtherDeactivated(Defaults.IPoolable sender)
        {
            if (!(sender is PoolableObject))
                return;
            sender.Deactivated -= OtherDeactivated;
            var t = (sender as PoolableObject).GetComponent<Transform>();
            influenced.Remove(t);
            LeaveZone(t);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<StatableObject>() == null)
                return;
            LeaveZone(other.GetComponent<Transform>());
        }
        /// <summary>
        /// Процедура покидания зоны.
        /// </summary>
        /// <param name="t"> Объект, покидающий зону. </param>
        protected virtual void LeaveZone(Transform t)
        {
            if (t == null)
                return;
            var p = t.GetComponent<PoolableObject>();
            if (p != null)
                p.Deactivated -= OtherDeactivated;
            var statable = t.GetComponent<StatableObject>();
            if (statable != null)
                DisableInfluence(statable);
        }

        protected virtual void OnDestroy()
        {
            foreach (var t in influenced)
            {
                if (t == null)
                    continue;
                var p = t.GetComponent<PoolableObject>();
                if (p != null)
                    p.Deactivated -= OtherDeactivated;
            }
        }
        /// <summary>
        /// Включение влияния зоны на объект.
        /// </summary>
        /// <param name="target"> Компонент состояний объекта. </param>
        protected virtual void IncludeInfluence(StatableObject target)
        {
            target.AddState(Effect);
        }
        /// <summary>
        /// Отключение влияния зоны на объект.
        /// </summary>
        /// <param name="target"> Компонент состояний объекта. </param>
        protected virtual void DisableInfluence(StatableObject target)
        {
            target.RemoveState(Effect);
        }
    }
}
