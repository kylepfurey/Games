using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wcoll_script : MonoBehaviour
{
    [SerializeField] Collider wall;
    [SerializeField] Rigidbody rb;

    //Jump check
    public bool jumping = false;
    //checks if player is touching a wall
    public bool wTouching = false;
    //checks if wall collision is to the left or right of the player
    public bool LR = false;

    private float rbPosx = 0;
    private float collPosx = 0;

    //Fixes issue where player could jump reght before landing and lock jumping to true
    private void OnTriggerEnter(Collider collider)
    {

        rbPosx = rb.position.x;
        collPosx = collider.transform.position.x;
        if (rbPosx - collPosx > 0)
        {
            LR = false;
            wTouching = true;
        }
        else if (rbPosx - collPosx < 0)
        {
            LR = true;
            wTouching = true;
        }                

    }

    private void OnTriggerExit(Collider collider)
    {
        wTouching = false;

        Debug.Log(wTouching);
    }

}
