using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only collides if the player is above
public class Platform : MonoBehaviour
{
    private p_movement Player;
    private Collider Collider;

    void Awake()
    {
        Player = GameManager.Player;
        Collider = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        // Check if the player's feet is above the platform's top
        Collider.enabled = (transform.position.y + (transform.lossyScale.x / 2) < Player.transform.position.y - (Player.transform.lossyScale.y / 2));
    }
}
