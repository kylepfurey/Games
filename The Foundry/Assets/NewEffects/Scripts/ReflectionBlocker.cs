using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Using this to prevent static objects from using reflection probes. Point is to prevent the player's probe from forcing its reflection data into the surrounding environment.
[ExecuteInEditMode]
public class ReflectionBlocker : MonoBehaviour
{
    public bool disableStaticReflectionsOnNextUpdate = false;
    public bool turnStaticToEnvironmental = false;

    void Update()
    {
        if (disableStaticReflectionsOnNextUpdate == true)
        {
            disableStaticReflectionsOnNextUpdate = false;
            BlockStaticReflections();
        }

        if (turnStaticToEnvironmental == true)
        {
            turnStaticToEnvironmental = false;
            SetStaticToEnvironmental();
        }
    }



    private static void BlockStaticReflections()
    {
        foreach (GameObject t in FindObjectsOfType<GameObject>())
        {
            if (t.isStatic == true)
            {
                MeshRenderer theRend = null;
                if (t.TryGetComponent<MeshRenderer>(out theRend))
                {
                    theRend.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                }
            }
        }
    }

    private static void SetStaticToEnvironmental()
    {
        foreach (GameObject t in FindObjectsOfType<GameObject>())
        {
            if (t.isStatic == true)
            {
                MeshRenderer theRend = null;
                if (t.TryGetComponent<MeshRenderer>(out theRend))
                {
                    theRend.gameObject.layer = LayerMask.NameToLayer("Environmental");
                }
            }
        }
    }
}
