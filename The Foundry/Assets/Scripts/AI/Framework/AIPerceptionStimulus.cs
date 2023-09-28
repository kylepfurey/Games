using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.AI
{
    /// <summary>
    /// A stimulus is an object that can be perceived by an AIPerception object.
    /// </summary>
    public class AIPerceptionStimulus : MonoBehaviour
    {
        [Tooltip("If true, this stimulus is considered to be at the exact same position, and it is not updated in the octree.")]
        [SerializeField]
        private bool isStatic = false;

        private void OnEnable()
        {
            AIPerceptionStimulusManager.EnsureCreation();
            AIPerceptionStimulusManager.Instance.RegisterStimulus(this);
        }

        private void OnDisable()
        {
            AIPerceptionStimulusManager.Instance?.UnregisterStimulus(this);
        }
    }
}
