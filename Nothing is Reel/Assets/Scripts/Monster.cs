using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    public Inventory inventory;
    public Transform target;
    public bool movementStart;
    public float monsterSpeed = .095f;

    // Monster faces player at all times but moves along path.
    void FixedUpdate()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }

        if (movementStart == true)
        {
            if (transform.position.x < -4)
            {
                transform.position += new Vector3(monsterSpeed, 0, 0);
            }

            if (transform.position.x >= -4)
            {
                if (transform.position.z > 6)
                {
                    transform.position -= new Vector3(0, 0, monsterSpeed);
                }
            }

            if (transform.position.z <= 6)
            {
                if (transform.position.x < 19)
                {
                    transform.position += new Vector3(monsterSpeed, 0, 0);
                }
            }
        }
    }
}