using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Wait for seconds, then return Success.
    /// </summary>
    public class EDBTTask_WaitBBEntry : EDBTTask
    {
        private string keyDuration;

        public EDBTTask_WaitBBEntry(string name, string keyDuration) : base(name)
        {
            this.keyDuration = keyDuration;
        }

        public override void OnStarted()
        {
            float duration;

            if (!Blackboard.TryGetValue(keyDuration, out duration))
            {
                Debug.LogError($"{Name}: Cannot get the value from the key \"{keyDuration}\".");
                FinishExecution(false);
                return;
            }

            if (duration <= 0)
            {
                FinishExecution(true);
            }
            else
            {
                TimerManager.Instance.AddTimer(ReturnSuccess, duration);
            }
        }

        public override void OnInterrupted()
        {
            TimerManager.Instance.RemoveTimer(ReturnSuccess);
            FinishExecution(false);
        }

        public void ReturnSuccess()
        {
            TimerManager.Instance.RemoveTimer(ReturnSuccess);
            FinishExecution(true);
        }

        public override void OnBehaviorTreeDestroyed()
        {
            TimerManager.Instance.RemoveTimer(ReturnSuccess);

            base.OnBehaviorTreeDestroyed();
        }
    }
}
