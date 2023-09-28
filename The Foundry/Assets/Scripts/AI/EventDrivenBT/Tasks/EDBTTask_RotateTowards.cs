using UnityEngine;
using UnityEngine.AI;

namespace GameFramework.AI
{
    /// <summary>
    /// Rotate a game object towards a target.
    /// </summary>
    public class EDBTTask_RotateTowards : EDBTTask
    {
        private Transform transform;
        private string blackboardKeyTarget;
        private float acceptableDotProduct;
        private float rotationSpeed;


        public EDBTTask_RotateTowards(string name, Transform transform, string blackboardKeyTarget, float acceptableDotProduct = 0.995f, float rotationSpeed = 50.0f) : base(name)
        {
            this.transform = transform;
            this.blackboardKeyTarget = blackboardKeyTarget;
            this.acceptableDotProduct = acceptableDotProduct;
            this.rotationSpeed = rotationSpeed;
        }

        public override void OnStarted()
        {
            if (!transform)
                CleanUpAndFinishExecution(false);

            TimerManager.Instance.AddTimer(OnUpdate, 0, -1);
        }

        public override void OnInterrupted()
        {
            CleanUpAndFinishExecution(false);
        }

        public void OnUpdate()
        {
            Vector3 targetPosition;
            if (!Blackboard.TryGetPosition(blackboardKeyTarget, out targetPosition))
            {
                Debug.LogError($"{Name}: Cannot find the target position.");
                CleanUpAndFinishExecution(false);
                return;
            }

            Vector3 forwardDirection = transform.forward;
            Vector3 forwardDirection2D = new Vector3(forwardDirection.x, 0.0f, forwardDirection.z).normalized;

            Vector3 targetDirection = targetPosition - transform.position;
            Vector3 targetDirection2D = new Vector3(targetDirection.x, 0.0f, targetDirection.z).normalized;

            float dotProduct = Vector3.Dot(forwardDirection2D, targetDirection2D);

            if (dotProduct >= acceptableDotProduct)
                CleanUpAndFinishExecution(true);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection2D), Time.deltaTime * rotationSpeed);
        }

        private void CleanUpAndFinishExecution(bool success)
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
