using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Wait for seconds, then return Success.
    /// </summary>
    public class EDBTTask_Wait : EDBTTask
    {
        private float duration;
        private float deviation;

        public EDBTTask_Wait(string name, float duration, float deviation = 0) : base(name)
        {
            if (duration < 0)
                duration = 0;

            this.duration = duration;

            if (deviation < 0)
                deviation = 0;

            this.deviation = deviation;
        }

        public EDBTTask_Wait(float duration, float deviation = 0) : this($"Wait for {duration} second(s)", duration, deviation)
        {
        }

        public override void OnStarted()
        {
            float realDuration = duration + Random.Range(-deviation, deviation);
            if (realDuration < 0)
            {
                realDuration = 0;
            }

            TimerManager.Instance.AddTimer(ReturnSuccess, realDuration);
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
