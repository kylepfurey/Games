using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVMover : MonoBehaviour
{
    //public float speed = 1f;
    public Vector2 speed;
    private MeshRenderer mesh;
    private Vector2 theVect;

    // Start is called before the first frame update
    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        theVect.y = theVect.y + speed.y * Time.deltaTime;
        theVect.x = theVect.x + speed.x * Time.deltaTime;

        mesh.material.mainTextureOffset = theVect;
    }
}
