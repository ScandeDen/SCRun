using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tests.Sources
{
    public class DirectionSourceFactory : AbstractSourceFactory
    {
        public static IEnumerable AllDirectionTestCases
        {
            get
            {
                yield return new TestCaseData(Vector3.zero).
                    SetName("Direction:" + Vector3.zero);
                yield return new TestCaseData(Vector3.right).
                    SetName("Direction:" + Vector3.right);
                yield return new TestCaseData(Vector3.left).
                    SetName("Direction:" + Vector3.left);
                yield return new TestCaseData(Vector3.forward).
                    SetName("Direction:" + Vector3.forward);
                yield return new TestCaseData(Vector3.back).
                    SetName("Direction:" + Vector3.back);
                yield return new TestCaseData(Vector3.up).
                    SetName("Direction:" + Vector3.up);
                yield return new TestCaseData(Vector3.down).
                    SetName("Direction:" + Vector3.down);
                yield return new TestCaseData(Vector3.one).
                    SetName("Direction:" + Vector3.one);
            }
        } 
    }
}
