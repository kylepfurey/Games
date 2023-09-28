using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorUnit : MonoBehaviour
{
    public GameObject doorObject;
    public Transform openPosition;
    public Transform closedPosition;

    private DoorSystem doorSystem;

    public float speedMultiplier = 1.0f;
    private bool Opening;
    // Start is called before the first frame update
    void Start()
    {
        doorSystem = GetComponent<DoorSystem>();
        doorSystem.openDoors += Open;
        doorSystem.closeDoors += Close;
    }

    // Update is called once per frame
    void Update()
    {
        if (Opening)
        {
            OpeningUpdate();
        }
        else
        {
            ClosingUpdate();
        }
    }

    //Call this to turn the screen on
    public void Open()
    {
        Opening = true;
    }

    //Call this to turn the screen off
    public void Close()
    {
        Opening = false;
    }
    private void OpeningUpdate()
    {
        OpeningLerp();
    }


    private float scaleVal;
    private void OpeningLerp()
    {
        if (scaleVal < 1)
        {
            scaleVal += Time.deltaTime * speedMultiplier;
            FullLerp(doorObject, closedPosition, openPosition, scaleVal);
        }
    }

    private void ClosingUpdate()
    {
        if (scaleVal > 0)
        {
            scaleVal -= Time.deltaTime * speedMultiplier;
            FullLerp(doorObject, closedPosition, openPosition, scaleVal);
        }
    }

    private void FullLerp(GameObject obj, Transform A, Transform B, float T)
    {
        obj.transform.position = Vector3.Lerp(A.position, B.position, T);
        obj.transform.rotation = Quaternion.Lerp(A.rotation, B.rotation, T);
        obj.transform.localScale = Vector3.Lerp(A.localScale, B.localScale, T);
    }
}
