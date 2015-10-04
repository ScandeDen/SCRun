using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Objects;

namespace Tests.Sources
{
    public class StatesFactory : AbstractSourceFactory
    {
        public static IEnumerable ConflictableEmptyStates_ActivatedIndex
        {
            get
            {
                var data = new List<WrappedEmptyState>();
                var state = new EmptyState("state1");
                state.ConflictKeyStates.Add(new AbstractState.StatesConflict("state4"));
                data.Add(new WrappedEmptyState(state));
                yield return new TestCaseData(data.ToArray()).Returns(0).
                    SetName("States:1,conflicts(forceDeactivate):{st1>st4(f)}");
                state = new EmptyState("state2");
                state.ConflictKeyStates.Add(new AbstractState.StatesConflict("state1", true));
                data.Add(new WrappedEmptyState(state));
                yield return new TestCaseData(data.ToArray()).Returns(1).
                    SetName("States:2,conflicts(forceDeactivate):{st1>st4(f)}" + 
                        ",{st2>st1(t)}");
                state = new EmptyState("state3");
                state.ConflictKeyStates.Add(new AbstractState.StatesConflict("state2"));
                data.Add(new WrappedEmptyState(state));
                yield return new TestCaseData(data.ToArray()).Returns(1).
                    SetName("States:3,conflicts(forceDeactivate):{st1>st4(f)}" +
                        ",{st2>st1(f)},{st3>st2(t)}");
                state = new EmptyState("state4");
                state.ConflictKeyStates.Add(new AbstractState.StatesConflict("state1", true));
                state.ConflictKeyStates.Add(new AbstractState.StatesConflict("state2", true));
                data.Add(new WrappedEmptyState(state));
                yield return new TestCaseData(data.ToArray()).Returns(3).
                    SetName("States:4,conflicts(forceDeactivate):{st1>st4(f)}" +
                        ",{st2>st1(f)},{st3>st2(t)},{st4>st1(t)}" +
                        ",{st4>st2(t)}");
            }
        }
    }

    public struct WrappedEmptyState
    {
        public EmptyState State;

        public WrappedEmptyState(EmptyState state)
        {
            State = state;
        }
    }

    public class EmptyState : AbstractState
    {
        public EmptyState(string tag)
            : base(tag) { }

        public override void ApplyState(StatableObject target) { }

        public override void ResetState(StatableObject target) { }
    }
}
