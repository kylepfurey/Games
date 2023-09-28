using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;

namespace GameFramework.AI
{
    /// <summary>
    /// Interface for objects that can be seen by AI Sight.
    /// </summary>
    public interface IAISightStimulus
    {
        public Transform transform { get; }

        public GameObject gameObject { get; }

        /// <summary>
        /// The transform of the owner of this AI sight.
        /// </summary>
        public Transform OwningTransform { get; }

        // If true, this stimulant is ignored during a scan.
        public bool ShouldAgentsIgnoreThis { get; }

        // Get the position where the AI looks at.
        public Vector3 GetFocusPosition();
    }

    public class AISight : MonoBehaviour
    {
        public delegate void OnStimulantEvaluated(IAISightStimulus stimulus, bool canBeSeen);

        public OnStimulantEvaluated onStimulusEvaluated;

        [SerializeField]
        private Transform owningTransform;
        public Transform OwningTransform => owningTransform;

        // NOTE: The eye position should be inside all colliders of the gameobject, otherwise ScanStimulants() will not properly (I hate you, crappy physics system).
        [Tooltip("The eye position with respect to the game object's position. NOTE: The eye position should be inside all colliders of the game object, otherwise this component will not properly.")]
        [SerializeField]
        private Vector3 eyeOffset;
        public Vector3 EyePosition => transform.TransformPoint(eyeOffset);

        [SerializeField]
        private float viewRadius = 50.0f;
        public float ViewRadius => viewRadius;

        [Tooltip("If the stimulus is seen in the previous tick and is within this radius, it will be automatically seen in the current tick, regardless of whether it is behind the eye.")]
        [SerializeField]
        private float autoDetectRadius = 10.0f;
        public float AutoDetectRadius => autoDetectRadius;

        [SerializeField]
        private float fieldOfView = 120.0f;
        public float FieldOfView => fieldOfView;

        [Tooltip("Only objects in this layer can be seen by AI Sight.")]
        [SerializeField]
        private LayerMask layerMask = -5;
        public LayerMask LayerMask => layerMask;

        /// <summary>
        /// Stimuli that are seen in the current tick.
        /// </summary>
        private HashSet<IAISightStimulus> seenStimuli;
        public HashSet<IAISightStimulus> SeenStimuli => seenStimuli;

        /// <summary>
        /// Stimuli that are seen in the previous tick.
        /// </summary>
        private HashSet<IAISightStimulus> previouslySeenStimuli;
        public HashSet<IAISightStimulus> PreviouslySeenStimuli => previouslySeenStimuli;

        /// <summary>
        /// Stimuli that are seen in the previous tick but not in this tick.
        /// </summary>
        private HashSet<IAISightStimulus> removedSeenStimuli;
        public HashSet<IAISightStimulus> RemovedSeenStimuli => removedSeenStimuli;

        private float minimumDotProduct;
        public float MinimumDotProduct => minimumDotProduct;

        private void OnValidate()
        {
            if (!owningTransform)
                owningTransform = transform;
        }

        private void Awake()
        {
            seenStimuli = new HashSet<IAISightStimulus>();
            previouslySeenStimuli = new HashSet<IAISightStimulus>();
            removedSeenStimuli = new HashSet<IAISightStimulus>();

            minimumDotProduct = Mathf.Cos(fieldOfView * Mathf.Deg2Rad / 2f);
        }

        public void OnEnable()
        {
            AISightManager.EnsureCreation();
            AISightManager.Instance.SubscribeAISight(this);
        }

        public void OnDisable()
        {
            AISightManager.Instance?.UnsubscribeAISight(this);
        }

        // Visualize the eye position.
        private void OnDrawGizmosSelected()
        {
            // Draw the eye position.
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(EyePosition, 0.1f);

            // Draw the view sphere.
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(EyePosition, ViewRadius);

            // Draw the view cone.
            Gizmos.color = Color.blue;

            Vector3 leftLine = (Quaternion.Euler(0.0f, -fieldOfView / 2f, 0.0f) * transform.forward) * viewRadius;
            Vector3 rightLine = (Quaternion.Euler(0.0f, fieldOfView / 2f, 0.0f) * transform.forward) * viewRadius;

            Gizmos.DrawLine(EyePosition, EyePosition + leftLine);
            Gizmos.DrawLine(EyePosition, EyePosition + rightLine);
        }
    }
}
