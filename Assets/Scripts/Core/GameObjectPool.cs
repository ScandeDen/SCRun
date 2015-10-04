using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.Objects;
using Defaults;

namespace Core.Pools
{
    /// <summary>
    /// Реализация абстрактного пула для игровых объектов с компонентом PoolableObject.
    /// </summary>
    public class GameObjectPool : AbstractPool<PoolableObject>
    {
        /// <summary>
        /// Префаб, который будет использоваться для создания игровых объектов.
        /// </summary>
        public PoolableObject Origin;

        /// <summary>
        /// Возвращает новый, сгенерированный эти пулом объект.
        /// </summary>
        /// <returns> Объект. </returns>
        protected override PoolableObject GenerateNew()
        {
            var pooled = Instantiate<PoolableObject>(Origin);
            pooled.transform.SetParent(GetCashedComponent<Transform>());
            return pooled;
        }
        /// <summary>
        /// Обрабатывает деактивацию объекта, помещая его в пул.
        /// </summary>
        /// <param name="item"> Объект. </param>
        protected override void ItemDeactivated(IPoolable item)
        {
            if (!(item is PoolableObject))
                return;
            activated.Remove(item);
            pool.Add(item);
            (item as PoolableObject).transform.SetParent(GetCashedComponent<Transform>());
        }

        protected override void OnAwake()
        {
            CashComponent<Transform>();
        }
    }
}
