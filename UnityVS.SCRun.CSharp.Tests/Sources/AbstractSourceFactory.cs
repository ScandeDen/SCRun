﻿using Core.Objects;
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
        public static PoolableObject Enemy1 = GetNewPoolablePrefab<PoolableObject>("Objects/Enemy1.prefab");
        public static PoolableObject Enemy2 = GetNewPoolablePrefab<PoolableObject>("Objects/Enemy2.prefab");
        public static PoolableObject Enemy3 = GetNewPoolablePrefab<PoolableObject>("Objects/Enemy3.prefab");
        public static PoolableObject Bonus1 = GetNewPoolablePrefab<PoolableObject>("Objects/Bonus1.prefab");
        public static Transform Character = GetNewPoolablePrefab<Transform>("Objects/Character.prefab");
        public static GameObjectPool SomethingPool = GetNewPoolablePrefab<GameObjectPool>("SomethingPool.prefab");

        protected static TComponent GetNewPoolablePrefab<TComponent>(string pathInPrefabs)
            where TComponent : Component
        {
            var result = Resources.LoadAssetAtPath<TComponent>("Assets/Prefab/" +
                pathInPrefabs);
            return result;
        }
    }
}
