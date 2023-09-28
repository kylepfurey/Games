using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace FPSGame
{
    public class DisablePostProcessing : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            DisablePostProcessingComponents();
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            DisablePostProcessingComponents();
        }

        private void DisablePostProcessingComponents()
        {
            PostProcessVolume[] postProcessingVolumes = FindObjectsOfType<PostProcessVolume>();
            for (int i = 0; i < postProcessingVolumes.Length; ++i)
                postProcessingVolumes[i].enabled = false;
        }
    }
}
