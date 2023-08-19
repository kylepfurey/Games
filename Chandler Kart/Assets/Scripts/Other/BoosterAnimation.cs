using UnityEngine;

public class BoosterAnimation : MonoBehaviour
{
    // Color Change
    public Renderer Renderer;
    public Material Red;
    public Material Orange;
    public Material Yellow;

    // Timer Value
    public float timer = 0;

    // Color Offest Value
    public int offset;


    void FixedUpdate()
    {
        // Timer Increment
        timer = timer + 6;

        // Timer Reset
        if (timer > 90)
        {
            timer = 0;
        }


        // Color Change
        switch (timer)
        {
            case 0:
                switch (offset)
                {
                    case 0:
                        Renderer.material = Red;
                        break;
                    case 1:
                        Renderer.material = Orange;
                        break;
                    case 2:
                        Renderer.material = Yellow;
                        break;
                }
                break;

            case 30:
                switch (offset)
                {
                    case 0:
                        Renderer.material = Orange;
                        break;
                    case 1:
                        Renderer.material = Yellow;
                        break;
                    case 2:
                        Renderer.material = Red;
                        break;
                }
                break;

            case 60:
                switch (offset)
                {
                    case 0:
                        Renderer.material = Yellow;
                        break;
                    case 1:
                        Renderer.material = Red;
                        break;
                    case 2:
                        Renderer.material = Orange;
                        break;
                }
                break;
        }
    }
}
