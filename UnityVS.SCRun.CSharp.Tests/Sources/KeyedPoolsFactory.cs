using Core;
using Core.Objects;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tests.Sources
{
    public class KeyedPoolsFactory : AbstractSourceFactory
    {
        public static KeyedPool Pool1 = 
            new KeyedPool("Enemy1", Enemy1);
        public static KeyedPool Pool2 =
            new KeyedPool("Enemy2", Enemy2);
        public static KeyedPool Pool3 =
            new KeyedPool("Enemy3", Enemy3);
        public static KeyedPool Pool4 =
            new KeyedPool("Bonus1", Enemy1);
        
        public static IEnumerable KeyedPoolTestCases_CountOfPools
        {
            get
            {
                var keyedPools = new List<KeyedPool>();
                yield return new TestCaseData(keyedPools.ToArray()).Returns(0).
                    SetName("Keyed pools: 0");
                keyedPools.Add(Pool1);
                yield return new TestCaseData(keyedPools.ToArray()).Returns(1).
                    SetName("Keyed pools: pool1");
                var pool2 = Pool2;
                var pool3 = Pool3;
                keyedPools.Add(Pool2);
                keyedPools.Add(Pool3);
                yield return new TestCaseData(keyedPools.ToArray()).Returns(3).
                    SetName("Keyed pools: pool1, pool2, pool3");
                keyedPools.Add(Pool2);
                keyedPools.Add(Pool3);
                keyedPools.Add(Pool4);
                yield return new TestCaseData(keyedPools.ToArray()).Returns(4).
                    SetName("Keyed pools: pool1, pool2, pool3, pool2, pool3, pool4");
            }
        } 
        
    }
}
