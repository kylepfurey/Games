using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

// https://www.youtube.com/watch?v=_QajrabyTJc&list=LLEVPW3phM9vFPCbwg2mo6oA&index=2 SOURCE

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 4f;
    public float gravity = -10f;

    public bool movement;

    public AudioSource footsteps;
    public Hitbox hitbox;

    public Vector3 velocity;
    public bool isGrounded;

    void Start()
    {
        transform.position = new Vector3(18.54f, 1.5f, 5.8f);
    }

    void Update()
    {
        // Movement
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * X + transform.forward * Z;

        if (hitbox.death == false && movement == true)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        // https://www.youtube.com/watch?v=uNYF1gsvD1A SOURCE

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            footsteps.enabled = true;
        }
        else
        {
            footsteps.enabled = false;
        }

        if (hitbox.death == true || movement == false)
        {
            footsteps.enabled = false;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}