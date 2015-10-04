using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects.Zones
{
    /// <summary>
    /// Абстрактная зона с обратным эффектом. Эффект накладывается на заданный объект, 
    /// если в зоне появляется объект отмеченный одним из тегов.
    /// </summary>
    /// <typeparam name="TEffect"> Тип эффекта. </typeparam>
    public class AbstractReversedZone<TEffect> : AbstractZone<TEffect>
        where TEffect : AbstractState
    {
        /// <summary>
        /// Целевой объект, на который будет накладываться эффект.
        /// </summary>
        public StatableObject TargetForInfluence;

        protected override void OnTriggerEnter(Collider other)
        {
            if (TargetForInfluence == null)
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
            IncludeInfluence(TargetForInfluence);
        }

        protected override void OnTriggerExit(Collider other)
        {
            LeaveZone(other.GetComponent<Transform>());
        }
        /// <summary>
        /// Процедура покидания зоны.
        /// </summary>
        /// <param name="t"> Объект, покидающий зону. </param>
        protected override void LeaveZone(Transform t)
        {
            if (TargetForInfluence == null)
                return;
            if (t == null)
                return;
            var p = t.GetComponent<PoolableObject>();
            if (p != null)
                p.Deactivated -= OtherDeactivated;
            DisableInfluence(TargetForInfluence);
        }
    }
}
