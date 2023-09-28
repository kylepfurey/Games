using Octree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    public class AIPerceptionStimulusManager : SingletonComponent<AIPerceptionStimulusManager>
    {
        /// <summary>
        /// List of AI perceptions that are actively processed in the update loop.
        /// </summary>
        private List<AIPerceptionStimulus> activeStimuli;
        public int ActiveStimulusCount => activeStimuli == null ? 0 : activeStimuli.Count;

        /// <summary>
        /// List of AI perceptions that are recently registered to this manager.
        /// </summary>
        private List<AIPerceptionStimulus> registeredStimuli;

        /// <summary>
        /// List of AI perceptions that are to be removed from the manager.
        /// </summary>
        private List<AIPerceptionStimulus> unregisteredStimuli;

        /// <summary>
        /// Octree for managing the positions of the stimuli.
        /// </summary>
        private PointOctree<AIPerceptionStimulus> activeStimulusOctree;

        private List<AIPerceptionStimulus> perceptionsInPreviousUpdate;

        protected override void OnSingletonAwake()
        {
            activeStimuli = new List<AIPerceptionStimulus>(8);
            registeredStimuli = new List<AIPerceptionStimulus>(8);
            unregisteredStimuli = new List<AIPerceptionStimulus>(8);

            activeStimulusOctree = new PointOctree<AIPerceptionStimulus>(10000, Vector3.zero, 1.2f);
            perceptionsInPreviousUpdate = new List<AIPerceptionStimulus>(8);
        }

        private void Update()
        {
            if (registeredStimuli.Count > 0)
            {
                AddActiveAIPerceptions();
            }

            if (unregisteredStimuli.Count > 0)
            {
                RemoveActiveAIPerceptions();
            }

            UpdateStimulusOctree();
        }

        public void RegisterStimulus(AIPerceptionStimulus aiPerception)
        {
            if (aiPerception == null || registeredStimuli.Contains(aiPerception))
                return;

            registeredStimuli.Add(aiPerception);
        }

        public void UnregisterStimulus(AIPerceptionStimulus aiPerception)
        {
            if (unregisteredStimuli.Contains(aiPerception))
                return;

            unregisteredStimuli.Add(aiPerception);
        }

        /// <summary>
        /// Returns stimuli in a sphere.
        /// </summary>
        public void GetStimuli(Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction, ref ICollection<AIPerceptionStimulus> outputList)
        {
            // TODO: Implement this function.
        }

        private void AddActiveAIPerceptions()
        {
            for (int i = 0; i < registeredStimuli.Count; ++i)
            {
                if (activeStimuli.Contains(registeredStimuli[i]))
                    continue;

                activeStimuli.Add(registeredStimuli[i]);
            }

            registeredStimuli.Clear();
        }

        private void RemoveActiveAIPerceptions()
        {
            for (int i = 0; i < unregisteredStimuli.Count; ++i)
            {
                activeStimuli.Remove(unregisteredStimuli[i]);
                activeStimulusOctree.Remove(unregisteredStimuli[i]);
            }

            unregisteredStimuli.Clear();
        }

        private void UpdateStimulusOctree()
        {
            activeStimulusOctree.GetAllNonAlloc(ref perceptionsInPreviousUpdate);
            
            for (int i = 0; i < perceptionsInPreviousUpdate.Count; ++i)
                activeStimulusOctree.Remove(perceptionsInPreviousUpdate[i]);

            for (int i = 0; i < activeStimuli.Count; ++i)
                activeStimulusOctree.Add(activeStimuli[i], activeStimuli[i].transform.position);
        }
    }
}
