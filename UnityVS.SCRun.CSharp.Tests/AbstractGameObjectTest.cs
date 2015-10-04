﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tests
{
    [TestFixture]
    public abstract class AbstractGameObjectTest<TComponent>
        where TComponent : Component
    {
        protected List<GameObject> objectsForTearDown = new List<GameObject>();

        public abstract TComponent GetArrange();

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {
            for (var i = 0; i < objectsForTearDown.Count; i++)
            {
                GameObject.DestroyImmediate(objectsForTearDown[i]);
            }
            objectsForTearDown.Clear();
        }
    }
}
