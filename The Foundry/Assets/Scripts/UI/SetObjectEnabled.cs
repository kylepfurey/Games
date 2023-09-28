using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectEnabled : MonoBehaviour
{
    public void EnableGameObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void DisableGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
