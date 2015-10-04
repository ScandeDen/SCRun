using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Objects
{
    /// <summary>
    /// Компонент, содержащий состояния объекта и осуществляющий контроль над ними, 
    /// поддерживая различные дополнительные возможности для разрешения конфликтов 
    /// перекрытия одних состояний другими.
    /// </summary>
    public class StatableObject : AbstractObject
    {
        /// <summary>
        /// Текущие состояния.
        /// </summary>
        protected List<IState> current = new List<IState>();

        /// <summary>
        /// Добавляет состояние в список текущих и активирует если это допустимо.
        /// </summary>
        /// <param name="state"> Состояние. </param>
        /// <param name="isActivate"> Пытаться ли активировать состояние. </param>
        public void AddState(IState state, bool isActivate = true)
        {
            current.Add(state);
            if (isActivate && state.IsCanActivate(this))
            {
                ForceConflictResolvingFor(state);
                ActivateState(state);
            }
        }
        /// <summary>
        /// Удаляет состояние из текущих, разрешая конфликтующие с ним.
        /// </summary>
        /// <param name="state"> Состояние. </param>
        public void RemoveState(IState state)
        {
            if (!current.Contains(state))
                return;
            current.Remove(state);
            state.Deactivate(this);
            CheckResolvedStateConflicts();
        }
        /// <summary>
        /// Деактивирует состояние, разрешая конфликтующие с ним.
        /// </summary>
        /// <param name="state"> Состояние. </param>
        public void DeactivateState(IState state)
        {
            if (!current.Contains(state))
                return;
            if (!state.IsActivated())
                return;
            state.Deactivate(this);
            CheckResolvedStateConflicts();
        }
        /// <summary>
        /// Активирует состояние, принуждая к деактивации допустимые конфликтующие с ним. 
        /// </summary>
        /// <param name="state"> Состояние. </param>
        public void ActivateState(IState state)
        {
            if (!current.Contains(state))
                return;
            if (!current.Contains(state))
                return;
            if (state.IsActivated())
                return;
            if (state.IsCanActivate(this))
            {
                ForceConflictResolvingFor(state);
                state.Activate(this);
            }
        }
        /// <summary>
        /// Содержит ли состояние.
        /// </summary>
        /// <param name="state"> Состояние. </param>
        /// <returns> True, если содержит. </returns>
        public bool ContainsState(IState state)
        {
            return current.Contains(state);
        }
        /// <summary>
        /// Содержит ли состояния с таким тегом.
        /// </summary>
        /// <param name="stateTag"> Тэг состояния. </param>
        /// <returns> True, если содержит хоть одно. </returns>
        public bool ContainsState(string stateTag)
        {
            return current.Exists(o => o.GetStateTag() == stateTag);
        }
        /// <summary>
        /// Содержит ли активные состояния с таким тегом.
        /// </summary>
        /// <param name="stateTag"> Тэг состояния. </param>
        /// <returns> True, если содержит хоть одно. </returns>
        public bool ContainsActivatedState(string stateTag)
        {
            return current.Exists(o => o.GetStateTag() == stateTag && o.IsActivated());
        }
        /// <summary>
        /// Проверяет все существующие конфликты и разрешает их если это возможно.
        /// </summary>
        protected void CheckResolvedStateConflicts()
        {
            foreach (var deactivated in 
                current.Where(o => !o.IsActivated()))
            {
                if (deactivated.IsCanActivate(this))
                {
                    deactivated.Activate(this);
                }
            }
        }
        /// <summary>
        /// Разрешает конфликты для состояния в принудительном порядке если такое возможно.
        /// </summary>
        /// <param name="forceAddingState"> Состояние. </param>
        protected void ForceConflictResolvingFor(IState forceAddingState)
        {
            foreach (var state in current)
            {
                if (forceAddingState.IsCanForceDeactivate(state))
                {
                    state.Deactivate(this);
                }
            }
        }
        /// <summary>
        /// Возвращает все активные состояния.
        /// </summary>
        /// <returns> Перечисление активных состояний. </returns>
        public IEnumerable<IState> GetActivatedStates()
        {
            return current.Where(o => o.IsActivated());
        }
    }

    /// <summary>
    /// Интерфейс состояния.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Активированно ли состояние на данный момент.
        /// </summary>
        /// <returns> True, если активированно. </returns>
        bool IsActivated();
        /// <summary>
        /// Возвращает тэг состояния.
        /// </summary>
        /// <returns> Тэг. </returns>
        string GetStateTag();
        /// <summary>
        /// Активирует состояние.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        void Activate(StatableObject target);
        /// <summary>
        /// Деактивирует состояние.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        void Deactivate(StatableObject target);
        /// <summary>
        /// Может ли состояние быть активированно.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        /// <returns> True, если может быть активирован. </returns>
        bool IsCanActivate(StatableObject target);
        /// <summary>
        /// Может ли состояние принудительно деактивировать заданное.
        /// </summary>
        /// <param name="target"> Целевое состояние. </param>
        /// <returns> True, если может деактивировать. </returns>
        bool IsCanForceDeactivate(IState conflictState);
    }
    /// <summary>
    /// Абстрактное состояние, реализует основные функции и предоставляет свои перегружаемые 
    /// функции для применения и сброса эффекта состояния.
    /// </summary>
    [System.Serializable]
    public abstract class AbstractState : IState
    {
        /// <summary>
        /// Конфликтующие состояния.
        /// </summary>
        public List<StatesConflict> ConflictKeyStates = new List<StatesConflict>();
        /// <summary>
        /// Тэг состояния.
        /// </summary>
        [SerializeField]
        protected string stateTag;
        /// <summary>
        /// Активно ли в данный момент состояние.
        /// </summary>
        protected bool isActivated;

        public AbstractState(string stateTag = "")
        {
            this.stateTag = stateTag;
        }
        /// <summary>
        /// Возвращает тэг состояния.
        /// </summary>
        /// <returns> Тэг. </returns>
        public string GetStateTag()
        {
            return stateTag;
        }
        /// <summary>
        /// Активированно ли состояние на данный момент.
        /// </summary>
        /// <returns> True, если активированно. </returns>
        public bool IsActivated()
        {
            return isActivated;
        }
        /// <summary>
        /// Активирует состояние.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        public void Activate(StatableObject target)
        {
            isActivated = true;
            ApplyState(target);
        }
        /// <summary>
        /// Деактивирует состояние.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        public void Deactivate(StatableObject target)
        {
            isActivated = false;
            ResetState(target);
        }
        /// <summary>
        /// Применяет эффекты состояния на объект.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        public abstract void ApplyState(StatableObject target);
        /// <summary>
        /// Снимает эффекты состояния с объекта.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        public abstract void ResetState(StatableObject target);
        /// <summary>
        /// Может ли состояние быть активированно.
        /// </summary>
        /// <param name="target"> Компонент состояний целевого объекта. </param>
        /// <returns> True, если может быть активирован. </returns>
        public bool IsCanActivate(StatableObject target)
        {
            foreach (var conflict in ConflictKeyStates)
            {
                if (target.ContainsActivatedState(conflict.StateKey)
                    && !conflict.IsForceDeactivate)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Может ли состояние принудительно деактивировать заданное.
        /// </summary>
        /// <param name="target"> Целевое состояние. </param>
        /// <returns> True, если может деактивировать. </returns>
        public bool IsCanForceDeactivate(IState conflictState)
        {
            foreach (var conflict in ConflictKeyStates)
            {
                if (conflict.StateKey == conflictState.GetStateTag()
                    && conflict.IsForceDeactivate)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Структура конфликта состояния с группой, обозначенной общим тегом.
        /// </summary>
        [System.Serializable]
        public struct StatesConflict
        {
            /// <summary>
            /// Тег группы состояний.
            /// </summary>
            public string StateKey;
            /// <summary>
            /// Может ли состояние принудительно деактивировать конфликтующее.
            /// </summary>
            public bool IsForceDeactivate;

            public StatesConflict(string stateKey, bool isForceDeactivate = false)
            {
                StateKey = stateKey;
                IsForceDeactivate = isForceDeactivate;
            }
        }
    }
}
