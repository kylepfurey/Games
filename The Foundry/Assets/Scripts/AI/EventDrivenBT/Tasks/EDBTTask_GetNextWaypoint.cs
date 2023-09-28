using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.AI;

/// <summary>
/// Get the next waypoint in the AI path.
/// </summary>
/// 

namespace Game
{
    public class EDBTTask_GetNextWaypoint : EDBTTask
    {
        private AIPath aiPath;
        private string keyCurrentIndex;

        private int currentIndex;

        public EDBTTask_GetNextWaypoint(string name, AIPath aiPath, string keyCurrentIndex) : base(name)
        {
            this.aiPath = aiPath;
            this.keyCurrentIndex = keyCurrentIndex;
        }

        public override void OnStarted()
        {
            if (!aiPath)
            {
                Debug.LogError($"{Name}: The AI path is NULL.");
                FinishExecution(false);
                return;
            }

            if (!Blackboard.TryGetValue<int>(keyCurrentIndex, out currentIndex))
            {
                Debug.LogError($"{Name}: Cannot get the value from key \"{keyCurrentIndex}\".");
                FinishExecution(false);
                return;
            }

            currentIndex = (currentIndex + 1) % aiPath.WayPoints.Count;

            Blackboard.SetValue<int>(keyCurrentIndex, currentIndex);
            FinishExecution(true);
        }
    }
}
