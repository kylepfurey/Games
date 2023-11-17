using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private itemType item;
    private bool canGrab;

    enum itemType
    {
        Oxygen,
        Helmet,
        Suit
    }

    private void Update()
    {
        if (canGrab && GameManager.player.INTERACT && GameManager.player.INTERACT_UP)
        {
            GameManager.player.INTERACT_UP = false;

            switch (item)
            {
                case itemType.Oxygen:

                    GameManager.player.oxygen = true;

                    break;

                case itemType.Helmet:

                    GameManager.player.helmet = true;

                    break;

                case itemType.Suit:

                    GameManager.player.suit = true;

                    break;
            }

            GameManager.audio.PlayOnce("Pick Up");

            if (GameManager.player.oxygen && GameManager.player.helmet && GameManager.player.suit)
            {
                GameManager.player.endText.start = true;
                Destroy(GameManager.player.endWall);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canGrab = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canGrab = false;
        }
    }
}
