using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Service nodes are node that ticks OnUpdate() while the child node is active.
    /// </summary>
    public class EDBTService : EDBTDecorator
    {
        /// <summary>
        /// How long the service node waits before OnUpdate() is called again. If the value is non-positive, OnUpdate() is called every frame.
        /// </summary>
        private float interval = -1.0f;

        public EDBTService(string name, float tickRate, EDBTNode childNode) : base(name, childNode)
        {
            interval = tickRate;
        }

        public EDBTService(float tickRate, EDBTNode childNode) : base("Service", childNode)
        {
            interval = tickRate;
        }

        public override void OnStarted()
        {
            ChildNode.OnStarted();

            TimerManager.Instance.AddTimer(OnUpdate, interval, -1);
        }

        public override void OnChildFinishExecution(EDBTNode child, bool success)
        {
            FinishExecution(success);
            TimerManager.Instance.RemoveTimer(OnUpdate);
        }

        /// <summary>
        /// Invoked every frame when the child node is active.
        /// </summary>
        public virtual void OnUpdate()
        {
        }

        public override void OnBehaviorTreeDestroyed()
        {
            TimerManager.Instance.RemoveTimer(OnUpdate);
            base.OnBehaviorTreeDestroyed();
        }
    }
}
