using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    [SerializeField] private ItemDefinition Item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory Inventory = collision.gameObject.GetComponent<Inventory>();

        if (Inventory != null)
        {
            if (Inventory.GetItem(Item))
            {
                Destroy(gameObject);
            }
        }
    }
}

// SOURCE Professor Funky Feeney's Repo