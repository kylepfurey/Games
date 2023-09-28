using GameFramework.Utils;
using Octree;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// Updates the active AI Sights.
    /// </summary>
    [DefaultExecutionOrder(-49)]
    public class AISightManager : SingletonComponent<AISightManager>
    {
        [Tooltip("The amount of time in seconds before the manager performs an update on all subscribed AI sights.")]
        [SerializeField]
        private float scanInterval = 0.2f;

        [Tooltip("The maximum number of stimuli each AI Sight can percept in each frame.")]
        [SerializeField]
        private int maxStimuli = 100;

        // Active AI sights in the scene.
        private HashSet<AISight> _activeAISights;
        public int ActiveAISightCount => _activeAISights == null ? 0 : _activeAISights.Count;

        /// <summary>
        /// List of recently added but not yet active AI sights.
        /// </summary>
        private HashSet<AISight> _addedAISights;

        /// <summary>
        /// List of active AI sights to be removed.
        /// </summary>
        private HashSet<AISight> _removedAISights;

        // Active AI sight stimuli in the scene.
        private HashSet<IAISightStimulus> _activeStimuli;
        public int ActiveStimulusCount => _activeStimuli == null ? 0 : _activeStimuli.Count;

        /// <summary>
        /// List of recently added but not yet active stimuli.
        /// </summary>
        private HashSet<IAISightStimulus> _addedStimuli;

        /// <summary>
        /// List of active stimuli to be removed.
        /// </summary>
        private HashSet<IAISightStimulus> _removedStimuli;

        // List of all stimuli in the previous frame.
        // Used for clearing the stimuli octree in the new frame.
        private List<IAISightStimulus> _stimuliInPreviousFrame;

        // Octree for keeping track of the positions of the stimuli in the scene.
        private PointOctree<IAISightStimulus> _stimuliOctree;

        // Used for recording stimuli seen by each AI sight.
        private Dictionary<AISight, List<IAISightStimulus>> _neighbors;

        // Used for storing hit results of a raycast.
        RaycastHit[] hitResults;

        protected override void OnSingletonAwake()
        {
            _activeAISights = new HashSet<AISight>();
            _addedAISights = new HashSet<AISight>();
            _removedAISights = new HashSet<AISight>();

            _activeStimuli = new HashSet<IAISightStimulus>();
            _addedStimuli = new HashSet<IAISightStimulus>();
            _removedStimuli = new HashSet<IAISightStimulus>();
            _stimuliInPreviousFrame = new List<IAISightStimulus>();
            _stimuliOctree = new PointOctree<IAISightStimulus>(1000000f, Vector3.zero, 1);

            _neighbors = new Dictionary<AISight, List<IAISightStimulus>>();

            hitResults = new RaycastHit[maxStimuli];
        }

        private void Update()
        {
            if (_addedAISights.Count > 0 || _removedAISights.Count > 0)
                UpdateActiveAISights();

            if (_addedStimuli.Count > 0 || _removedStimuli.Count > 0)
                UpdateActiveStimuli();

            UpdateStimuliOctree();
            UpdateNeighborStimuli();

            // Single thread.
            foreach (AISight aiSight in _activeAISights)
            {
                aiSight.SeenStimuli.Clear();

                Vector3 eyePosition = aiSight.EyePosition;

                foreach (IAISightStimulus stimulus in _neighbors[aiSight])
                {
                    Vector3 targetPosition = stimulus.GetFocusPosition();
                    Vector3 displacementToTarget = targetPosition - eyePosition;
                    Vector3 directionToTarget = displacementToTarget.normalized;

                    // Check if the sight is within view cone.
                    if (Vector3.Dot(directionToTarget, aiSight.transform.forward) < aiSight.MinimumDotProduct)
                    {
                        Debug.DrawLine(eyePosition, targetPosition, Color.yellow);
                        continue;
                    }

                    // Check if the sight is blocked by another object.
                    float distanceToTarget = displacementToTarget.magnitude;
                    int hitCount = 0;
                    RaycastHit closestHitResult = new RaycastHit();
                    PhysicsUtils.RaycastDetailed(eyePosition, directionToTarget, distanceToTarget, aiSight.OwningTransform, aiSight.LayerMask, QueryTriggerInteraction.Ignore, ref hitCount, ref hitResults, ref closestHitResult);
                    if (hitCount > 0 && !closestHitResult.transform.IsChildOf(stimulus.OwningTransform))
                    {
                        Debug.DrawLine(eyePosition, targetPosition, Color.red);
                        continue;
                    }

                    aiSight.SeenStimuli.Add(stimulus);
                    Debug.DrawLine(eyePosition, targetPosition, Color.green);
                }

                foreach (var stimulus in aiSight.SeenStimuli)
                    aiSight.onStimulusEvaluated?.Invoke(stimulus, true);

                aiSight.RemovedSeenStimuli.Clear();
                aiSight.RemovedSeenStimuli.AddRange(aiSight.PreviouslySeenStimuli);
                aiSight.RemovedSeenStimuli.ExceptWith(aiSight.SeenStimuli);

                foreach (var stimulus in aiSight.RemovedSeenStimuli)
                    aiSight.onStimulusEvaluated?.Invoke(stimulus, false);

                aiSight.PreviouslySeenStimuli.Clear();
                aiSight.PreviouslySeenStimuli.AddRange(aiSight.SeenStimuli);
            }
        }

        /// <summary>
        /// Subscribes an AI sight to the manager.
        /// </summary>
        public void SubscribeAISight(AISight aiSight)
        {
            if (!aiSight)
                return;

            _addedAISights.Add(aiSight);
            _removedAISights.Remove(aiSight);
        }

        /// <summary>
        /// Unsubscribes an AI sight from the manager.
        /// </summary>
        public void UnsubscribeAISight(AISight aiSight)
        {
            _removedAISights.Add(aiSight);
            _addedAISights.Remove(aiSight);
        }

        private void UpdateActiveAISights()
        {
            foreach (AISight aiSight in _addedAISights)
            {
                if (!aiSight)
                    continue;

                _activeAISights.Add(aiSight);

                if (!_neighbors.ContainsKey(aiSight))
                    _neighbors[aiSight] = new List<IAISightStimulus>(maxStimuli);
            }

            foreach (AISight aiSight in _removedAISights)
            {
                _activeAISights.Remove(aiSight);
                _neighbors.Remove(aiSight);
            }

            _addedAISights.Clear();
            _removedAISights.Clear();
        }

        /// <summary>
        /// Subscribes a sight stimulus to the manager.
        /// </summary>
        public void SubscribeStimulus(IAISightStimulus stimulus)
        {
            if (stimulus == null || _activeStimuli.Contains(stimulus))
                return;

            _addedStimuli.Add(stimulus);
            _removedStimuli.Remove(stimulus);
        }

        /// <summary>
        /// Unsubscribes a sight stimulus from the manager.
        /// </summary>
        public void UnsubscribeStimulus(IAISightStimulus stimulus)
        {
            if (stimulus == null || !_activeStimuli.Contains(stimulus))
                return;

            _removedStimuli.Add(stimulus);
            _addedStimuli.Remove(stimulus);
        }

        private void UpdateActiveStimuli()
        {
            foreach (IAISightStimulus stimulus in _addedStimuli)
            {
                if (stimulus == null || _activeStimuli.Contains(stimulus))
                    return;

                _activeStimuli.Add(stimulus);
            }

            foreach (IAISightStimulus stimulus in _removedStimuli)
            {
                if (stimulus == null || !_activeStimuli.Contains(stimulus))
                    return;

                _activeStimuli.Remove(stimulus);
                _stimuliOctree.Remove(stimulus);
            }

            _addedStimuli.Clear();
            _removedStimuli.Clear();
        }

        private void UpdateStimuliOctree()
        {
            // Step 1: Clear all elements in the octree.
            //_stimuliOctree.GetAllNonAlloc(_stimuliInPreviousFrame);
            //for (int i = 0; i < _stimuliInPreviousFrame.Count; ++i)
            //    _stimuliOctree.Remove(_stimuliInPreviousFrame[i]);
            _stimuliOctree = new PointOctree<IAISightStimulus>(1000000f, Vector3.zero, 1);

            // Step 2: Add the latest stimuli into the octree.
            //for (int i = 0; i < _activeStimuli.Count; ++i)
            //    _stimuliOctree.Add(_activeStimuli[i], _activeStimuli[i].transform.position);
            foreach (IAISightStimulus aiSightStimulus in _activeStimuli)
                if (aiSightStimulus.IsUnityNull() == false)
                {
                    _stimuliOctree.Add(aiSightStimulus, aiSightStimulus.transform.position);
                }
        }

        /// <summary>
        /// Finds the stimuli around each AI Sight.
        /// </summary>
        private void UpdateNeighborStimuli()
        {
            var aiSights = _neighbors.Keys;

            // Remove the previous neighbors.
            foreach (AISight aiSight in aiSights)
            {
                _neighbors[aiSight].Clear();
            }

            // Add the new neighbors.
            foreach (AISight aiSight in aiSights)
            {
                _stimuliOctree.GetNearbyNonAlloc(aiSight.EyePosition, aiSight.ViewRadius, _neighbors[aiSight]);
                int neighborCount = _neighbors[aiSight].Count;
            }
        }
    }
}
