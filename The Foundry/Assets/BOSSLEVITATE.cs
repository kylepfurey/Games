using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSLEVITATE : MonoBehaviour
{
    public float timer;
    public float timerReset = 100000;
    public float startHeight;
    public float levitateSpeed = 1f;
    public float levitateHeight = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        startHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, startHeight + Mathf.Sin(timer * levitateSpeed) * levitateHeight, transform.position.z);

        timer += Time.deltaTime;

        if (timer >= timerReset)
        {
            timer -= timerReset;
        }
    }

}
