using Eflatun.SceneReference;
using FPSGame;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public class PlayerStart : MonoBehaviour
    {
        [SerializeField]
        private SceneReference playerScene;

        private void Start()
        {
            // Check if a player character has already been loaded into the game scene.
            PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
            if (playerMovement)
            {
                Destroy(gameObject);
                return;
            }

            // Load the player scene.
            if (!SceneManager.GetSceneByName(playerScene.Name).isLoaded)
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(playerScene.Name, LoadSceneMode.Additive);
                asyncOperation.completed += OnPlayerSceneLoaded;
            }
        }

        private void OnPlayerSceneLoaded(AsyncOperation operation)
        {
            PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
            if (playerMovement)
            {
                playerMovement.Motor.SetPosition(transform.position);
                playerMovement.Motor.SetRotation(transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
