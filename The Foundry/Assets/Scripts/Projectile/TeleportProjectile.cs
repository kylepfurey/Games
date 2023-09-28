using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportProjectile : MonoBehaviour
{
    public Vector3 targetPosition;
    public float smoothTime = 0.3f;
    public float ageInSeconds = 5.0f;

    private Vector3 currentVelocity;
    [SerializeField] private bool playerLink;
    [SerializeField] private GameObject Link;
    [SerializeField] private GameObject Player;

    private void Start()
    {
        StartCoroutine(DestroyWhenAgeReached());
        Player = GameObject.Find("FPSCharacter");
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }

    private void LateUpdate()
    {
        if (playerLink)
        {
            Link.transform.position = transform.position;
            Link.transform.LookAt(Player.transform.position + new Vector3(0, 1.25f, 0));
            Link.transform.localScale = new Vector3(Link.transform.localScale.x, Link.transform.localScale.y, Mathf.Abs(Vector3.Distance(Link.transform.position, Player.transform.position + new Vector3(0, 1.25f, 0))));
            Link.transform.Translate(new Vector3(0, 0, Link.transform.localScale.z / 2));
        }
        else
        {
            Link.transform.position = transform.position;
            Link.transform.localScale = Vector3.zero;
        }
    }

    private IEnumerator DestroyWhenAgeReached()
    {
        yield return new WaitForSeconds(ageInSeconds);
        Destroy(gameObject);
    }
}
