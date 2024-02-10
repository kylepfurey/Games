using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stops the player's momentum
public class Web : MonoBehaviour
{
    bool broken = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!broken && other.tag == "Player")
        {
            broken = true;
            other.attachedRigidbody.velocity = Vector3.zero;
            GameManager.Instance.Audio.Play("Lose");
        }
    }
}
