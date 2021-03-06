﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.Spawners;
using Core.Objects;

namespace Core.Spawners
{
    /// <summary>
    /// Генератор игрового пространства, который делит его на нужную ему сетку. Реализует 
    /// функционал спавнера используя свой вид групп вариантов. Реагирует на передвижение 
    /// отслеживаемого объекта.
    /// </summary>
    public class WorldGenerator : AbstractSpawner<WorldItemsGroup, SpawnCase>
    {
        /// <summary>
        /// Отслеживаемый объект.
        /// </summary>
        [Header("World generation")]
        public Transform TrackingObject;
        /// <summary>
        /// Направление генерирования.
        /// </summary>
        [SerializeField]
        protected Vector3 direction;
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
                normalizedDirection = direction.normalized;
                var trans = GetCashedComponent<Transform>();
                trans.LookAt(trans.position + direction);
            }
        }
        /// <summary>
        /// Число строк, которое нужно сгенерировать сразу при активации.
        /// </summary>
        public int InitRowCount = 10;
        /// <summary>
        /// Ширина строки в клетках.
        /// </summary>
        public int RowWidthInCells = 3;
        /// <summary>
        /// Размер клетки формы квадрата.
        /// </summary>
        public float SizeOfCells = 1f;
        /// <summary>
        /// Координаты последней обработанной строки пространства.
        /// </summary>
        protected Vector3 lastFilledRow;
        /// <summary>
        /// Нормаль направления спавна.
        /// </summary>
        protected Vector3 normalizedDirection;
        /// <summary>
        /// Массив для отметки уже заполненных ячеек в следующей строке.
        /// </summary>
        protected bool[] placesInNextRow;
        /// <summary>
        /// Последняя позиция строки на которой был замечен отслеживаемый объект.
        /// </summary>
        protected Vector3 lastPositionOfTrackingObject;

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnStart()
        {
            lastFilledRow = GetCashedComponent<Transform>().position;
            normalizedDirection = direction.normalized;
            var trans = GetCashedComponent<Transform>();
            trans.LookAt(trans.position + direction);
            if (TrackingObject != null)
                lastPositionOfTrackingObject = TrackingObject.position;
            foreach (var i in CaseGroups)
            {
                i.ItemSpawned();
            }
            for (int i = 0; i < InitRowCount; i++)
            {
                ShiftToNextRow();
            }
        }

        public void Update()
        {
            if (TrackingObject == null)
                return;
            var trans = GetCashedComponent<Transform>();
            var plane = new Plane(trans.forward, lastPositionOfTrackingObject + 
                normalizedDirection * SizeOfCells);
            var trackingObjPosition = TrackingObject.position;
            var distanceToTracking = 
                (trackingObjPosition - lastPositionOfTrackingObject).magnitude;
            if (plane.GetSide(TrackingObject.position)
                || plane.GetDistanceToPoint(trackingObjPosition) == 0)
            {
                var rmDiv = distanceToTracking % SizeOfCells;
                var count = rmDiv < 1 ? 1 : rmDiv;
                lastPositionOfTrackingObject = trackingObjPosition 
                    + plane.normal * (-1) * (distanceToTracking - (count * SizeOfCells));
                for (int i = 0; i < count; i++)
                {
                    ShiftToNextRow();
                }
            }
        }
        /// <summary>
        /// Сдвигает на следующую строку, генерируя ее.
        /// </summary>
        public void ShiftToNextRow()
        {
            placesInNextRow = new bool[RowWidthInCells];
            lastFilledRow += normalizedDirection * SizeOfCells;
            foreach (var cs in CaseGroups)
            {
                SpawnFromChosenedCase(cs);
            }
        }
        /// <summary>
        /// Реализует функцию абстрактного спавнера.
        /// </summary>
        /// <param name="chosened"> Группа вариантов. </param>
        public override void SpawnFromChosenedCase(WorldItemsGroup chosened)
        {
            if (chosened == null)
                return;
            var spawnCase = SwitchRandomCase(chosened.Cases);
            if (spawnCase == null)
            {
                chosened.RowsShiftedPerOne();
                return;
            }
            var spawnItem = PoolsManager.Instance.GetPool(spawnCase.UsedPool).
                    GetFreeItem();
            if (spawnItem == null)
            {
                chosened.RowsShiftedPerOne();
                return;
            }
            chosened.RowsShiftedPerOne();
            if (chosened.IsCanSpawned() && spawnCase != null)
            {
                spawnItem.ResetChanges();
                var transformIns = GetCashedComponent<Transform>();
                var itemCellable = spawnItem.GetComponent<CellableObject>();
                var x = 0;
                var avalableCellSets = new List<List<int>>();
                List<int> currentSet = null;
                for (var i = 0; i < RowWidthInCells; i++)
                {

                    if (!placesInNextRow[i])
                    {
                        if (currentSet == null)
                        {
                            currentSet = new List<int>();
                            avalableCellSets.Add(currentSet);
                        }
                        currentSet.Add(i);
                    }
                    else
                    {
                        if (currentSet != null)
                        {
                            
                            currentSet = null;
                        }
                    }
                }
                var width = itemCellable.WidthInCells;
                var isCanSets = avalableCellSets.Where(o => o.Count >= width);
                if (isCanSets.Count() != 0)
                {
                    var chosenedRange = isCanSets.
                        ToArray()[Random.Range(0, isCanSets.Count())];
                    x = Random.
                        Range(chosenedRange[0] + itemCellable.LeftEmptyCellsCount,
                            chosenedRange[chosenedRange.Count - 1] -
                                itemCellable.RightEmptyCellsCount + 1);
                    for (int i = x - itemCellable.LeftEmptyCellsCount;
                        i <= x + itemCellable.RightEmptyCellsCount; i++)
                    {
                        placesInNextRow[i] = true;
                    }
                    var shift = Vector3.right * ((x + 0.5f) * SizeOfCells);
                    var itemTransform = spawnItem.GetComponent<Transform>();
                    itemTransform.localPosition = lastFilledRow + shift + Offset;
                    itemTransform.SetParent(transformIns, false);
                    spawnItem.Activate();
                    chosened.ItemSpawned();
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Группа вариантов спавна для генератора пространства.
    /// </summary>
    [System.Serializable]
    public class WorldItemsGroup : SpawnGroup<SpawnCase>
    {
        /// <summary>
        /// Минимальное количество строк между объектами.
        /// </summary>
        public int MinCellsDelaySpawn = 5;
        /// <summary>
        /// Максимальное количество строк между объектами.
        /// </summary>
        public int MaxCellsDelaySpawn = 6;
        /// <summary>
        /// Строк пройдено с последнего использования этой группы.
        /// </summary>
        protected int rowsElapsedFromLastSpawn;
        /// <summary>
        /// Строк необходимо для спавна следующего объекта этой группы.
        /// </summary>
        protected int rowsForSpawnNextItem;

        /// <summary>
        /// Обрабатывает спавн объекта из этой группы.
        /// </summary>
        public void ItemSpawned()
        {
            rowsElapsedFromLastSpawn = 0;
            rowsForSpawnNextItem = Random.Range(MinCellsDelaySpawn,
                MaxCellsDelaySpawn + 1);
        }
        /// <summary>
        /// Может ли группа спавнить объект на данный момент.
        /// </summary>
        /// <returns> True, если спавн возможен. </returns>
        public bool IsCanSpawned()
        {
            return rowsElapsedFromLastSpawn >= rowsForSpawnNextItem;
        }
        /// <summary>
        /// Сдвигает счетчик строк на одну для этой группы.
        /// </summary>
        public void RowsShiftedPerOne()
        {
            rowsElapsedFromLastSpawn++;
        }
    }
}
