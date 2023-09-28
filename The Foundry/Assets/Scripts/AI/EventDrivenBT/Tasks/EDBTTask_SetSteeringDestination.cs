using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.AI;

namespace Game
{
    public class EDBTTask_SetSteeringDestination : EDBTTask
    {
        SteeringDestination steeringDestination;
        string keyTarget;
        float acceptableDistance;

        bool isTargetDynamic = false;

        Vector3 staticTarget;

        Transform dynamicTarget;
        Vector3 offset;

        // The position of the agent in the previous frame.
        private Vector3 previousAgentPosition;

        // How long has this agent been idle (not moving)?
        // Use for making this task fail if the agent is not willing to move.
        float idleTime;

        public EDBTTask_SetSteeringDestination(string name, SteeringDestination steeringDestination, string keyTarget, float acceptableDistance = 1.0f) : base(name)
        {
            this.steeringDestination = steeringDestination;
            this.keyTarget = keyTarget;
            this.acceptableDistance = acceptableDistance;
        }

        public EDBTTask_SetSteeringDestination(string name, SteeringDestination steeringDestination, string keyTarget, Vector3 offset, float acceptableDistance = 1.0f) : base(name)
        {
            this.steeringDestination = steeringDestination;
            this.keyTarget = keyTarget;
            this.offset = offset;
            this.acceptableDistance = acceptableDistance;
        }

        public override void OnStarted()
        {
            //Debug.Log($"{Name}: Started.");

            // Check if the FlyToTarget component exists.
            if (!steeringDestination)
            {
                Debug.LogError($"FlyToTarget is NULL.");
                FinishExecution(false);
                return;
            }

            // Check if the blackboard contains the key.
            if (!Blackboard.ContainsKey(keyTarget))
            {
                Debug.LogError($"{steeringDestination.name}: Cannot find the position from ${keyTarget} since the key does not exist.");
                CleanUpAndFinishExecution(false);
                return;
            }

            // Check if the blackboard key is a static target or a dynamic target.
            if (Blackboard.TryGetTransform(keyTarget, out dynamicTarget))
            {
                isTargetDynamic = true;
                steeringDestination.SetDestination(dynamicTarget, offset);
            }
            else if (Blackboard.TryGetPosition(keyTarget, out staticTarget))
            {
                isTargetDynamic = false;
                steeringDestination.SetDestination(staticTarget, offset);
            }
            else
            {
                Debug.LogError($"{steeringDestination.name}: Cannot find the target from ${keyTarget}.");
                CleanUpAndFinishExecution(false);
                return;
            }

            idleTime = 0.0f;
            TimerManager.Instance.AddTimer(OnUpdate, 0.0f, -1);
        }

        public override void OnInterrupted()
        {
            CleanUpAndFinishExecution(false);
        }

        private void OnUpdate()
        {
            if (isTargetDynamic && !dynamicTarget)
            {
                CleanUpAndFinishExecution(false);
            }

            float distanceToTarget = Vector3.Distance(steeringDestination.transform.position, steeringDestination.DestinationTransform.position);
            //Debug.Log($"{Name}: Distance to target: {distanceToTarget}. Acceptable distance: {acceptableDistance}.");

            if (distanceToTarget <= acceptableDistance)
            {
                //Debug.Log($"{Name}: Target reached.");
                CleanUpAndFinishExecution(true);
            }

            // If the agent is idle for a long time, cancel movement.
            if (Vector3.Distance(steeringDestination.transform.position, previousAgentPosition) <= 0.1f)
            {
                idleTime += Time.deltaTime;
                if (idleTime > 10.0f)
                {
                    //Debug.LogError($"{Name}: Agent is idle for a long time. Canceling movement...");
                    CleanUpAndFinishExecution(false);
                }
            }
            else
            {
                idleTime = 0.0f;
            }

            previousAgentPosition = steeringDestination.transform.position;
        }

        private void CleanUpAndFinishExecution(bool success)
        {
            TimerManager.Instance?.RemoveTimer(OnUpdate);

            steeringDestination?.SetDestination(steeringDestination.transform.position, Vector3.zero);

            FinishExecution(success);
        }

        public override void OnBehaviorTreeDestroyed()
        {
            TimerManager.Instance?.RemoveTimer(OnUpdate);
            base.OnBehaviorTreeDestroyed();
        }
    }
}
