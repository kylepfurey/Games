using UnityEngine;

public class Animation : MonoBehaviour
{
    // Timer Value
    public float timer = 0;
    public float timerSpeed = 2;
    public bool levitationOn;
    public bool colorOn;

    // Location
    public Vector3 originalPosition;

    // Color Change
    public Renderer Renderer;
    public Material Off;
    public Material On;

    public void Start()
    {
        // Start Position
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Timer Increment
        timer = timer + timerSpeed;

        // Timer Reset
        if (timer >= 120)
        {
            timer = 0;

            if (levitationOn)
            {
                transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
            }
        }


        // Color Change
        if (colorOn)
        {
            switch (timer)
            {
                case 0:
                    Renderer.material = Off;
                    break;
                case 30:
                    Renderer.material = On;
                    break;
                case 60:
                    Renderer.material = Off;
                    break;
                case 90:
                    Renderer.material = On;
                    break;
            }
        }


        // Levitation Equation: Sin(Timer / speedValue) / heightValue + currentHeight
        if (levitationOn)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Sin(timer / 10) / 50 + transform.position.y, transform.position.z);
        }
    }
}
