using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererDisabler : MonoBehaviour
{
    public MeshRenderer[] meshes;
    public bool testbuttonlol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (testbuttonlol)
        {
            testbuttonlol = false;
            DisableAllRenderers();
        }
    }

    void DisableAllRenderers()
    {
        meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = false;
        }
    }
}
