using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;
using GameFramework.AI;

namespace Game
{
    /// <summary>
    /// Enable or disable flying for a steering agent.
    /// </summary>
    public class EDBTTask_SetCanFly : EDBTTask
    {
        private MovementAIRigidbody aiRigidbody;
        private bool canFly;

        public EDBTTask_SetCanFly(string name, MovementAIRigidbody aiRigidbody, bool canFly) : base(name)
        {
            this.aiRigidbody = aiRigidbody;
            this.canFly = canFly;
        }

        public override void OnStarted()
        {
            aiRigidbody.canFly = canFly;
            FinishExecution(true);
        }
    }
}
