using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrazoaSwitchGlowy : MonoBehaviour
{
    public Vector2 speed;
    private Renderer flowmesh;
    private Vector2 theVect;


    public float timeToLerp;
    private float lerpStart;
    private float lerpEnd;
    private bool hasActivated;
    //private Color theColor;
    //private float lerpCurrent;
    public float goalIntensity;
    public float testVal;
    public bool testbutton = false;
    void Start()
    {
        flowmesh = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        theVect.y = theVect.y + speed.y * Time.deltaTime;
        theVect.x = theVect.x + speed.x * Time.deltaTime;
        flowmesh.material.mainTextureOffset = theVect;
        //flowmesh.material.SetTextureOffset("_EmissiveColorMap", theVect);

        //if (testbutton)
        //{
        //    testbutton = false;
        //    ActivateGlow();
        //}
        //if (hasActivated && testVal < 1)
        //{
        //    glowUpdate();
        //}
    }

    //public void ActivateGlow()
    //{
    //    lerpStart = Time.time;
    //    lerpEnd = Time.time + timeToLerp;
    //    testVal = 0;

    //    //float emissiveIntensity = Mathf.Lerp(lerpStart, lerpEnd, Time.time);
    //    //Color emissiveColor = Color.white;
    //    //glowMesh.material.SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);
    //    //flowmesh.material.SetColor("_EmissiveColor", Color.Lerp(Color.black, Color.white, testVal));
    //    hasActivated = true;

    //}
    //public void glowUpdate()
    //{
    //    //testVal = Mathf.Lerp(0, lerpEnd - lerpStart, Time.time - lerpEnd);
    //    //testVal = Mathf.Lerp(0, timeToLerp, Time.time - lerpStart)/timeToLerp;
    //    testVal = (Time.time - lerpStart) / timeToLerp;
    //    foreach (Material mat in flowmesh.materials)
    //    {
    //        //mat.SetColor("_EmissiveColor", Color.Lerp(Color.black, Color.white * goalIntensity, testVal));
    //    }
    //}
}
