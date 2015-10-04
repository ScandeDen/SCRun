using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Core;
using System.Collections;
using UnityEngine;
using Core.Pools;
using Tests.Sources;

namespace Tests.Core
{
    [TestFixture]
    public class PoolSetTests : AbstractGameObjectTest<PoolsSet>
    {
        public override PoolsSet GetArrange()
        {
            var result = new GameObject().AddComponent<PoolsSet>();
            result.Prefab = AbstractSourceFactory.SomethingPool;
            return result;
        }

        [Test, TestCaseSource(typeof(KeyedPoolsFactory), "KeyedPoolTestCases_CountOfPools")]
        public int UpdatePools_AfterAddingEmptyKeyedPools_CorrectCountAtPoolsManager(params KeyedPool[] pools)
        {
            //arrange
            var instance = GetArrange();
            instance.Pools.AddRange(pools);
            //act
            instance.Update();
            var result = PoolsManager.Instance.Count();
            //clearing
            objectsForTearDown.Add(instance.gameObject);
            //assert
            return result;
        }

        [Test, TestCaseSource(typeof(KeyedPoolsFactory), "KeyedPoolTestCases_CountOfPools")]
        public int OnAwake_AfterAddingKeyedPools_PoolsAreInstantiated(params KeyedPool[] pools)
        {
            //arrange
            var instance = GetArrange();
            instance.Pools.AddRange(pools);
            //act
            instance.InstantiatePoolsOnAwakeAtEditor = true;
            instance.Awake();
            var objects = GameObject.FindObjectsOfType<GameObjectPool>();
            var result = objects.Length;
            //clearing
            objectsForTearDown.Add(instance.gameObject);
            for (int i = 0; i < objects.Length; i++)
                objectsForTearDown.Add(objects[i].gameObject);
            //assert
            return result;
        }
        
    }
}
