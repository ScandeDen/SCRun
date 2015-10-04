using Core.Objects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Sources;
using UnityEngine;

namespace Tests.Core.Objects
{
    public class StatableObjectTests : AbstractGameObjectTest<StatableObject>
    {
        public override StatableObject GetArrange()
        {
            var result = new GameObject().AddComponent<StatableObject>();
            return result;
        }

        [Test, TestCaseSource(typeof(StatesFactory), "ConflictableEmptyStates_ActivatedIndex")]
        public int AddState_AfterAwake_AreStatesIncludedCorrectAfterResolvingConflicts(params WrappedEmptyState[] states)
        {
            //arrange
            var instance = GetArrange();
            foreach (var ws in states)
                instance.AddState(ws.State);
            //act
            instance.Awake();
            //clearing
            objectsForTearDown.Add(instance.gameObject);
            //assert
            var activated = instance.GetActivatedStates().ToArray();
            Assert.AreEqual(activated.Count(), 1, "Activated state is not one");
            var i = 0;
            foreach (var ws in states)
            {
                if (ws.State == activated[0])
                    return i;
                i++;
            }
            return -1;
        }
    }
}
