using UnityEngine;

namespace GameFramework.AI
{
    public class AIBehavior : MonoBehaviour
    {
        [SerializeField]
        private AIMovement aiMovement;

        [SerializeField]
        private AIPerception aiPerception;

        [SerializeField]
        private AIDecisionMaker aiDecisionMaker;
    }
}
