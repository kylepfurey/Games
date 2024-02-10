using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private bool hideOnEmpty;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI glueText;

    // Settings Enumerator
    // NOTE: SettingsUI enumeration is used to update the '+' before Money and Glue to indicate Automatic versus Semi Automatic fire and the number of Charges for the Gravity Gun.
    public enum SettingsType
    {
        NULL = 0,
        Automatic = 1,
        SemiAutomatic = 2,
        Charges = 3,
        Length
    }

    private void Start()
    {
        RemoveHUD(StatSystem.StatType.Glue);
        RemoveHUD(StatSystem.StatType.Money);

        if (GameManager.Player != null)
        {
            GameManager.Player.AddCallback(UpdateHUD);
            GameManager.Player.AddCallback(RemoveHUD);
        }

        if (GameManager.Player.GetComponent<StatSystem>() != null)
        {
            GameManager.Player.GetComponent<StatSystem>().AddCallback(UpdateHUD);
        }

        if (GameManager.Player.GetComponent<GlueAndGravityGun>() != null)
        {
            GameManager.Player.GetComponent<GlueAndGravityGun>().AddCallback(UpdateHUD);
        }
    }

    public void UpdateHUD(StatSystem.StatType type, SettingsType settings, int value)
    {
        switch (type)
        {
            default:

                print("No text was found!");

                break;

            case StatSystem.StatType.Health:

                healthText.text = "Health: " + value;

                if (value <= 0 && hideOnEmpty)
                {
                    healthText.text = "";
                }

                break;

            case StatSystem.StatType.Money:

                string charges = "";

                for (int i = 0; i < moneyText.text.Length; i++)
                {
                    if (moneyText.text[i] == '+')
                    {
                        charges += "+";
                    }
                }

                moneyText.text = charges + "Money: " + value;

                if (value <= 0 && hideOnEmpty)
                {
                    moneyText.text = "";
                }

                break;

            case StatSystem.StatType.Glue:

                if (glueText.text.Contains("Glue") == false)
                {
                    // DEFAULT VALUE (AUTO RECOVER GLUE)
                    glueText.text = "+Glue: " + value;
                }
                else if (glueText.text[0] == '+')
                {
                    glueText.text = "+Glue: " + value;
                }
                else
                {
                    glueText.text = "Glue: " + value;
                }

                if (value <= 0 && hideOnEmpty)
                {
                    glueText.text = "";
                }

                break;
        }

        switch (settings)
        {
            case SettingsType.Automatic:

                if (glueText.text[0] != '+')
                {
                    glueText.text = "+" + glueText.text;
                }
                else
                {
                    glueText.text = glueText.text.Remove(0, 1);
                }

                break;

            case SettingsType.SemiAutomatic:

                if (glueText.text[0] == '+')
                {
                    glueText.text = glueText.text.Remove(0, 1);
                }

                break;

            case SettingsType.Charges:

                for (int i = 0; i < moneyText.text.Length; i++)
                {
                    if (moneyText.text[0] == '+')
                    {
                        moneyText.text = moneyText.text.Remove(0, 1);
                    }
                }

                for (int i = 0; i < value; i++)
                {
                    moneyText.text = "+" + moneyText.text;
                }

                break;
        }
    }

    public void RemoveHUD(StatSystem.StatType type)
    {
        switch (type)
        {
            default:

                print("No text was found!");

                break;

            case StatSystem.StatType.Health:

                healthText.text = "";

                break;

            case StatSystem.StatType.Money:

                moneyText.text = "";

                break;

            case StatSystem.StatType.Glue:

                glueText.text = "";

                break;
        }
    }
}