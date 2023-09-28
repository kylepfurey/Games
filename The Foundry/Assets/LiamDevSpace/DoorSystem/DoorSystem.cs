using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public delegate void OpenDoors();
    public OpenDoors openDoors;

    public GameObject soundFXOpen;
    public GameObject soundFXClose;
    public void OpenTheDoor()
    {
        openDoors();
        GameObject.Instantiate(soundFXOpen, transform.position, transform.rotation);
    }
    public delegate void CloseDoors();
    public CloseDoors closeDoors;
    public void CloseTheDoor()
    {
        closeDoors();
        GameObject.Instantiate(soundFXOpen, transform.position, transform.rotation);
    }

}
