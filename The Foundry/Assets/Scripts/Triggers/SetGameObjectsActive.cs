using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame
{
    public class SetGameObjectsActive : MonoBehaviour
    {
        [SerializeField]
        private Health healthComponent;

        [SerializeField]
        private float triggerHealth = 50.0f;

        [SerializeField]
        private List<GameObject> gameObjectsToHide = new List<GameObject>();

        [SerializeField]
        private List<GameObject> gameObjectsToDestroy = new List<GameObject>();

        private void OnValidate()
        {
            if (!healthComponent)
            {
                healthComponent = GetComponent<Health>();
            }
        }

        private void Start()
        {
            foreach (GameObject gameObject in gameObjectsToHide)
            {
                gameObject.SetActive(false);
            }

            if (healthComponent)
            {
                healthComponent.onHealthChanged += OnHealthChanged;
            }
        }

        private void OnDestroy()
        {
            if (healthComponent)
            {
                healthComponent.onHealthChanged -= OnHealthChanged;
            }
        }

        public void SetObjectsActive()
        {
            foreach (GameObject gameObject in gameObjectsToHide)
            {
                gameObject.SetActive(true);
            }

            foreach (GameObject gameObject in gameObjectsToDestroy)
            {
                Destroy(gameObject);
            }

            Destroy(this);
        }

        private void OnHealthChanged(float newHealth)
        {
            if (newHealth <= triggerHealth)
            {
                SetObjectsActive();
            }
        }
    }
}
