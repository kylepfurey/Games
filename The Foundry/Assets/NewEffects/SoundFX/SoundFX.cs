using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    public float duration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }
}
