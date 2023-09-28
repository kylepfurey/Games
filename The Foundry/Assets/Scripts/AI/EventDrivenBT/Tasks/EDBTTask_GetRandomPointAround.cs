using UnityEngine;
using UnityEngine.AI;
using GameFramework.Math;

namespace GameFramework.AI
{
    public class EDBTTask_GetRandomPointAround : EDBTTask
    {
        private Transform centerTransform;
        private float radius;
        private string blackboardKeyPoint;

        public EDBTTask_GetRandomPointAround(string name, Transform centerTransform, float radius, string blackboardKey) : base(name)
        {
            this.centerTransform = centerTransform;
            this.radius = radius;
            blackboardKeyPoint = blackboardKey;
        }

        public override void OnStarted()
        {
            if (!Blackboard.ContainsKey(blackboardKeyPoint))
            {
                FinishExecution(false);
                return;
            }

            Vector3 randomPoint = MathfExtension.GetRandomPointOnCircle(centerTransform.transform.position, radius, Vector3.up);

            // Need to sample the point to make sure that the agent stands on navmesh.
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
            {
                randomPoint = hit.position;
            }

            if (!Blackboard.TrySetValue(blackboardKeyPoint, randomPoint))
            {
                FinishExecution(false);
                return;
            }

            FinishExecution(true);
        }
    }
}
