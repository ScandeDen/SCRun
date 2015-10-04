using Core.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Одиночка менеджер пулов. Хранит библиотеку ссылок на пулы с доступом по ключевой 
    /// строке-названию пула и обеспечивает работу с ними.
    /// </summary>
    public class PoolsManager
    {
        private static PoolsManager instance;
        public static PoolsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PoolsManager();
                return instance;
            }
        }

        /// <summary>
        /// Библиотека ссылок на пулы.
        /// </summary>
        private Dictionary<string, GameObjectPool> pools = 
            new Dictionary<string, GameObjectPool>();

        private PoolsManager()
        {

        }
        /// <summary>
        /// Возвращает пул, используя строку как ключ.
        /// </summary>
        /// <param name="poolKey"> Ключевая строка. </param>
        /// <returns> Пул. </returns>
        public GameObjectPool GetPool(string poolKey)
        {
            if (!pools.ContainsKey(poolKey))
                return null;
            return pools[poolKey];
        }
        /// <summary>
        /// Возвращает ключ пула, размещенного по заданному индексу.
        /// </summary>
        /// <param name="index"> Индекс пары в библиотеке, которая хранит пулы. </param>
        /// <returns> Ключ-строка пула. </returns>
        public string GetPoolKey(int index)
        {
            return pools.Keys.ToArray()[index];
        }
        /// <summary>
        /// Возращает индекс пары с такой ключ-строкой.
        /// </summary>
        /// <param name="poolKey"> Ключ-строка. </param>
        /// <returns> Индекс пары. </returns>
        public int GetPoolIndex(string poolKey)
        {
            return pools.Keys.ToList().IndexOf(poolKey);
        }
        /// <summary>
        /// Возращает список всех ключ-строк, названий пулов.
        /// </summary>
        /// <returns> Список ключ-строк. </returns>
        public List<string> GetPoolKeys()
        {
            return pools.Keys.ToList();
        }
        /// <summary>
        /// Добавляет пул и размещает его в билиотеке по заданному ключу.
        /// </summary>
        /// <param name="poolKey"> Ключ-строка. </param>
        /// <param name="pool"> Пул. </param>
        public void SetPool(string poolKey, GameObjectPool pool)
        {
            pools[poolKey] = pool;
        }
        /// <summary>
        /// Удаляет пул из библиотеки.
        /// </summary>
        /// <param name="poolKey"> Ключ-строка. </param>
        public void RemovePool(string poolKey)
        {
            if (pools.ContainsKey(poolKey))
                pools.Remove(poolKey);
        }
        /// <summary>
        /// Очищает библиотеку пулов.
        /// </summary>
        public void Clear()
        {
            pools.Clear();
        }
        /// <summary>
        /// Возвращает число зарегестрированных пулов.
        /// </summary>
        /// <returns> Количество пулов. </returns>
        public int Count()
        {
            return pools.Count();
        }
    }
}
