using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.Spawners;
using NUnit.Framework;
using Core.Objects;
using Core;
using Core.Pools;
using Tests.Sources;

namespace Tests.Core.Spawners
{
    [TestFixture]
    public class WorldGeneratorTests : AbstractGameObjectTest<WorldGenerator>
    {
        public override WorldGenerator GetArrange()
        {
            var result = new GameObject().AddComponent<WorldGenerator>();
            var poolSet = result.gameObject.AddComponent<PoolsSet>();
            poolSet.Prefab = AbstractSourceFactory.SomethingPool;
            poolSet.Pools.
                 AddRange(new KeyedPool[] 
                    {KeyedPoolsFactory.Pool1, KeyedPoolsFactory.Pool2,
                        KeyedPoolsFactory.Pool3, KeyedPoolsFactory.Pool4});
            poolSet.InstantiatePoolsOnAwakeAtEditor = true;
            poolSet.Awake();
            return result;
        }

        [Test,
        TestCaseSource(typeof(SpawnGroupsFactory), "WorldItemGroupsTestCases")]
        public void OnStart_AfterAddingCases_CasesGenerateCorrectPosition(params WrappedWorldItemGroup[] cases)
        {
            int resultCount = 0;
            foreach (var wiig in cases)
            {
                resultCount += (int)(10 / wiig.Group.MaxCellsDelaySpawn);
            }
            //arrange
            var instance = GetArrange();
            instance.SizeOfCells = 1;
            instance.RowWidthInCells = 3;
            instance.InitRowCount = 10;
            foreach (var wwig in cases)
                instance.CaseGroups.Add(wwig.Group);
            //act
            instance.Awake();
            instance.Direction = new Vector3(0, 0, 1);
            instance.Start();
            var objects = GameObject.FindObjectsOfType<CellableObject>();
            var count = objects.Length;
            //clearing
            foreach (var obj in objects)
                objectsForTearDown.Add(obj.gameObject);
            objectsForTearDown.Add(instance.gameObject);
            //assert
            Assert.GreaterOrEqual(count, resultCount);
            foreach (var obj in objects)
                if (obj.gameObject.activeSelf)
                    foreach (var objOther in objects)
                        if (objOther != obj)
                            Assert.AreNotEqual(obj.transform.position, 
                                objOther.transform.position);
        }

        [Test,
        TestCaseSource(typeof(SpawnGroupsFactory), "WorldItemGroupsTestCases")]
        public void OnUpdate_AfterAddingCasesAndTrackingObject_CasesGenerateCorrectPosition(params WrappedWorldItemGroup[] cases)
        {
            int resultCount = 0;
            foreach (var wiig in cases)
            {
                resultCount += (int)(10 / wiig.Group.MaxCellsDelaySpawn);
            }
            //arrange
            var instance = GetArrange();
            instance.SizeOfCells = 1;
            instance.RowWidthInCells = 3;
            instance.InitRowCount = 0;
            instance.TrackingObject = GameObject.
                Instantiate<Transform>(AbstractSourceFactory.Character);
            instance.TrackingObject.position = new Vector3(-10, 0, 0);
            foreach (var wwig in cases)
                instance.CaseGroups.Add(wwig.Group);
            //act
            instance.Awake();
            instance.Direction = new Vector3(0, 0, 1);
            for (int i = 0; i < 21; i++)
            {
                instance.TrackingObject.position = 
                    new Vector3(instance.TrackingObject.position.x, 
                        instance.TrackingObject.position.y, 0.5f * i);
                instance.Update();
            }
            var objects = GameObject.FindObjectsOfType<CellableObject>();
            var count = objects.Length;
            //clearing
            objectsForTearDown.Add(instance.TrackingObject.gameObject);
            foreach (var obj in objects)
                objectsForTearDown.Add(obj.gameObject);
            objectsForTearDown.Add(instance.gameObject);
            //assert
            Assert.GreaterOrEqual(count, resultCount);
            foreach (var obj in objects)
                if (obj.gameObject.activeSelf)
                    foreach (var objOther in objects)
                        if (objOther != obj)
                            Assert.AreNotEqual(obj.transform.position,
                                objOther.transform.position);
        }
    }
}
