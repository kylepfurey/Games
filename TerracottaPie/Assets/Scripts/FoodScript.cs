using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    static CollectionManager collectionManager;

    public bool goodItem = true;

    public bool pushPlayer = false;
    public float forceScale = 1;

    public float floatSpeed = 7.5f;

    public float itemForce = 5;
    public GameObject itemPrefab = null;

    private static GameObject itemSpawnPoint = null;

    private bool grabbed = false;
    private bool itemSpawned = false;

    private void Awake()
    {
        if (collectionManager == null)
        {
            collectionManager = FindFirstObjectByType<CollectionManager>();
        }

        if (itemSpawnPoint == null)
        {
            itemSpawnPoint = GameObject.FindGameObjectWithTag("ItemSpawn");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (itemSpawned)
        {
            return;
        }

        if (!grabbed)
        {
            if (other.tag != "Player")
            {
                return;
            }

            if (goodItem)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.pickUpGood);

                collectionManager.collectionScore += collectionManager.goodScoreAddition;
            }
            else if (!goodItem)
            {
                other.GetComponent<PlayerMovement>().Damage(pushPlayer, forceScale);
            }

            if (!pushPlayer)
            {
                grabbed = true;
            }
        }
        else if (other.tag == "Ceiling")
        {
            Rigidbody item = Instantiate(itemPrefab).GetComponent<Rigidbody>();

            item.transform.position = itemSpawnPoint.transform.position;

            item.transform.rotation = itemSpawnPoint.transform.rotation;

            item.AddForce(item.transform.forward * itemForce, ForceMode.Impulse);

            item.angularVelocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * itemForce;

            Destroy(gameObject);

            itemSpawned = true;
        }
    }

    private void FixedUpdate()
    {
        if (grabbed)
        {
            Vector3 pos = transform.position;

            pos.y += floatSpeed * Time.fixedDeltaTime;

            transform.position = pos;

            transform.Rotate(0, 0, floatSpeed);
        }
    }
}
