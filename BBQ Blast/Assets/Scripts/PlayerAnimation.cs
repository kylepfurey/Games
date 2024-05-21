using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Player Player;

    // Appearance
    public Material[] TeamColors;
    public MeshRenderer[] TeamRenderers;
    public Material[] HairColors;
    public MeshRenderer[] HairRenderers;
    public Material[] SkinColors;
    public MeshRenderer[] SkinRenderers;

    public int hairColor;
    public int skinColor;

    [SerializeField] private bool hasHelmet;
    [SerializeField] private GameObject helmet;
    [SerializeField] private bool hasHair;
    [SerializeField] private GameObject hair;
    [SerializeField] private bool hasBeard;
    [SerializeField] private GameObject beard;
    [SerializeField] private bool hasMoustache;
    [SerializeField] private GameObject moustache;

    private void Start()
    {
        UpdatePlayerBody(false);
    }

    public void UpdatePlayerBody(bool confirm)    // Call once at the start of a game.
    {
        if (Player)
        {
            SetTeamColor(Player.Settings.freeForAll, Player.team);
        }

        SetHairColor();
        SetSkinColor();
        SetCosmetics(confirm);
    }

    private void SetTeamColor(bool freeForAll, int team)
    {
        if (team > 0 && team < 9)
        {
            if (freeForAll)
            {
                foreach (var Renderer in TeamRenderers)
                {
                    Renderer.material = TeamColors[team + 3];
                }
            }
            else
            {
                foreach (var Renderer in TeamRenderers)
                {
                    Renderer.material = TeamColors[team - 1];
                }
            }
        }
    }

    private void SetHairColor()
    {
        if (hairColor >= 0 && hairColor < HairColors.Length)
        {
            foreach (var Renderer in HairRenderers)
            {
                Renderer.material = HairColors[hairColor];
            }
        }
    }

    private void SetSkinColor()
    {
        if (skinColor >= 0 && skinColor < SkinColors.Length)
        {
            foreach (var Renderer in SkinRenderers)
            {
                Renderer.material = SkinColors[skinColor];
            }
        }
    }

    private void SetCosmetics(bool confirm)
    {
        if (!hasHelmet && confirm)
        {
            Destroy(helmet);
        }

        helmet.gameObject.SetActive(hasHelmet);


        if (!hasHair && confirm)
        {
            Destroy(hair);
        }

        hair.gameObject.SetActive(hasHair);


        if (!hasBeard && confirm)
        {
            Destroy(beard);
        }

        beard.gameObject.SetActive(hasBeard);


        if (!hasMoustache && confirm)
        {
            Destroy(moustache);
        }

        moustache.gameObject.SetActive(hasMoustache);
    }

    // Animations
    // SCRIPT ANIMATIONS USING LERP AND ROTATIONS
}
