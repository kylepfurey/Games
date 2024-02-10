using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string item;
    [HideInInspector] private Inventory Inventory;
    private bool interactable;

    private void Start()
    {
        Inventory = GameManager.Player.GetComponent<Inventory>();
    }

    private void Update()
    {
        if (interactable && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            switch (item.ToLower())
            {
                default:
                    print("No item set!");
                    break;

                case "bottle cap":
                    Inventory.bottleCaps++;
                    break;

                case "pencil":
                    Inventory.pencil = true;
                    break;

                case "cinnamon stick":
                    Inventory.cinnamonStick = true;
                    break;

                case "pinwheel":
                    Inventory.pinwheel = true;
                    break;

                case "paper clip":
                    Inventory.paperClip = true;
                    break;

                case "rubber band":
                    Inventory.rubberBand = true;
                    break;
            }

            GameManager.Audio.PlayOnce("Item");

            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactable = false;
        }
    }
}
