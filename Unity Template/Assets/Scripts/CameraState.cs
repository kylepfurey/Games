using System;
using UnityEngine;

[Serializable]
public class CameraState
{
    public enum CameraType { FixedCamera, FollowCamera };

    public string stateName;

    public CameraType cameraType;

    // Position and Rotation of the Fixed Camera
    public Vector3 fixedPosition;
    public Vector3 fixedRotation;
    public bool lookAtPlayer;

    // Distance and Rotation Modifiers for Following Camera
    public Vector3 distance;
    public Vector3 angle;

    // Transition Variables
    public bool smoothTransition;

    public float transitionMovementSpeed;
    public float transitionRotationSpeed;

    // Player's New Rotation
    public Vector3 playerRotation;

    // Direction of the Player's Movement (-1 or 1)
    public int direction = 1;
}