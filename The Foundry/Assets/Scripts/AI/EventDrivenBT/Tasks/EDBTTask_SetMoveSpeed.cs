using UnityEngine;
using UnityEngine.AI;

namespace GameFramework.AI
{
    /// <summary>
    /// Move a NavMesh agent to a position.
    /// </summary>
    public class EDBTTask_SetMoveSpeed : EDBTTask
    {
        private NavMeshAgent agent;
        private float moveSpeed;

        public EDBTTask_SetMoveSpeed(string name, NavMeshAgent agent, float moveSpeed) : base(name)
        {
            this.agent = agent;
            this.moveSpeed = moveSpeed;
        }

        public override void OnStarted()
        {
            if (!agent)
            {
                Debug.LogError($"NavMeshAgent is NULL.");
                FinishExecution(false);
                return;
            }

            agent.speed = moveSpeed;
            FinishExecution(true);
        }
    }
}
