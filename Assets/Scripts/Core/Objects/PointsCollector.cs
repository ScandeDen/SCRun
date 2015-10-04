using Core.Objects;
using Core.Objects.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Компонент для сбора очков, реагирует на определенные тэги игровых объектов и 
    /// начисляет очки в указанный счетчик. Также перестает их собирать при некоторых 
    /// условиях.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PointsCollector : AbstractObject
    {
        /// <summary>
        /// Свойство жизней у отслежимаемого объекта.
        /// </summary>
        public DamagableObject Life;
        /// <summary>
        /// Свойство очков у отслеживаемого объекта.
        /// </summary>
        public PointsCounter Counter;
        /// <summary>
        /// Очков за единицу времени (сек.).
        /// </summary>
        public int PointsPerSecond = 1;
        /// <summary>
        /// Тэги игровых объектов и количество очков, поулчаемых за их вхождение в зону 
        /// триггера.
        /// </summary>
        public List<PointsGetter> TagsForCollectingPoints = new List<PointsGetter>();
        /// <summary>
        /// Корутина получения очков в единицу времени.
        /// </summary>
        protected Defaults.SafeCoroutine pointsPerSecond;

        protected override void OnAwake()
        {
            base.OnAwake();
            pointsPerSecond = new Defaults.SafeCoroutine(this);
            if (Life != null)
                Life.ValueChanged += LifeValueChanged;
        }
        /// <summary>
        /// Обрабатывает изменение свойства очков жизни.
        /// </summary>
        /// <param name="sender"> Свойство. </param>
        /// <param name="oldVal"> Старое значение переменной. </param>
        void LifeValueChanged(Property sender, int oldVal)
        {
            if (sender.Value <= 0)
            {
                pointsPerSecond.Stop();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (Counter == null)
                return;
            foreach (var pointsGet in TagsForCollectingPoints)
            {
                if (other.gameObject.CompareTag(pointsGet.Tag))
                {
                    Counter.Value += pointsGet.Points;
                }
            }
        }

        void OnEnable()
        {
            pointsPerSecond.Start(CollectingPointsPerTime());
        }

        void OnDisable()
        {
            pointsPerSecond.Stop();
        }
        /// <summary>
        /// Возвращает энумератор, который генерирует очки вединицу времени.
        /// </summary>
        /// <returns> Энумератор. </returns>
        protected IEnumerator CollectingPointsPerTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if (Counter != null)
                    Counter.Value += PointsPerSecond;
            }
        }

        void OnDestroy()
        {
            if (Life != null)
                Life.ValueChanged -= LifeValueChanged;
        }
    }

    [System.Serializable]
    public struct PointsGetter
    {
        public string Tag;
        public int Points;
    }
}
