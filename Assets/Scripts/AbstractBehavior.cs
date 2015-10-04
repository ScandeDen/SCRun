using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Defaults
{
    /// <summary>
    /// Абстрактное поведение, реализует хранение ссылок на компоненты.
    /// </summary>
    public abstract class AbstractBehavior : MonoBehaviour
    {
        /// <summary>
        /// Библиотека, которая хранит ссылки на компоненты, используя как ключ их тип.
        /// </summary>
        protected Dictionary<Type, Component> cashedComponents =
            new Dictionary<Type, Component>();

        public void Awake()
        {
            OnAwake();
        }

        public void Start()
        {
            OnStart();
        }

        protected abstract void OnAwake();

        protected abstract void OnStart();
        /// <summary>
        /// Кэширует компонент для дальнейшего быстрого доступа.
        /// </summary>
        /// <typeparam name="TComponent"> Тип компонента, который будет использоваться 
        /// как ключ. </typeparam>
        protected void CashComponent<TComponent>()
            where TComponent : Component
        {
            var c = GetComponent<TComponent>();
            if (c == null)
                return;
            cashedComponents[typeof(TComponent)] = c;
        }
        /// <summary>
        /// Возвращает кэшированный ранее компонент.
        /// </summary>
        /// <typeparam name="TComponent"> Тип компонента. </typeparam>
        /// <returns> Компонент. </returns>
        public TComponent GetCashedComponent<TComponent>()
            where TComponent : Component
        {
            var t = typeof(TComponent);
            if (!cashedComponents.ContainsKey(t))
                return null;
            return (TComponent)cashedComponents[t];
        }
    }
}