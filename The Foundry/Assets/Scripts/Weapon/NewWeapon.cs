using FPSGame;
using UnityEngine;

public class NewWeapon : MonoBehaviour
{
    public int weapon;

    public bool levitate;
    public float levitateSpeed;
    public float levitateHeight;

    public float timerReset;
    public float timer;
    public float startHeight;

    public bool rotate;
    public Vector3 rotateSpeed;

    public GameObject soundEffect;

    void Start()
    {
        startHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        if (levitate)
        {
            transform.position = new Vector3(transform.position.x, startHeight + Mathf.Sin(timer * levitateSpeed) * levitateHeight, transform.position.z);

            timer += Time.deltaTime;

            if (timer >= timerReset)
            {
                timer -= timerReset;
            }
        }

        if (rotate)
        {
            transform.Rotate(rotateSpeed);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerCombat Player = collision.gameObject.GetComponent<PlayerCombat>();

            if (Player != null)
            {
                Player.Weapon = weapon;
                Player.unlocked[weapon] = true;
                Player.ammoReserve[Player.ammoReserveType[weapon]] += Player.maxAmmo[weapon] * 2;
                Player.reloadTimer = 0;

                Instantiate(soundEffect, transform.position, transform.rotation, null);

                Destroy(gameObject);
            }
        }
    }
}