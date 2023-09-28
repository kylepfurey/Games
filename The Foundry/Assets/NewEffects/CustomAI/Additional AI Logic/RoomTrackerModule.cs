using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ROOM TRACKER MODULE
//This is a brief script to section off logic for keeping track of the collider representing the walkable area the AI character is currently in
//Script implements FindObjectsOfType on Awake. Performance implications on character instantiation unknown.
//WRITTEN BY LIAM SHELTON
public class RoomTrackerModule : MonoBehaviour
{
    public Collider currentRoom;

    private void Awake()
    {
        //set the starting room
        foreach (Collider t in FindObjectsOfType<Collider>())
        {
            if (LayerMask.LayerToName(t.gameObject.layer) == "NavRoomCollider")
            {
                if (t.bounds.Contains(gameObject.transform.position))
                {
                    Debug.Log($"AGENT {gameObject.name} has entered room {t.gameObject.transform.parent.gameObject.name} ON AWAKE");
                    currentRoom = t;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "NavRoomCollider")
        {
            if (currentRoom != other)
            {
                Debug.Log($"AGENT {gameObject.name} has entered room {other.gameObject.transform.parent.gameObject.name}");
                currentRoom = other;
            }
        }
    }
}
