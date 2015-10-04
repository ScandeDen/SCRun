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
    public class MovementTests : AbstractGameObjectTest<Movement>
    {
        public override Movement GetArrange()
        {
            var result = new GameObject().AddComponent<Movement>();
            result.SpeedVector = new Vector3(0, 0, 1);
            result.JumpPower = 200f;
            result.ShiftDistance = 1f;
            result.ShiftDelay = 0f;
            return result;
        }

        [Test, TestCaseSource(typeof(DirectionSourceFactory),"AllDirectionTestCases")]
        public void OnFixedUpdate_AfterAwakeSpeedVectorSetted_IsDestinationReachedAtCorrectTime(Vector3 speedVector)
        {
            //arrange
            var instance = GetArrange();
            instance.SpeedVector = speedVector;
            var destination = instance.transform.position + speedVector * 10;
            //act
            instance.Awake();
            instance.IsOn = true;
            instance.Start();
            for (int i = 0; i < 20; i++)
            {
                instance.OnFixedUpdate(0.5f);
            }
            //clearing
            objectsForTearDown.Add(instance.gameObject);
            //assert
            Assert.LessOrEqual((destination - instance.transform.position).magnitude, 
                0.00001);
        }
    }
}
