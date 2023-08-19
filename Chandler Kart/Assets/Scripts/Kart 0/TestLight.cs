using UnityEngine;
using UnityEngine.InputSystem;

public class TestLight : MonoBehaviour
{
    public TestKart Kart;

    public Light Lighting;
    public bool LightOn = false;
    public bool lightUp = true;
    public bool beepUp = true;

    // Lights On or Off
    public Renderer HeadLights1;
    public Renderer HeadLights2;
    public Renderer RearLights1;
    public Renderer RearLights2;
    public Material HeadLightsOn;
    public Material HeadLightsOff;
    public Material RearLightsOn;
    public Material RearLightsOff;

    // Wheels
    public GameObject Wheel1;
    public GameObject Wheel2;

    void Update()
    {
        if (Kart.start)
        {
            bool LIGHT = Button(Kart.Input.actions.FindAction("Light").ReadValue<float>());
            bool BEEP = Button(Kart.Input.actions.FindAction("Beep").ReadValue<float>());


            // Turn Wheels
            Wheel1.transform.localEulerAngles = new Vector3(90, Kart.Input.actions.FindAction("Turn").ReadValue<Vector2>().x * 22.5f, 0);
            Wheel2.transform.localRotation = Wheel1.transform.localRotation;

            if (Kart.CharacterHead != null)
            {
                Kart.CharacterHead.transform.localEulerAngles = new Vector3(0, Kart.Input.actions.FindAction("Turn").ReadValue<Vector2>().x * 67.5f, 0);
            }


            // Head Lights and Rear Lights
            if (LightOn)
            {
                HeadLights1.material = HeadLightsOn;
                HeadLights2.material = HeadLightsOn;
            }
            else
            {
                HeadLights1.material = HeadLightsOff;
                HeadLights2.material = HeadLightsOff;
            }

            if ((Kart.Input.actions.FindAction("Reverse").ReadValue<float>() > 0 && Kart.Input.actions.FindAction("Accelerate").ReadValue<float>() == 0) || LightOn)
            {
                RearLights1.material = RearLightsOn;
                RearLights2.material = RearLightsOn;
            }
            else
            {
                RearLights1.material = RearLightsOff;
                RearLights2.material = RearLightsOff;
            }


            // Turn Light On or Off
            if (LIGHT && lightUp)
            {
                lightUp = false;
            }

            if (LIGHT == false)
            {
                lightUp = true;
            }


            if (LightOn)
            {
                Lighting.intensity = 0.5f;
            }
            else
            {
                Lighting.intensity = 0;
            }


            // Beeping Horn
            if (BEEP && beepUp == true)
            {
                beepUp = false;
            }

            if (BEEP == false)
            {
                beepUp = true;
            }
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