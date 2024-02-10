using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jumping up a ladder
public class Ladder : MonoBehaviour
{
    private void Awake()
    {
        tag = "Ladder";
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.attachedRigidbody.velocity = new Vector3(0, other.attachedRigidbody.velocity.y, other.attachedRigidbody.velocity.z);
        }
    }
}
