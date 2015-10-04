using Core.Objects;
using Core.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Набор пулов с префабом, который они будут использовать при создании своих экземпляров.
    /// </summary>
    [ExecuteInEditMode]
    public class PoolsSet : AbstractObject
    {
        /// <summary>
        /// Разрешает инстанцировать пулы даже в режиме радактора.
        /// </summary>
        public bool InstantiatePoolsOnAwakeAtEditor;
        /// <summary>
        /// Префаб, который будет использоваться при создании пулов.
        /// </summary>
        public GameObjectPool Prefab;
        /// <summary>
        /// Список пар ключ - объект. При начале работы компонента будут созданы пулы, 
        /// которые будут добавлены в менеджер пулов. Объект эти пулы используют как префаб 
        /// для создания своих объектов.
        /// </summary>
        public List<KeyedPool> Pools = new List<KeyedPool>();
        /// <summary>
        /// Список занятых ключей после последнего обращения к менеджеру пулов.
        /// </summary>
        protected List<string> oldPools = new List<string>();
        
        protected override void OnAwake()
        {
            base.OnAwake();
            if (Application.isPlaying || InstantiatePoolsOnAwakeAtEditor)
            {
                RefreshManager(true);
            }
        }

        public void Update()
        {
            if (!Application.isPlaying)
            {
                RefreshManager();
            }
        }
        /// <summary>
        /// Обновляет менеджер пулов, добавляя в него соответствующие пулы.
        /// </summary>
        protected void RefreshManager(bool instantiatePools = false)
        {
            foreach (var pk in oldPools)
            {
                if (!Pools.Exists(o=>o.PoolKey == pk))
                {
                    PoolsManager.Instance.RemovePool(pk);
                }
            }
            oldPools.Clear();
            foreach (var p in Pools)
            {
                GameObjectPool pool = PoolsManager.Instance.GetPool(p.PoolKey);
                if (instantiatePools && pool == null)
                {
                    pool = Instantiate<GameObjectPool>(Prefab);
                    pool.Origin = p.Prefab;
                    pool.transform.SetParent(GetCashedComponent<Transform>());
                    PoolsManager.Instance.SetPool(p.PoolKey, pool);
                }
                PoolsManager.Instance.SetPool(p.PoolKey, pool);
                oldPools.Add(p.PoolKey);
            }
        }

        public void OnDestroy()
        {
            foreach (var p in Pools)
            {
                PoolsManager.Instance.RemovePool(p.PoolKey);
            }
        }
    }

    /// <summary>
    /// Структура, передающая префабы пулам при инициализации по ключу в менеджере пулов.
    /// </summary>
    [System.Serializable]
    public struct KeyedPool
    {
        public string PoolKey;
        public PoolableObject Prefab; 

        public KeyedPool(string key, PoolableObject prefab = null)
        {
            PoolKey = key;
            Prefab = prefab;
        }
    }
}
