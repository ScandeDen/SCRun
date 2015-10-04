using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Defaults
{
    /// <summary>
    /// Абстрактный пул объектов заданного типа, реализующего соответствующий интерфейс.
    /// </summary>
    /// <typeparam name="TItem"> Тип объектов. </typeparam>
    public abstract class AbstractPool<TItem> : AbstractBehavior
        where TItem : class, IPoolable
    {
        /// <summary>
        /// Размер пула при инициализации.
        /// </summary>
        public int InitPoolSize = 0;
        /// <summary>
        /// Если true, то пул расширяемый и будет увеличивать количество хранимых объектов
        /// путем генерирования при помощи соответствующей функции.
        /// </summary>
        public bool IsExtendable = true;
        /// <summary>
        /// Объекты, которые деактивированны и находятся в пуле.
        /// </summary>
        protected List<IPoolable> pool = new List<IPoolable>();
        /// <summary>
        /// Активированные объекты.
        /// </summary>
        protected List<IPoolable> activated = new List<IPoolable>();

        protected override void OnAwake()
        {
            Init();
        }

        protected override void OnStart() { }

        /// <summary>
        /// Инициализирует пул заданным количеством объектов.
        /// </summary>
        protected virtual void Init()
        {
            for (int i = 0; i < InitPoolSize; i++)
            {
                var item = GetNewItem();
                item.Deactivate();
            }
        }
        /// <summary>
        /// Обрабатывает деактивацию объекта, помещая его в пул.
        /// </summary>
        /// <param name="item"> Объект. </param>
        protected virtual void ItemDeactivated(IPoolable item)
        {
            if (!(item is TItem))
                return;
            activated.Remove(item);
            pool.Add(item);
        }
        /// <summary>
        /// Обрабатывает активацию объекта, перемещая егоиз пула в список активированных.
        /// </summary>
        /// <param name="item"> Объект. </param>
        protected virtual void ItemActivated(IPoolable item)
        {
            if (!(item is TItem))
                return;
            pool.Remove(item);
            activated.Add(item);
        }
        /// <summary>
        /// Возвращает свободный на данный момент объект в пуле. Если пул расширяем и 
        /// деактивированных не осталось - генерируется новый.
        /// </summary>
        /// <returns> Свободный объект. </returns>
        public virtual TItem GetFreeItem()
        {
            TItem item = null;
            if (pool.Count == 0)
            {
                if (IsExtendable)
                {
                    item = GetNewItem();
                    item.Deactivate();
                }
                else
                {
                    return null;
                }
            }
            item = (TItem)pool[0];
            return item;
        }
        /// <summary>
        /// Возвращает новый, сгенерированный и настроенный объект в пуле.
        /// </summary>
        /// <returns> Объект. </returns>
        protected TItem GetNewItem()
        {
            var item = GenerateNew();
            item.Activated += ItemActivated;
            item.Deactivated += ItemDeactivated;
            return item;
        }
        /// <summary>
        /// Возвращает новый, сгенерированный эти пулом объект.
        /// </summary>
        /// <returns> Объект. </returns>
        protected abstract TItem GenerateNew();

        void OnDestroy()
        {
            foreach (var p in pool)
            {
                p.Activated -= ItemActivated;
                p.Deactivated -= ItemDeactivated;
            }
            foreach (var p in activated)
            {
                p.Activated -= ItemActivated;
                p.Deactivated -= ItemDeactivated;
            }
        }
    }
}
