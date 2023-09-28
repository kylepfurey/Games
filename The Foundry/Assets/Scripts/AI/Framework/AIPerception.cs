using UnityEngine;

namespace GameFramework.AI
{
    public class AIPerception : MonoBehaviour
    {
        [SerializeField]
        private AISight aiSight;

        [SerializeField]
        private AIHearing aiHearing;

        [SerializeField]
        private AITouch aiTouch;

        [SerializeField]
        private AIDamage aiDamage;

        private void OnEnable()
        {
            AIPerceptionManager.EnsureCreation();
            AIPerceptionManager.Instance.RegisterAIPerception(this);
        }

        private void OnDisable()
        {
            AIPerceptionManager.Instance?.UnregisterAIPerception(this);
        }
    }
}
