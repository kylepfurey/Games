using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXSpawner : MonoBehaviour
{
    public GameObject SoundFXPrefab;

    void Start()
    {
        GameObject.Instantiate(SoundFXPrefab, transform.position, transform.rotation);
    }
}
