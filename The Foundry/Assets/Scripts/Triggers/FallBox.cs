using FPSGame;
using UnityEngine;

public class FallBox : MonoBehaviour
{
    [SerializeField] private PlayerMovement Player;
    [SerializeField] private Health Health;
    [SerializeField] private int Damage;
    [SerializeField] private bool TeleportToObject;
    [SerializeField] private GameObject TeleportObject;

    void Update()
    {
        if (Player == null)
        {
            GameObject player = GameObject.Find("FPSCharacter");

            if (player != null)
            {
                Player = player.GetComponent<PlayerMovement>();
            }
        }

        if (Player != null && Health == null)
        {
            Health = Player.GetComponent<Health>();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (Player != null && Health != null)
        {
            if (collision == Player.GetComponent<CapsuleCollider>())
            {
                if (TeleportToObject && TeleportObject != null)
                {
                    Player.Motor.SetPosition(TeleportObject.transform.position, true);
                    Player.teleportTargetPosition = TeleportObject.transform.position;
                }
                else
                {
                    Player.Motor.SetPosition(Player.LastStablePosition, true);
                    Player.teleportTargetPosition = Player.LastStablePosition;
                }

                Health.OnTakeDamage(Damage, null);
            }
        }
    }
}