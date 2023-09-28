using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public BezierCurveData curveData;
    public List<GameObject> points;

    public GameObject target;
    public GameObject LeftOrRightObject;

    public float speedMultiplier = 1f;
    public float T;

    public bool isTheSecondOne;
    public enum MissileState
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        //ConvertListToBez();
        ConvertListToNewBez();
    }

    void ConvertListToBez()
    {
        Vector3[] theVects = new Vector3[points.Count];

        for (int i = 0; i < points.Count; i++)
        {
            theVects[i] = points[i].transform.position;
        }
        curveData.points = theVects;
    }
    void ConvertListToNewBez()
    {
        Vector3[] theVects = new Vector3[4];
        theVects[0] = transform.position;


        theVects[1] = LeftOrRightObject.transform.position;
        ////Vector3 theVectLOL = target.transform.position;
        ////theVectLOL.y = LeftOrRightObject.transform.position.y;
        ////theVects[2] = theVectLOL;

        Vector3 theVect = target.transform.position;
        theVect.y = transform.position.y;
        theVects[2] = theVect;

        theVects[3] = target.transform.position;
        curveData.points = theVects;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = curveData.GetPoint(T);
        curveData.points[3] = target.transform.position;
        OpeningLerp();
    }

    private float scaleVal;
    private void OpeningLerp()
    {
        if (scaleVal < 1)
        {
            scaleVal += Time.deltaTime * speedMultiplier;
            T = scaleVal;
        }
        else
        {
            DetonateLol();
        }
    }

    public void DetonateLol()
    {
        TargetZoneScript theZone = target.GetComponent<TargetZoneScript>();
        theZone.Detonate();
        Destroy(gameObject);
    }
}
