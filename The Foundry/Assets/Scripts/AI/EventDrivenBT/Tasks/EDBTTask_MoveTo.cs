using UnityEngine;
using UnityEngine.AI;

namespace GameFramework.AI
{
    /// <summary>
    /// Move a NavMesh agent to a position.
    /// </summary>
    public class EDBTTask_MoveTo : EDBTTask
    {
        private NavMeshAgent agent;
        private string blackboardKeyPosition;
        private float acceptableDistance;

        // The position of the agent in the previous frame.
        private Vector3 previousAgentPosition;

        Vector3 previousDestination;

        bool firstUpdate = true;

        // How long has this agent been idle (not moving)?
        // Use for making this task fail if the agent is not willing to move.
        float idleTime;

        public EDBTTask_MoveTo(string name, NavMeshAgent agent, string blackboardKeyPosition, float acceptableDistance) : base(name)
        {
            this.agent = agent;
            this.blackboardKeyPosition = blackboardKeyPosition;
            this.acceptableDistance = acceptableDistance;
        }

        public EDBTTask_MoveTo(NavMeshAgent agent, string blackboardKeyPosition, float acceptableDistance) : base($"Move to: {blackboardKeyPosition}")
        {
            this.agent = agent;
            this.blackboardKeyPosition = blackboardKeyPosition;
            this.acceptableDistance = acceptableDistance;
        }

        public override void OnStarted()
        {
            if (!agent)
            {
                Debug.LogError($"NavMeshAgent is NULL.");
                CleanUpAndFinishExecution(false);
                return;
            }

            if (!Blackboard.ContainsKey(blackboardKeyPosition))
            {
                Debug.LogError($"{agent.name}: Cannot find the position from ${blackboardKeyPosition}.");
                CleanUpAndFinishExecution(false);
                return;
            }

            firstUpdate = true;
            idleTime = 0.0f;
            TimerManager.Instance.AddTimer(OnUpdate, 0, -1);
        }

        public override void OnInterrupted()
        {
            CleanUpAndFinishExecution(false);
        }

        public void OnUpdate()
        {
            Vector3 destination;
            if (!Blackboard.TryGetPosition(blackboardKeyPosition, out destination))
            {
                Debug.LogError($"{agent.name}: Cannot find the position from ${blackboardKeyPosition}.");
                CleanUpAndFinishExecution(false);
                return;
            }

            float distanceToDestination = Vector3.Distance(agent.transform.position, destination);
            if (distanceToDestination <= acceptableDistance)
            {
                //Debug.Log($"{agent.name}: Target reached.");
                CleanUpAndFinishExecution(true);
                return;
            }

            if (firstUpdate || destination != previousDestination)
            {
                if (firstUpdate)
                    firstUpdate = false;

                if (!agent.SetDestination(destination))
                {
                    //Debug.LogError($"{agent.name}: Cannot reach to the target.");
                    CleanUpAndFinishExecution(false);
                    return;
                }

                previousDestination = destination;
            }

            if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                //Debug.LogError($"{agent.name}: Path is no longer valid, cannot reach to the target.");
                CleanUpAndFinishExecution(false);
                return;
            }

            // Debug.Log($"{agent.name}: Moving to position...");

            // If the agent is idle for a long time, cancel movement.
            if (Vector3.Distance(agent.transform.position, previousAgentPosition) <= 0.001f)
            {
                idleTime += Time.deltaTime;
                if (idleTime > 10.0f)
                {
                    Debug.LogError($"{agent.name}: Agent is idle for a long time. Canceling movement...");
                    CleanUpAndFinishExecution(false);
                }
            }
            else
            {
                idleTime = 0.0f;
            }

            previousAgentPosition = agent.transform.position;
        }

        private void CleanUpAndFinishExecution(bool success)
        {
            TimerManager.Instance.RemoveTimer(OnUpdate);

            if (agent.isOnNavMesh)
            {
                agent.SetDestination(agent.transform.position);
            }

            FinishExecution(success);
        }

        public override void OnBehaviorTreeDestroyed()
        {
            TimerManager.Instance.RemoveTimer(OnUpdate);

            base.OnBehaviorTreeDestroyed();
        }
    }
}
