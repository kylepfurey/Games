using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Monster monster;
    public Transform target;
    public bool death;

    public AudioSource deathSound;

    // On collision with the hitbox, you die.
    public void OnTriggerEnter(Collider other)
    {
        death = true;
        deathSound.Play();
        monster.movementStart = false;
    }

    void Start()
    {
        death = false;
        monster.movementStart = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // Hitbox moves without facing at the player.
    void FixedUpdate()
    {

        if (monster.movementStart == true)
        {
            if (transform.position.x < -4)
            {
                transform.position += new Vector3(monster.monsterSpeed, 0, 0);
            }

            if (transform.position.x >= -4)
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
                if (transform.position.z > 6)
                {
                    transform.position -= new Vector3(0, 0, monster.monsterSpeed);
                }
            }

            if (transform.position.z <= 6)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                if (transform.position.x < 19)
                {
                    transform.position += new Vector3(monster.monsterSpeed, 0, 0);
                }
            }
        }
    }
}