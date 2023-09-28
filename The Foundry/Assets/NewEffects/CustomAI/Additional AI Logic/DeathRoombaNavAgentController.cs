using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRoombaNavAgentController : NavAgentController
{
    public override void MoveToTarget()
    {
        //base.MoveToTarget();
        MonitorTarget();
        if (targetLOS && targetInRoom)
        {
            //If we have clear line of sight & target is in our room, then we can move straight towards them!
            movementModule.SetTargetPosition(targetPosition);
            keepTheFirst = true;

            if (m_Agent != null)
            {
                if (m_Agent.currentPath != null)
                {
                    if (m_Agent.currentPath.Count > 0)
                    {
                        m_Agent.currentPath.Clear();
                    }
                    pathRequestTimer = 0.25f;
                }
            }
        }
        else
        {
            //Watch the timer to prevent wasteful pathfinding calls
            if (pathRequestTimer < Time.time)
            {
                pathRequestTimer = Time.time + pathRequestDelay;
                if (targetPosition != lastPos) //Block pathfinding requests if navigation target position hasn't changed
                {
                    lastPos = targetPosition;

                    if (m_Agent != null)
                    {
                        m_Agent.SetDestination(targetPosition);
                        if (keepTheFirst == true)
                        {
                            keepTheFirst = false;
                        }
                        else //I believe this check allows the agent to continue their momentum when a new path is generated
                        {
                            if (m_Agent.currentPath != null)
                            {
                                if (m_Agent.currentPath.Count > 0)
                                {
                                    m_Agent.currentPath.RemoveAt(0);

                                    m_Agent.currentNode = m_Agent.currentPath[0];
                                }
                            }
                        }
                    }
                }
            }

            //If we don't have direct LOS and have a path, then we should try to follow it.
            if (m_Agent != null)
            {
                if (m_Agent.currentPath != null)
                {
                    if (m_Agent.currentPath.Count > 0)
                    {
                        movementModule.SetTargetPosition(m_Agent.currentPath[0].transform.position);
                    }
                }
            }
        }
    }
}
