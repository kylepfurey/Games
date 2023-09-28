using UnityEngine;
using GameFramework.AI;

namespace Game
{
    /// <summary>
    /// Set the value of a blackboard key.
    /// </summary>
    /// <typeparam name="T">The type of the blackboard key.</typeparam>
    public class EDBTTask_SetCurrentWaypointOnPath : EDBTTask
    {
        private AIPath aiPath;
        private string keyCurrentWaypointIndex;
        private string keyMoveToPosition;
        private string keyLookAtPoint;
        private string keyWaitDuration;

        public EDBTTask_SetCurrentWaypointOnPath(string name, AIPath aiPath, string keyCurrentWaypointIndex, string keyMoveToPosition, string keyLookAtPoint, string keyWaitDuration): base(name)
        {
            this.aiPath = aiPath;
            this.keyCurrentWaypointIndex = keyCurrentWaypointIndex;
            this.keyMoveToPosition = keyMoveToPosition;
            this.keyLookAtPoint = keyLookAtPoint;
            this.keyWaitDuration = keyWaitDuration;
        }

        public override void OnStarted()
        {
            if (!aiPath)
            {
                Debug.LogError($"{Name}: The AI path is NULL.");
                FinishExecution(false);
                return;
            }

            int currentWaypointIndex;
            if (!Blackboard.TryGetValue(keyCurrentWaypointIndex, out currentWaypointIndex))
            {
                Debug.LogError($"{Name}: Cannot get the value from the key \"{keyCurrentWaypointIndex}\".");
                FinishExecution(false);
                return;
            }

            Transform waypointTransform = aiPath.WayPoints[currentWaypointIndex].transform;
            if (!Blackboard.TrySetValue<Vector3>(keyMoveToPosition, waypointTransform.position))
            {
                Debug.LogError($"{Name}: Cannot set the value for the key \"{keyMoveToPosition}\".");
                FinishExecution(false);
                return;
            }

            if (!Blackboard.TrySetValue<Vector3>(keyLookAtPoint, waypointTransform.position + waypointTransform.forward * 20000.0f))
            {
                Debug.LogError($"{Name}: Cannot set the value for the key \"{keyLookAtPoint}\".");
                FinishExecution(false);
                return;
            }

            if (!Blackboard.TrySetValue<float>(keyWaitDuration, aiPath.WayPoints[currentWaypointIndex].waitTime))
            {
                Debug.LogError($"{Name}: Cannot set the value for the key \"{keyWaitDuration}\".");
                FinishExecution(false);
                return;
            }

            FinishExecution(true);
        }
    }
}
