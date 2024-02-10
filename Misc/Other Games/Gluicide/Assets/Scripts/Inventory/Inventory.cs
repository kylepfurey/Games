using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Player Inventory
    private bool playerInventory;

    // Item Enumerator
    public enum ItemType
    {
        NULL = 0,
        Money = 1,
        Glue = 2,
        Wallet = 3,
        GlueGun = 4,
        GravityGun = 5,
        Length
    }

    // Stats Reference
    private StatSystem Stats;
    public bool hasGlueGun;
    public bool hasGravityGun;
    public bool hasWallet;

    // Item Drop Prefabs
    [SerializeField] private GameObject moneyItemDrop;
    [SerializeField] private GameObject glueItemDrop;
    [SerializeField] private GameObject walletItemDrop;
    [SerializeField] private GameObject glueGunItemDrop;
    [SerializeField] private GameObject gravityGunItemDrop;

    public void Start()
    {
        // Check if Player
        if (GetComponent<Player>() != null)
        {
            playerInventory = true;
        }
        else
        {
            playerInventory = false;
        }

        // Get Stat System
        if (Stats == null)
        {
            if (GetComponent<StatSystem>() != null)
            {
                Stats = GetComponent<StatSystem>();
            }
            else
            {
                print("No Stat System was found!");
            }
        }
    }

    public bool GetItem(ItemDefinition item)
    {
        bool success = false;

        if (item.grantsGlueGun)
        {
            hasGlueGun = true;

            if (playerInventory)
            {
                if (GameManager.Player.Gun != null)
                {
                    if (GameManager.Player.Gun.hasGrabbedObject == false)
                    {
                        GameManager.Player.primaryShot = GameManager.Player.glueIsPrimary;
                    }
                }
            }

            success = true;
        }

        if (item.grantsGravityGun)
        {
            hasGravityGun = true;

            if (playerInventory)
            {
                GameManager.Player.primaryShot = !GameManager.Player.glueIsPrimary;
            }

            success = true;
        }

        if (item.grantsWallet)
        {
            hasWallet = true;
            success = true;
        }

        if (item.healthGranted > 0)
        {
            if (GetComponent<Health>() != null)
            {
                print(name + " healed " + item.healthGranted + " Health.");

                GetComponent<Health>().Heal(item.healthGranted);

                success = true;
            }
        }

        switch (item.item)
        {
            case StatSystem.StatType.NULL:

                print("You were right, we do need a NULL value.");

                break;


            case StatSystem.StatType.Glue:

                if (hasGlueGun)
                {
                    if (Stats != null)
                    {
                        Stats.AddStat(item.item, item.amount);

                        success = true;
                    }
                    else
                    {
                        print("No Stat System found!");
                    }
                }

                break;


            case StatSystem.StatType.Money:

                if (hasWallet)
                {
                    if (Stats != null)
                    {
                        Stats.AddStat(item.item, item.amount);

                        success = true;
                    }
                    else
                    {
                        print("No Stat System found!");
                    }
                }

                break;
        }

        return success;
    }

    public bool RemoveItem(ItemType itemType, int amount)
    {
        bool success = false;

        switch (itemType)
        {
            default:

                print("No item was found!");

                break;


            case ItemType.Money:

                if (Stats != null)
                {
                    if (Stats.GetStat(StatSystem.StatType.Money) >= amount)
                    {
                        print("Money was lost.");

                        Stats.AddStat(StatSystem.StatType.Money, -amount);

                        success = true;
                    }
                    else if (Stats.GetStat(StatSystem.StatType.Money) < amount)
                    {
                        print("Money was lost.");

                        Stats.AddStat(StatSystem.StatType.Money, -Stats.GetStat(StatSystem.StatType.Money));

                        success = true;
                    }
                }
                else
                {
                    print("No Stat System found!");
                }

                break;


            case ItemType.Glue:

                if (Stats != null)
                {
                    if (Stats.GetStat(StatSystem.StatType.Glue) >= amount)
                    {
                        print("Glue was lost.");

                        Stats.AddStat(StatSystem.StatType.Glue, -amount);

                        success = true;
                    }
                    else if (Stats.GetStat(StatSystem.StatType.Glue) < amount)
                    {
                        print("Glue was lost.");

                        Stats.AddStat(StatSystem.StatType.Glue, -Stats.GetStat(StatSystem.StatType.Glue));

                        success = true;
                    }
                }
                else
                {
                    print("No Stat System found!");
                }

                break;


            case ItemType.Wallet:

                if (hasWallet)
                {
                    hasWallet = false;

                    print("Wallet was dropped.");

                    success = true;

                    if (Stats != null)
                    {
                        if (Stats.GetStat(StatSystem.StatType.Money) > 5)
                        {
                            Stats.AddStat(StatSystem.StatType.Money, -5);
                        }
                        else
                        {
                            Stats.AddStat(StatSystem.StatType.Money, -Stats.GetStat(StatSystem.StatType.Money));
                        }
                    }
                    else
                    {
                        print("No Stat System found!");
                    }
                }
                else
                {
                    print("Does not have wallet.");
                }

                break;


            case ItemType.GlueGun:

                if (hasGlueGun)
                {
                    hasGlueGun = false;

                    print("Glue gun was dropped.");

                    success = true;

                    if (Stats != null)
                    {
                        if (Stats.GetStat(StatSystem.StatType.Glue) > 3)
                        {
                            Stats.AddStat(StatSystem.StatType.Glue, -3);
                        }
                        else
                        {
                            Stats.AddStat(StatSystem.StatType.Glue, -Stats.GetStat(StatSystem.StatType.Glue));
                        }
                    }
                    else
                    {
                        print("No Stat System found!");
                    }
                }
                else
                {
                    print("Does not have glue gun.");
                }

                break;


            case ItemType.GravityGun:

                if (hasGravityGun)
                {
                    hasGravityGun = false;

                    print("Gravity gun was dropped.");

                    success = true;
                }
                else
                {
                    print("Does not have gravity gun");
                }

                break;
        }

        return success;
    }

    public void DropItem(ItemType itemType)
    {
        GameObject item = new GameObject();

        switch (itemType)
        {
            default:

                print("No item was found!");

                break;


            case ItemType.Money:

                if (Stats != null)
                {
                    if (Stats.GetStat(StatSystem.StatType.Money) > 0)
                    {
                        item = Instantiate(moneyItemDrop);

                        print("Money was dropped.");

                        Stats.AddStat(StatSystem.StatType.Money, -1);
                    }
                    else
                    {
                        print("Not enough money.");
                    }
                }
                else
                {
                    print("No Stat System found!");
                }

                break;


            case ItemType.Glue:

                if (Stats != null)
                {
                    if (Stats.GetStat(StatSystem.StatType.Glue) > 0)
                    {
                        item = Instantiate(glueItemDrop);

                        print("Glue was dropped.");

                        Stats.AddStat(StatSystem.StatType.Glue, -1);
                    }
                    else
                    {
                        print("Not enough glue.");
                    }
                }
                else
                {
                    print("No Stat System found!");
                }

                break;


            case ItemType.Wallet:

                if (hasWallet)
                {
                    item = Instantiate(walletItemDrop);
                    hasWallet = false;

                    print("Wallet was dropped.");

                    if (Stats != null)
                    {
                        if (Stats.GetStat(StatSystem.StatType.Money) > 5)
                        {
                            Stats.AddStat(StatSystem.StatType.Money, -5);
                        }
                        else
                        {
                            Stats.AddStat(StatSystem.StatType.Money, -Stats.GetStat(StatSystem.StatType.Money));
                        }
                    }
                    else
                    {
                        print("No Stat System found!");
                    }
                }
                else
                {
                    print("Does not have wallet.");
                }

                break;


            case ItemType.GlueGun:

                if (hasGlueGun)
                {
                    item = Instantiate(glueGunItemDrop);
                    hasGlueGun = false;

                    print("Glue gun was dropped.");

                    if (Stats != null)
                    {
                        if (Stats.GetStat(StatSystem.StatType.Glue) > 3)
                        {
                            Stats.AddStat(StatSystem.StatType.Glue, -3);
                        }
                        else
                        {
                            Stats.AddStat(StatSystem.StatType.Glue, -Stats.GetStat(StatSystem.StatType.Glue));
                        }
                    }
                    else
                    {
                        print("No Stat System found!");
                    }
                }
                else
                {
                    print("Does not have glue gun");
                }

                break;


            case ItemType.GravityGun:

                if (hasGravityGun)
                {
                    item = Instantiate(gravityGunItemDrop);
                    hasGravityGun = false;

                    print("Gravity gun was dropped.");
                }
                else
                {
                    print("Does not have gravity gun");
                }

                break;
        }

        // Place Item
        item.transform.position = transform.position;

        if (playerInventory)
        {
            if (transform.parent.GetComponentInChildren<Aiming>().armRotation <= 90 || transform.parent.GetComponentInChildren<Aiming>().armRotation >= 270)
            {
                item.transform.Translate(3, 1, 0);
            }
            else
            {
                item.transform.Translate(-3, 1, 0);
            }
        }

        Destroy(item.GetComponent<Levitate>());
        item.AddComponent<BoxCollider2D>();
        item.AddComponent<Rigidbody2D>();
    }
}

// SOURCE Professor Funky Feeney's Repo