using FPSGame;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPickup : MonoBehaviour
{
    public PlayerCombat Player;

    public int ammoType;
    public Material[] color;
    public Light Light;
    public Image Glow;

    public float followDistance;
    public float followSpeed;
    public float followSpeedIncrease;
    public float followTime;

    public int minimumStart;

    public GameObject soundEffect;

    void Start()
    {
        Player = GameObject.Find("FPSCharacter").gameObject.GetComponent<PlayerCombat>();

        // Random Ammo
        if (ammoType <= minimumStart)
        {
            ammoType = Random.Range(minimumStart, Player.Gun.Length);

            if (Player.unlocked[ammoType] == false)
            {
                for (int i = minimumStart; i < Player.Gun.Length; i++)
                {
                    if (Player.unlocked[ammoType])
                    {
                        break;
                    }

                    ammoType++;

                    if (ammoType > Player.Gun.Length - 1)
                    {
                        ammoType = minimumStart;
                    }
                }

                if (Player.unlocked[ammoType] == false)
                {
                    print("No ammo dropped.");
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        Material[] materials = new Material[3];
        materials[0] = GetComponent<MeshRenderer>().materials[0];
        materials[1] = color[ammoType];
        materials[2] = color[ammoType];

        GetComponent<MeshRenderer>().materials = materials;

        Glow.color = color[ammoType].GetColor("_EmissionColor");
        Glow.color = new Vector4(Glow.color.r, Glow.color.g, Glow.color.b, Glow.color.a / 2);
        Light.color = color[ammoType].GetColor("_EmissionColor");
    }

    void FixedUpdate()
    {
        // Move towards the player and move faster over time
        if (Player != null)
        {
            if (Mathf.Abs(Vector3.Distance(Player.transform.position + new Vector3(0, 1.25f, 0), transform.parent.transform.position)) < followDistance)
            {
                if (Mathf.Abs(Vector3.Distance(Player.transform.position + new Vector3(0, 1.25f, 0), transform.parent.transform.position)) > followSpeed + followSpeedIncrease * followTime)
                {
                    transform.parent.transform.LookAt(Player.transform.position + new Vector3(0, 1.25f, 0));
                    transform.parent.transform.Translate(new Vector3(0, 0, followSpeed + followSpeedIncrease * followTime));
                }
                else
                {
                    transform.parent.transform.position = Player.transform.position;
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

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            Player = collision.gameObject.GetComponent<PlayerCombat>();

            if (Player != null)
            {
                Player.ammoReserve[Player.ammoReserveType[ammoType]] += Player.maxAmmo[ammoType] * 2;

                Destroy(transform.parent.gameObject);
            }
        }
    }
}