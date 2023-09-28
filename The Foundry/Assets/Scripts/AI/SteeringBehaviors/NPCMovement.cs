using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

namespace Game
{
    public abstract class NPCMovement : MonoBehaviour
    {
        [Tooltip("Used for getting access to rigid body and basic steering behaviors.")]
        [SerializeField]
        protected SteeringBasics steeringBasics;

        [Tooltip("Used for setting the move destination of the agent.")]
        [SerializeField]
        protected SteeringDestination steeringDestination;
        public SteeringDestination SteeringDestination => steeringDestination;

        protected Transform targetTransform;

        protected virtual void OnValidate()
        {
            if (!steeringBasics)
                steeringBasics = GetComponent<SteeringBasics>();
            if (!steeringDestination)
                steeringDestination = GetComponent<SteeringDestination>();
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            targetTransform = steeringDestination.DestinationTransform;
        }

        protected virtual void FixedUpdate()
        {
            UpdateMovement(Time.fixedDeltaTime);
        }

        protected virtual void OnDestroy()
        {

        }

        protected abstract void UpdateMovement(float deltaTime);
    }
}
