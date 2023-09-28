using GameFramework.AI;
using UnityEngine;
using UnityEngine.AI;

namespace GameFramework
{
    /// <summary>
    /// Move a NavMesh agent to a position.
    /// </summary>
    public class EDBTTask_Flee : EDBTTask
    {
        private NavMeshAgent agent;
        private string blackboardKeyTarget;
        private float fleeDistance;

        Vector3 previousTargetPosition;

        bool firstUpdate = true;

        public EDBTTask_Flee(string name, NavMeshAgent agent, string blackboardKeyPosition, float fleeDistance) : base(name)
        {
            this.agent = agent;
            this.blackboardKeyTarget = blackboardKeyPosition;
            this.fleeDistance = fleeDistance;
        }

        public override void OnStarted()
        {
            if (!agent)
            {
                Debug.LogError($"NavMeshAgent is NULL.");
                CleanUpAndFinishExecution(false);
                return;
            }

            if (!Blackboard.ContainsKey(blackboardKeyTarget))
            {
                Debug.LogError($"{agent.name}: Cannot find the position from ${blackboardKeyTarget}.");
                CleanUpAndFinishExecution(false);
                return;
            }

            firstUpdate = true;
            TimerManager.Instance.AddTimer(OnUpdate, 0, -1);
        }

        public override void OnInterrupted()
        {
            CleanUpAndFinishExecution(false);
        }

        public void OnUpdate()
        {
            //Debug.Log($"{agent.name}: Predator detected!");

            Vector3 targetPosition;
            if (!Blackboard.TryGetPosition(blackboardKeyTarget, out targetPosition))
            {
                Debug.LogError($"{agent.name}: Cannot find the position from ${blackboardKeyTarget}.");
                CleanUpAndFinishExecution(false);
                return;
            }

            float distanceToTarget = Vector3.Distance(agent.transform.position, targetPosition);
            if (distanceToTarget >= fleeDistance)
            {
                //Debug.Log($"{agent.name}: Fled from target.");
                CleanUpAndFinishExecution(true);
                return;
            }

            //if (firstUpdate || targetPosition != previousTargetPosition)
            {
                if (firstUpdate)
                    firstUpdate = false;

                Vector3 fleeDirection = (agent.transform.position - targetPosition).normalized;

                Vector3 fleePosition = agent.transform.position + fleeDirection * 2.0f;

                NavMeshHit hit;
                if (!NavMesh.SamplePosition(fleePosition, out hit, fleeDistance, NavMesh.AllAreas))
                {
                    Debug.LogError($"{agent.name}: Cannot place the flee position on nav mesh.");
                    CleanUpAndFinishExecution(false);
                    return;
                }

                fleePosition = hit.position;


                if (!agent.SetDestination(fleePosition))
                {
                    Debug.LogError($"{agent.name}: Cannot set the flee position.");
                    CleanUpAndFinishExecution(false);
                    return;
                }

                previousTargetPosition = targetPosition;
            }

            if (agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                Debug.LogError($"{agent.name}: Path is no longer valid, cannot flee from the target.");
                CleanUpAndFinishExecution(false);
                return;
            }

            //Debug.Log($"{agent.name}: Fleeing...");
        }

        private void CleanUpAndFinishExecution(bool success)
        {
            TimerManager.Instance.RemoveTimer(OnUpdate);
            agent.SetDestination(agent.transform.position);
            FinishExecution(success);
        }

        public override void OnBehaviorTreeDestroyed()
        {
            TimerManager.Instance.RemoveTimer(OnUpdate);
            base.OnBehaviorTreeDestroyed();
        }
    }
}
