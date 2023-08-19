using UnityEngine;

public class Camera3 : MonoBehaviour
{
    public Kart3 Kart;
    public Camera Camera;
    public float cameraAngle;
    public float angleConstant;

    public bool lockCamera = false;
    public bool on = false;

    void Start()
    {
        cameraAngle = Kart.rotation.y;
    }

    void Update()
    {
        Camera.enabled = on;

        if (lockCamera == false && Kart.start)
        {
            // Get the Right Joystick Control and add its movement to the camera
            float CAMERATURNX = Kart.Input.actions.FindAction("Camera Turn").ReadValue<Vector2>().x;
            float CAMERATURNY = Kart.Input.actions.FindAction("Camera Turn").ReadValue<Vector2>().y;

            // Follow the Car's rotation
            cameraAngle = Kart.transform.eulerAngles.y;

            if (CAMERATURNX > 0.25f || CAMERATURNX < -0.25f || CAMERATURNY > 0.25f || CAMERATURNY < -0.25f)
            {
                cameraAngle = cameraAngle + Mathf.Atan2(CAMERATURNX, CAMERATURNY) * Mathf.Rad2Deg;
            }

            // Move Camera
            transform.position = Kart.position;
            transform.eulerAngles = new Vector3(0, cameraAngle, 0);
        }
    }

    public bool Button(float input)
    {
        if (input > 0)
        {
            return true;
        }

        return false;
    }
}
