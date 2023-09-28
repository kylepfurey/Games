using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.AI;

namespace Game
{
    public class EDBTTask_GetRandomPointInBox : EDBTTask
    {
        /// <summary>
        /// The key that stores the center of the box.
        /// </summary>
        private string keyCenter;

        /// <summary>
        /// The key that stores the size of the box.
        /// </summary>
        private string keySize;

        /// <summary>
        /// The key that stores the random position in the box.
        /// </summary>
        private string keyRandomPosition;

        Vector3 center;
        Vector3 size;

        public EDBTTask_GetRandomPointInBox(string name, string keyCenter, string keySize, string keyRandomPosition) : base(name)
        {
            this.keyCenter = keyCenter;
            this.keySize = keySize;
            this.keyRandomPosition = keyRandomPosition;
        }

        public override void OnStarted()
        {
            if (!Blackboard.ContainsKey(keyCenter))
            {
                Debug.LogError($"{Name}: The blackboard does not contain key \"{keyCenter}\"");
                FinishExecution(false);
                return;
            }

            if (!Blackboard.TryGetPosition(keyCenter, out center))
            {
                Debug.LogError($"{Name}: Cannot get the center from \"{keyCenter}\"");
                FinishExecution(false);
                return;
            }

            if (!Blackboard.ContainsKey(keySize))
            {
                Debug.LogError($"{Name}: The blackboard does not contain key \"{keySize}\"");
                FinishExecution(false);
                return;
            }

            if (!Blackboard.TryGetPosition(keyCenter, out size))
            {
                Debug.LogError($"{Name}: Cannot get the size from \"{keyCenter}\"");
                FinishExecution(false);
                return;
            }

            float randomPositionX = Random.Range(center.x - size.x / 2.0f, center.x + size.x / 2.0f);
            float randomPositionY = Random.Range(center.y - size.y / 2.0f, center.y + size.y / 2.0f);
            float randomPositionZ = Random.Range(center.z - size.z / 2.0f, center.z + size.z / 2.0f);
            Vector3 randomPosition = new Vector3(randomPositionX, randomPositionY, randomPositionZ);

            if (!Blackboard.TrySetValue<Vector3>(keyRandomPosition, randomPosition))
            {
                Debug.LogError($"{Name}: Cannot set the value of \"{keyRandomPosition}\" in the blackboard");
                FinishExecution(false);
                return;
            }

            FinishExecution(true);
        }
    }
}
