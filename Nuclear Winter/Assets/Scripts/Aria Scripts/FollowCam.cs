using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] GameObject target;
    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            Time.deltaTime * 10);
    }
}
