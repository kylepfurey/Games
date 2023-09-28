using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhasesLOL : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] spawnPositions;
    public List<GameObject> spawnedObjectsLOL = new List<GameObject>();

    public MIDPHASEMUWAHAHAHAH midphase;
    public FinalAIStatemachine theMachine;

    private void Start()
    {
        midphase = GetComponentInChildren<MIDPHASEMUWAHAHAHAH>();
        theMachine = GetComponent<FinalAIStatemachine>();
    }
    //public BossWait theWaitScript;
    //public FinalAIStatemachine theMachine;
    public void SpawnBaddies()
    {
        foreach (GameObject go in spawnPositions)
        {
            spawnedObjectsLOL.Add( GameObject.Instantiate(enemyPrefab, go.transform.position, go.transform.rotation));

        }
        ShieldsUp();
    }

    public void ShieldsUp()
    {
        theMachine.SwitchToState(midphase);
    }

    public void Update()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject go in spawnedObjectsLOL)
        {
            if (go == null)
            {
                list.Add(go);
            }
        }

        foreach (GameObject go in list)
        {
            spawnedObjectsLOL.Remove(go);
        }
    }
}
