using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Octree;
using Unity.VisualScripting;

namespace GameFramework.AI
{
    /// <summary>
    /// Used for keeping track of active characters in the game.
    /// </summary>
    public class AgentManager : SingletonComponent<AgentManager>
    {
        /// <summary>
        /// List of active agents during runtime.
        /// </summary>
        private List<IAgent> _agents = null;

        /// <summary>
        /// This octree is used for quickly finding the neighbors.
        /// </summary>
        private PointOctree<IAgent> _positionOctree;

        protected override void OnSingletonAwake()
        {
            InitializeManager();
        }

        private void FixedUpdate()
        {
            UpdateOctree();
        }

        private void InitializeManager()
        {
            _agents = new List<IAgent>();

            _positionOctree = new PointOctree<IAgent>(1000000f, Vector3.zero, 1);

            Debug.Log("Agent manager initialized.");
        }

        public List<IAgent> GetRegisteredAgents()
        {
            return _agents;
        }

        /// <summary>
        /// Adds an agent to the manager. Returns false if the agent is null or it is already added.
        /// </summary>
        public bool RegisterAgent(IAgent steeringAgent)
        {
            if (steeringAgent != null && !_agents.Contains(steeringAgent))
            {
                _agents.Add(steeringAgent);

                //Debug.Log($"Registered agent: {steeringAgent.GetName()}", steeringAgent as Object);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an agent from the manager. Returns false if the agent is null or it is not in the list.
        /// </summary>
        public bool UnregisterAgent(IAgent steeringAgent)
        {
            if (steeringAgent != null && _agents.Contains(steeringAgent))
            {
                _agents.Remove(steeringAgent);
                _positionOctree.Remove(steeringAgent);

                //Debug.Log($"Unregistered agent: {steeringAgent.GetName()}", steeringAgent as Object);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the octree as the agents move to new positions.
        /// </summary>
        private void UpdateOctree()
        {
            // Remove the elements in the previous frame.
            ICollection<IAgent> agentsInOctree = _positionOctree.GetAll();
            foreach (IAgent agent in agentsInOctree)
            {
                _positionOctree.Remove(agent);
            }

            //_positionOctree = new PointOctree<SteeringAgent>(1000000f, Vector3.zero, 1);

            // Add new elements into the octree.
            for (int i = 0; i < _agents.Count; ++i)
            {
                _positionOctree.Add(_agents[i], _agents[i].Position);
            }
        }

        /// <summary>
        /// Returns the neighbors of an agent within distance.
        /// </summary>
        public IAgent[] GetNeighbors(IAgent agent, float distance)
        {
            return _positionOctree.GetNearby(agent.Position, distance, agent);
        }

        public IAgent[] GetAgentsInSphere(Vector3 position, float radius)
        {
            return _positionOctree.GetNearby(position, radius);
        }

        public void GetNeighborsNonAlloc(IAgent agent, float radius, List<IAgent> agents)
        {
            _positionOctree.GetNearbyNonAlloc(agent.Position, radius, agents, agent);
        }
    }
}
