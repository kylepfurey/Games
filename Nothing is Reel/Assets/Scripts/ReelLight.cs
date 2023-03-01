using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelLight : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}