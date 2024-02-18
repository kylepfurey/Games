using System.Collections;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float destroyTimer = 2;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= destroyTimer)
        {
            Destroy(gameObject);
        }
    }
}
