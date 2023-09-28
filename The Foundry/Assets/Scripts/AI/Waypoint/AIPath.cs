using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AIPath : MonoBehaviour
    {
        [System.Serializable]
        public struct WayPoint
        {
            public Transform transform;
            public float waitTime;
        }

        [SerializeField] List<WayPoint> waypoints;
        public List<WayPoint> WayPoints => waypoints;

        private void OnDrawGizmos()
        {
            DrawWaypoints();
        }

        private void DrawWaypoints()
        {
            // Draw the waypoints.
            Gizmos.color = Color.red;

            for (int i = 0; i < waypoints.Count; i++)
            {
                if (!waypoints[i].transform)
                {
                    Debug.LogWarning($"{name}: Cannot draw the path. One of the waypoints is null.");
                    return;
                }

                Gizmos.DrawSphere(waypoints[i].transform.position, 0.3f);
            }

            // Draw the forward directions of the waypoints.
            Gizmos.color = Color.blue;

            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i].transform.position + waypoints[i].transform.forward);
            }

            // Draw the edges between the waypoints.
            Gizmos.color = Color.green;

            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                Transform waypoint1 = waypoints[i].transform;
                Transform waypoint2 = waypoints[i + 1].transform;

                Gizmos.DrawLine(waypoint1.position, waypoint2.position);
            }
        }
    }
}
