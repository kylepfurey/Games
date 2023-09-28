using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    public class AIPerceptionManager : SingletonComponent<AIPerceptionManager>
    {
        /// <summary>
        /// List of AI perceptions that are actively processed in the update loop.
        /// </summary>
        private List<AIPerception> activeAiPerceptions;
        public int ActiveAIPerceptionCount => activeAiPerceptions == null ? 0 : activeAiPerceptions.Count;
        
        /// <summary>
        /// List of AI perceptions that are recently registered to this manager.
        /// </summary>
        private List<AIPerception> registeredAiPerceptions;

        /// <summary>
        /// List of AI perceptions that are to be removed from the manager.
        /// </summary>
        private List<AIPerception> unregisteredAiPerceptions;

        protected override void OnSingletonAwake()
        {
            activeAiPerceptions = new List<AIPerception>(8);
            registeredAiPerceptions = new List<AIPerception>(8);
            unregisteredAiPerceptions = new List<AIPerception>(8);
        }

        private void Update()
        {
            if (registeredAiPerceptions.Count > 0)
            {
                AddActiveAIPerceptions();
            }

            if (unregisteredAiPerceptions.Count > 0)
            {
                RemoveActiveAIPerceptions();
            }

            for (int i = 0; i < activeAiPerceptions.Count; ++i)
            {
                //Debug.Log($"Update {activeAiPerceptions[i]}.");
            }
        }

        public void RegisterAIPerception(AIPerception aiPerception)
        {
            if (aiPerception == null || registeredAiPerceptions.Contains(aiPerception))
                return;

            registeredAiPerceptions.Add(aiPerception);
        }

        public void UnregisterAIPerception(AIPerception aiPerception)
        {
            if (unregisteredAiPerceptions.Contains(aiPerception))
                return;

            unregisteredAiPerceptions.Add(aiPerception);
        }

        private void AddActiveAIPerceptions()
        {
            for (int i = 0; i < registeredAiPerceptions.Count; ++i)
            {
                if (activeAiPerceptions.Contains(registeredAiPerceptions[i]))
                    continue;

                activeAiPerceptions.Add(registeredAiPerceptions[i]);
            }

            registeredAiPerceptions.Clear();
        }

        private void RemoveActiveAIPerceptions()
        {
            for (int i = 0; i < unregisteredAiPerceptions.Count; ++i)
            {
                activeAiPerceptions.Remove(unregisteredAiPerceptions[i]);
            }

            unregisteredAiPerceptions.Clear();
        }
    }
}
