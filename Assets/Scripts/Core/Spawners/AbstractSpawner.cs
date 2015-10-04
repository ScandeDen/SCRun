using Core.Objects;
using Core.Pools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Spawners
{
    /// <summary>
    /// Абстрактный спавнер игровых объектов, исопльзующий пулы для производства и 
    /// контроля популяции объектов.
    /// </summary>
    /// <typeparam name="TSpawnGroup"> Группа вариантов спавна. </typeparam>
    /// <typeparam name="TSpawnCase"> Вариант спавна. </typeparam>
    public abstract class AbstractSpawner<TSpawnGroup, TSpawnCase> : AbstractObject
        where TSpawnGroup : SpawnGroup<TSpawnCase>
        where TSpawnCase : SpawnCase
    {
        /// <summary>
        /// Смещение, которое будет применено ко всем произведенным объектам.
        /// </summary>
        [Header("Spawn")]
        public Vector3 Offset;
        /// <summary>
        /// Группы вариантов.
        /// </summary>
        public List<TSpawnGroup> CaseGroups = new List<TSpawnGroup>();

        protected override void OnAwake()
        {
            CashComponent<Transform>();
        }
        /// <summary>
        /// Выбирает случайный вариант из предоставленного списка.
        /// </summary>
        /// <param name="group"> Список вариантов. </param>
        /// <returns> Выбранный случайный вариант.  </returns>
        protected virtual TSpawnCase SwitchRandomCase(IEnumerable<TSpawnCase> group)
        {
            var currentChances = Random.Range(1, 100);
            var chosens = group.Where(o => o.ChanceWeight >= currentChances).ToArray();
            if (chosens.Length == 0)
                return null;
            var closer = chosens[0];
            int rem = Mathf.Abs(closer.ChanceWeight - currentChances);
            foreach (var c in chosens)
            {
                var locRem = Mathf.Abs(c.ChanceWeight - currentChances);
                if (rem > locRem)
                {
                    rem = locRem;
                    closer = c;
                }
            }
            return closer;
        }
        /// <summary>
        /// Производит инициализацию группы вариантов. Создает экземпляры при помощи пулов 
        /// и активирует их.
        /// </summary>
        /// <param name="chosened"> Группа вариантов. </param>
        public abstract void SpawnFromChosenedCase(TSpawnGroup chosened);
    }

    /// <summary>
    /// Группа вариантов для спавна.
    /// </summary>
    /// <typeparam name="TCase"> Тип вариантов. </typeparam>
    public class SpawnGroup<TCase> where TCase : SpawnCase
    {
        /// <summary>
        /// Варианты.
        /// </summary>
        public List<TCase> Cases = new List<TCase>();
    }
    /// <summary>
    /// Вариант для спавна, содержащий минимум информации необходимый любому спавнеру.
    /// </summary>
    [System.Serializable]
    public class SpawnCase
    {
        /// <summary>
        /// Используемый пул.
        /// </summary>
        [Gui.Editor.PoolKey("String key in pool manager")]
        public string UsedPool;
        /// <summary>
        /// Шансы на появление объекта.
        /// </summary>
        [Range(1, 100)]
        public int ChanceWeight;

        public SpawnCase(string usedPoolKey, int chances)
        {
            UsedPool = usedPoolKey;
            ChanceWeight = chances;
        }
    }
}
