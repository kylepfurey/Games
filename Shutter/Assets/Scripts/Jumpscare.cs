using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public Vector3 target = Vector3.zero;
    public float speed = 4;

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, target) <= 0.005f)
        {
            transform.localPosition = target;
        }
    }
}
