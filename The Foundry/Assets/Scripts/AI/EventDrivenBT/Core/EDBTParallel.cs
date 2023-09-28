using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Parallel nodes are nodes that perform a major task and a minor task at the same time. This node only stops when the major node stops.
    /// Still WIP, don't use this node.
    /// </summary>
    public class EDBTParallel : EDBTComposite
    {
        public EDBTNode MajorNode => ChildNodes[0];
        public EDBTNode MinorNode => ChildNodes[1];

        public EDBTParallel(EDBTNode majorNode, EDBTNode minorNode) : base("Parallel", new EDBTNode[2] { majorNode, minorNode })
        {
        }

        public EDBTParallel(string name, EDBTNode majorNode, EDBTNode minorNode) : base(name, new EDBTNode[2] { majorNode, minorNode })
        {
        }

        public override void OnStarted()
        {
            MajorNode.Start();
            MinorNode.Start();
        }

        public override void OnInterrupted()
        {
            MinorNode.Stop();
            MajorNode.Stop();
            CleanUpAndFinishExecution(false);
        }

        public override void OnChildFinishExecution(EDBTNode child, bool success)
        {
            // If the major node finishes execution, stop the minor node as well.
            if (child == MajorNode)
            {
                MinorNode.Stop();
                CleanUpAndFinishExecution(success);
            }

            // If the minor node finishes execution, restart the minor node if the major node is still active.
            else
            {
                if (MajorNode.CurrentState == State.Active)
                {
                    TimerManager.Instance.AddTimer(MinorNode.Start, 0.0f, 1);
                }
            }
        }

        public override void OnBehaviorTreeDestroyed()
        {
            TimerManager.Instance.RemoveTimer(MinorNode.Start);
            base.OnBehaviorTreeDestroyed();
        }

        private void CleanUpAndFinishExecution(bool success)
        {
            TimerManager.Instance.RemoveTimer(MinorNode.Start);
            FinishExecution(success);
        }
    }
}
