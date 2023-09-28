using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Multi-frame tasks are tasks that are executed in multiple frames instead of one single frame.
    /// </summary>
    public abstract class EDBTMultiFrameTask : EDBTTask
    {
        public EDBTMultiFrameTask(string name) : base(name)
        {
        }

        public override void OnStarted()
        {
            // Check if the task can be executed before starting the loop.
            if (!IsConditionMet())
            {
                CleanUpAndFinishExecution(false);
            }

            // Start the loop.
            else
            {
                TimerManager.Instance.AddTimer(OnUpdate, 0, -1);
            }
        }

        public override void OnInterrupted()
        {
            CleanUpAndFinishExecution(false);
        }

        /// <summary>
        /// Check if the condition is met before starting the loop.
        /// </summary>
        public abstract bool IsConditionMet();

        /// <summary>
        /// Executed on every frame.
        /// </summary>
        public abstract void OnUpdate();

        protected virtual void CleanUpAndFinishExecution(bool success)
        {
            TimerManager.Instance.RemoveTimer(OnUpdate);
            FinishExecution(success);
        }

        public override void OnBehaviorTreeDestroyed()
        {
            TimerManager.Instance.RemoveTimer(OnUpdate);
            base.OnBehaviorTreeDestroyed();
        }
    }
}
