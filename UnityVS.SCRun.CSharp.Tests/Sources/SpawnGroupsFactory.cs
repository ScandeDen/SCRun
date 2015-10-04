using Core.Spawners;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tests.Sources
{
    public class SpawnGroupsFactory : AbstractSourceFactory
    {
        public static IEnumerable WorldItemGroupsTestCases
        {
            get
            {
                var groups = new List<WrappedWorldItemGroup>();
                yield return new TestCaseData(groups.ToArray()).
                    SetName("Spawned 0 group");
                var group = new WorldItemsGroup();
                group.MaxCellsDelaySpawn = 1;
                group.MinCellsDelaySpawn = 1;
                group.Cases.Add(new SpawnCase(KeyedPoolsFactory.Pool1.PoolKey, 100));
                groups.Add(new WrappedWorldItemGroup(group));
                yield return new TestCaseData(groups.ToArray()).
                    SetName("Spawned 1 group: {min1-max1: " +
                        KeyedPoolsFactory.Pool1.PoolKey + "}");
                group = new WorldItemsGroup();
                group.MaxCellsDelaySpawn = 2;
                group.MinCellsDelaySpawn = 2;
                group.Cases.Add(new SpawnCase(KeyedPoolsFactory.Pool1.PoolKey, 100));
                group.MaxCellsDelaySpawn = 3;
                group.MinCellsDelaySpawn = 3;
                group.Cases.Add(new SpawnCase(KeyedPoolsFactory.Pool2.PoolKey, 50));
                groups.Add(new WrappedWorldItemGroup(group));
                yield return new TestCaseData(groups.ToArray()).
                    SetName("Spawned 2 group: {min1-max1: " + KeyedPoolsFactory.Pool1.PoolKey
                        + "},{min2-max2: " + KeyedPoolsFactory.Pool1.PoolKey
                        + " min3-max3: " + KeyedPoolsFactory.Pool2.PoolKey + "}");
            }
        }
    }

    public struct WrappedWorldItemGroup
    {
        public WorldItemsGroup Group;

        public WrappedWorldItemGroup(WorldItemsGroup group)
        {
            this.Group = group;
        }
    }
}
