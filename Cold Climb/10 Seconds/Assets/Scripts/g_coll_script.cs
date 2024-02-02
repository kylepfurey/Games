using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_script : MonoBehaviour
{
    [SerializeField] Animator animation;
    [SerializeField] Collider ground;
    [SerializeField] Rigidbody rb;

    //Jump check
    public bool jumping = false;
    //checks if player has already wall-jumped
    public bool canWJ = false;
    //checks if wall collision is to the left or right of the player


    //Used to check for ground collision in order to update the jumping boolean
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.isTrigger || collider.tag == "Ladder")
        {
            jumping = false;

            canWJ = false;

            animation.SetTrigger("Land");
        }
    }
    //Fixes issue where player could jump reght before landing and lock jumping to true
    private void OnTriggerStay(Collider collider)
    {
        if ((!collider.isTrigger || collider.tag == "Ladder") && jumping == true && rb.velocity.y <= 0)
        {
            jumping = false;
            canWJ = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!collider.isTrigger || collider.tag == "Ladder")
        {
            canWJ = true;
            animation.ResetTrigger("Land");
        }
    }
}
