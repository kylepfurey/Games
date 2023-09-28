using UnityEngine;
using FPSGame;
public class MedkitLogic : MonoBehaviour
{
    public float healValue = 25f;

    private bool hasHealed;

    public string layername;

    public GameObject healFXPrefab;

    public PlayerCombat Player;

    public float followDistance;
    public float followSpeed;
    public float followSpeedIncrease;
    public float followTime;

    void Start()
    {
        Player = GameObject.Find("FPSCharacter").gameObject.GetComponent<PlayerCombat>();
    }

    void FixedUpdate()
    {
        if (Player != null)
        {
            // Move towards the player and move faster over time
            if (Mathf.Abs(Vector3.Distance(Player.transform.position + new Vector3(0, 1.25f, 0), transform.position)) < followDistance)
            {
                if (Mathf.Abs(Vector3.Distance(Player.transform.position + new Vector3(0, 1.25f, 0), transform.position)) > followSpeed + followSpeedIncrease * followTime)
                {
                    transform.LookAt(Player.transform.position + new Vector3(0, 1.25f, 0));
                    transform.Translate(new Vector3(0, 0, followSpeed + followSpeedIncrease * followTime));
                }
                else
                {
                    transform.position = Player.transform.position;
                }

                followTime += Time.deltaTime;
            }
            else
            {
                followTime = 0;
            }
        }
        else
        {
            Player = GameObject.Find("FPSCharacter").gameObject.GetComponent<PlayerCombat>();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == layername)
        {
            Debug.Log($"ISLAYER ");
        }
        else
        {
            Debug.Log($"NOTLAYER ");

        }

        if (hasHealed == false && LayerMask.LayerToName(collision.gameObject.layer) == layername)
        {
            Debug.Log($"COLLISION {collision.gameObject.name}");

            Health theHealth = collision.GetComponent<Health>();
            if (theHealth == null)
            {
                theHealth = collision.GetComponent<Health>();
            }
            if (theHealth == null)
            {
                theHealth = collision.GetComponentInParent<Health>();
            }
            if (theHealth == null)
            {
                theHealth = collision.GetComponentInChildren<Health>();
            }

            if (theHealth != null)
            {
                if (theHealth.currentHealth + healValue > theHealth.maxHealth)
                {
                    theHealth.OnTakeDamage(theHealth.currentHealth - theHealth.maxHealth, null);
                }
                else
                {
                    theHealth.OnTakeDamage(-healValue, null);
                }

                //theHealth.OnTakeDamage(-healValue, null);
                hasHealed = true;
                GameObject.Instantiate(healFXPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

    }
}