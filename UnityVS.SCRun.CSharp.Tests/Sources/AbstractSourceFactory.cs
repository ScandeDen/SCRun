using Core.Objects;
using Core.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tests.Sources
{
    public abstract class AbstractSourceFactory
    {
        public static PoolableObject Enemy1
        {
            get
            {
                return GetPrefab<PoolableObject>("Objects/Enemy1");
            }
        }

        public static PoolableObject Enemy2
        {
            get
            {
                return GetPrefab<PoolableObject>("Objects/Enemy2");
            }
        }

        public static PoolableObject Enemy3
        {
            get
            {
                return GetPrefab<PoolableObject>("Objects/Enemy3");
            }
        }

        public static PoolableObject Bonus1
        {
            get
            {
                return GetPrefab<PoolableObject>("Objects/Bonus1");
            }
        }

        public static Transform Character
        {
            get
            {
                return GetPrefab<Transform>("Objects/Character");
            }
        }

        public static GameObjectPool SomethingPool
        {
            get
            {
                return GetPrefab<GameObjectPool>("SomethingPool");
            }
        }

        protected static TComponent GetPrefab<TComponent>(string pathInPrefabs)
            where TComponent : Component
        {
            return Resources.Load<TComponent>("Prefab/" + pathInPrefabs);
        }
    }
}
