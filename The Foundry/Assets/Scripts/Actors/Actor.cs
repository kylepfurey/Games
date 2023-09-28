using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPSGame
{
    public class Actor : MonoBehaviour
    {
        /// <summary>
        /// Invoked when this actor kills a target.
        /// </summary>
        public delegate void OnKillTargetDelegate(Actor target);
        public OnKillTargetDelegate onKillTarget;

        [SerializeField]
        private Transform targetLookAtTransform;
        public Transform TargetLookAtTransform => targetLookAtTransform;

        [SerializeField]
        private int affilitation = 0;
        public int Affiliation => affilitation;

        private void OnValidate()
        {
            if (!targetLookAtTransform)
            {
                targetLookAtTransform = transform;
            }
        }

        private void Start()
        {
            ActorManager.Instance.RegisterActor(this);
        }

        private void OnDestroy()
        {
            if (SceneManager.GetActiveScene().isLoaded)
            {
                ActorManager.Instance.UnregisterActor(this);
            }
        }
    }
}