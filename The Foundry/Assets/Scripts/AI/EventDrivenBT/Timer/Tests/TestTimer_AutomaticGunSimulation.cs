using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameFramework;
using System;

public class TestTimer_AutomaticGunSimulation : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float fireRate = 1.0f;

    private bool isFiring;

    InputAction interact;

    private void Start()
    {
        if (!playerInput)
            playerInput = FindObjectOfType<PlayerInput>();

        interact = playerInput.actions.FindAction("Interact");
        interact.performed += OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        if (!isFiring)
            StartFiring();
        else
            StopFiring();
    }

    private void StartFiring()
    {
        if (isFiring)
            return;

        Debug.Log("Start firing.");

        isFiring = true;

        FireLoop();
        TimerManager.Instance.AddTimer(FireLoop, fireRate, -1);
    }

    private void StopFiring()
    {
        if (!isFiring)
            return;

        Debug.Log("Stop firing.");

        TimerManager.Instance.RemoveTimer(FireLoop);

        isFiring = false;
    }

    private void FireLoop()
    {
        Debug.Log("Fire!");
    }
}
