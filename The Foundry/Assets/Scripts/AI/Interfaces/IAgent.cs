using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Interface for all types of agents in 3D space.
    /// </summary>
    public interface IAgent
    {
        public string GetName();

        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public float Radius { get; }

        public float Height { get; }

        public Vector3 Velocity { get; }

        /// <summary>
        /// Velocity retrieved from rigidbody.
        /// </summary>
        public Vector3 RealVelocity { get; }
    }
}
