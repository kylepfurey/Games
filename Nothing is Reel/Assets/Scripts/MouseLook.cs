using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=_QajrabyTJc&list=LLEVPW3phM9vFPCbwg2mo6oA&index=2 SOURCE

public class MouseLook : MonoBehaviour
{

    public float mouseSensitvity = 100f;

    public Transform playerBody;

    public PlayerMovement player;

    float xRotation = 0f;
    float mouseX;
    float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.movement == true)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitvity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitvity * Time.deltaTime;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}