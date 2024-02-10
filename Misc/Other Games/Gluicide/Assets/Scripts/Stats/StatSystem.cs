using UnityEngine;

public class StatSystem : MonoBehaviour
{
    // Player Stats
    private bool playerStats;

    // Stat Enumerator
    public enum StatType
    {
        NULL = 0,
        Glue = 1,
        Money = 2,
        Health = 3,
        Length
    }

    // Resource Values Class
    [System.Serializable]
    public class Resources
    {
        public StatType type;
        public int value;

        public int maximum = 100;
        public int minimum;
    }

    public Resources[] resources;

    // HUD Update Delegate
    // NOTE: My HUD script manages all UI elements through a function, so I only need one event to carry that data to the HUD.
    public delegate void UpdateHUDEvent(StatType type, HUD.SettingsType settings, int value);
    private static UpdateHUDEvent updateHUD;

    // SOURCE Professor Funky Feeney's Repo

    private void Start()
    {
        // Check if Player
        if (GetComponent<Player>() != null)
        {
            playerStats = true;
        }
        else
        {
            playerStats = false;
        }

        // Add Resources List if not present
        if (resources.Length < ((int)StatType.Length) - 1)
        {
            resources = new Resources[((int)StatType.Length) - 1];

            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = new Resources();
                resources[i].type = (StatType)(i + 1);
            }
        }
    }

    public void AddCallback(UpdateHUDEvent function)
    {
        updateHUD += function;
    }

    public int GetStat(StatType type)
    {
        return resources[(int)type - 1].value;
    }

    public void SetStat(StatType type, int newAmount)
    {
        // Get Resource from Specified Type
        Resources resource = resources[(int)type - 1];

        // Modify Resources
        resource.value = newAmount;

        // Minimum and Maximum
        if (resource.value < resource.minimum)
        {
            resource.value = resource.minimum;
        }
        else if (resource.value > resource.maximum)
        {
            resource.value = resource.maximum;
        }
        else
        {
            // Inform user the resource's new value.
            print(type.ToString().ToUpper() + " is now " + resource.value + ".");
        }

        // HUD Update Callback
        if (playerStats)
        {
            updateHUD(type, HUD.SettingsType.NULL, resource.value);
        }
    }

    public void AddStat(StatType type, int amount)
    {
        // Get Resource from Specified Type
        Resources resource = resources[(int)type - 1];

        // Modify Resources
        resource.value += amount;

        // Minimum and Maximum
        if (resource.value < resource.minimum)
        {
            resource.value = resource.minimum;
        }
        else if (resource.value > resource.maximum)
        {
            resource.value = resource.maximum;
        }
        else
        {
            // Inform user the resource's new value.
            print(type.ToString().ToUpper() + " is now " + resource.value + ".");
        }

        // HUD Update Callback
        if (playerStats)
        {
            updateHUD(type, HUD.SettingsType.NULL, resource.value);
        }
    }

    public void RemoveStat(StatType type, int amount)
    {
        amount = -amount;

        // Get Resource from Specified Type
        Resources resource = resources[(int)type - 1];

        // Modify Resources
        resource.value += amount;

        // Minimum and Maximum
        if (resource.value < resource.minimum)
        {
            resource.value = resource.minimum;
        }
        else if (resource.value > resource.maximum)
        {
            resource.value = resource.maximum;
        }
        else
        {
            // Inform user the resource's new value.
            print(type.ToString().ToUpper() + " is now " + resource.value + ".");
        }

        // HUD Update Callback
        if (playerStats)
        {
            updateHUD(type, HUD.SettingsType.NULL, resource.value);
        }
    }

    public int GetMin(StatType type)
    {
        return resources[(int)type - 1].minimum;
    }

    public int GetMax(StatType type)
    {
        return resources[(int)type - 1].maximum;
    }
}