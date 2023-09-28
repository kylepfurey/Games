using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbitrarySoundTrigger : MonoBehaviour
{
    public GameObject thePrefab;
    public void SpawnTheObject()
    {
        GameObject.Instantiate(thePrefab, transform.position, transform.rotation);
        GameObject.Find("MusicSystemExample").GetComponent<MusicSystem>().StopAllInstantly();
    }
}
