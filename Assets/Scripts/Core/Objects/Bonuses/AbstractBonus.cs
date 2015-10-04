using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects.Bonuses
{
    /// <summary>
    /// Абстрактный бонус, который могут получить объекты, отмеченные тегами.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class AbstractBonus : AbstractObject
    {
        /// <summary>
        /// Тэги объектов которые могут получить бонус.
        /// </summary>
        [Header("Bonus")]
        public List<string> TagsForTaking = new List<string>();

        protected override void OnAwake()
        {
            base.OnAwake();
            CashComponent<PoolableObject>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!TagsForTaking.Contains(other.tag))
                return;
            BonusApply(other);
            var p = GetCashedComponent<PoolableObject>();
            if (p != null)
                p.Deactivate();
        }
        /// <summary>
        /// Применение бонуса на объект, имеющег заданный коллаидер.
        /// </summary>
        /// <param name="target"> Коллаидер объекта. </param>
        protected abstract void BonusApply(Collider target);
    }
}
