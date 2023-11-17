using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{
    [SerializeField] private float expansionRate;
    private void Update()
    {
        Expand();
    }
    private void Expand()
    {
        transform.localScale +=
            Vector3.one
            * expansionRate
            * GameManager.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        if (otherRB == null)
            return;

        if (other.tag == "Player")
            return;

        otherRB.constraints = new RigidbodyConstraints();

        otherRB.AddForce(
            (other.transform.position - transform.position)//get direction
            .normalized//set it to 1
            * otherRB.mass//mass irelevent when faced with this kind of force
            * GameManager.timeScale//slow down force based on time
            * 500//scaler
            );
    }
    private void OnTriggerStay(Collider other)
    {
        Rigidbody otherRB = other.GetComponent<Rigidbody>();
        if (otherRB == null)
            return;

        otherRB.AddForce(
            (other.transform.position - transform.position)//get direction
            .normalized//set it to 1
            * otherRB.mass//mass irelevent when faced with this kind of force
            * GameManager.timeScale//slow down force based on time
            * 1//scaler
            );
    }
}
