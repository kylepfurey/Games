using System.Globalization;
using UnityEngine;

public class TooManyLights : MonoBehaviour
{
    public KartSettings KartSettings;
    public string lightName;
    public int count = 0;
    public Light[] lights;

    void Start()
    {
        GameObject Object0 = GameObject.FindWithTag("Settings");
        KartSettings = Object0.GetComponent<KartSettings>();

        foreach (Light currentLight in FindObjectsOfType(typeof(Light)))
        {
            if (currentLight.name == lightName)
            {
                count++;
            }
        }

        lights = new Light[count];
        count = 0;

        foreach (Light currentLight in FindObjectsOfType(typeof(Light)))
        {
            if (currentLight.name == lightName)
            {
                lights[count] = currentLight;
                count++;
            }
        }
    }

    // Find all lights of the given name and enable or disable them based on whether it is night.
    void Update()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = KartSettings.night;
        }
    }
}
