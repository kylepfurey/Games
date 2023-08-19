using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public GameObject WholeMenu;
    public MenuAssets MenuAssets;

    // Kart Data
    public KartSettings KartSettings;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;
    public GameObject CountOff;
    public GameObject[] CountOffActive;

    public Light Sun;

    // Current Displayed Menu and Assets
    public int menu = 0;
    public bool menuMoving;

    // Unloaded Maps
    public GameObject TestMap;
    public GameObject BelknapMap;
    public GameObject ChandlerMap;
    public GameObject FureyMap;

    // Current Selection (one for each player)
    public Vector2[] selection = new Vector2[4];
    public Vector2 selectionSize;

    // Input Variables
    public Vector2[] MOVE = new Vector2[4];
    public int[] MOVECOOLDOWN = new int[4];
    public int moveCooldownTime;
    public bool[] CANMOVE = new bool[4] { true, true, true, true };
    public bool[] SELECT = new bool[4];
    public bool[] SELECTUP = new bool[4] { true, true, true, true };
    public bool[] BACK = new bool[4];
    public bool[] BACKUP = new bool[4] { true, true, true, true };

    public bool[] READY = new bool[4] { false, false, false, false };

    // Menu Logic
    Vector2 previousSelection;
    bool buttonRight;

    void Start()
    {
        GameObject Object0 = GameObject.FindWithTag("Settings");

        if (Object0 != null)
        {
            KartSettings = Object0.GetComponent<KartSettings>();
        }

        KartSettings.Audio.StartSound("Menu Music", 0);
    }

    void FixedUpdate()
    {
        // Initialize Classes
        GameObject Object1 = GameObject.FindWithTag("Kart1");
        if (Object1 != null)
        {
            Kart1 = Object1.GetComponent<Kart1>();
        }

        GameObject Object2 = GameObject.FindWithTag("Kart2");
        if (Object2 != null)
        {
            Kart2 = Object2.GetComponent<Kart2>();
        }

        GameObject Object3 = GameObject.FindWithTag("Kart3");
        if (Object3 != null)
        {
            Kart3 = Object3.GetComponent<Kart3>();
        }

        GameObject Object4 = GameObject.FindWithTag("Kart4");
        if (Object4 != null)
        {
            Kart4 = Object4.GetComponent<Kart4>();
        }


        if (KartSettings.start == false)
        {
            previousSelection = selection[0];

            if (KartSettings.joinedPlayers > 0)
            {
                // Controls
                if (menuMoving == false)
                {
                    MOVE[0] = Kart1.Input.actions.FindAction("Move").ReadValue<Vector2>();
                    SELECT[0] = Button(Kart1.Input.actions.FindAction("Select").ReadValue<float>());
                    BACK[0] = Button(Kart1.Input.actions.FindAction("Back").ReadValue<float>());

                    if (KartSettings.joinedPlayers >= 2 && KartSettings.maxPlayers >= 2)
                    {
                        MOVE[1] = Kart2.Input.actions.FindAction("Move").ReadValue<Vector2>();
                        SELECT[1] = Button(Kart2.Input.actions.FindAction("Select").ReadValue<float>());
                        BACK[1] = Button(Kart2.Input.actions.FindAction("Back").ReadValue<float>());
                    }

                    if (KartSettings.joinedPlayers >= 3 && KartSettings.maxPlayers >= 3)
                    {
                        MOVE[2] = Kart3.Input.actions.FindAction("Move").ReadValue<Vector2>();
                        SELECT[2] = Button(Kart3.Input.actions.FindAction("Select").ReadValue<float>());
                        BACK[2] = Button(Kart3.Input.actions.FindAction("Back").ReadValue<float>());
                    }

                    if (KartSettings.joinedPlayers == 4 && KartSettings.maxPlayers == 4)
                    {
                        MOVE[3] = Kart4.Input.actions.FindAction("Move").ReadValue<Vector2>();
                        SELECT[3] = Button(Kart4.Input.actions.FindAction("Select").ReadValue<float>());
                        BACK[3] = Button(Kart4.Input.actions.FindAction("Back").ReadValue<float>());
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        MOVE[i] = new Vector2(0, 0);
                        SELECT[i] = false;
                        BACK[i] = false;
                    }
                }

                // Input Cooldowns
                for (int i = 0; i < selection.Length; i++)
                {
                    if (MOVECOOLDOWN[i] > 0 && MOVE[i].x == 0 && MOVE[i].y == 0)
                    {
                        MOVECOOLDOWN[i] = 0;
                    }
                    else if (MOVECOOLDOWN[i] > 0)
                    {
                        MOVE[i].x = 0;
                        MOVE[i].y = 0;
                        MOVECOOLDOWN[i] -= 1;
                    }

                    if (SELECT[i] == false)
                    {
                        SELECTUP[i] = true;
                    }

                    if (BACK[i] == false)
                    {
                        BACKUP[i] = true;
                    }
                }

                // Move
                for (int i = 0; i < selection.Length; i++)
                {
                    if (MOVE[i].x > 0 && MOVECOOLDOWN[i] <= 0 && CANMOVE[i])
                    {
                        selection[i].x += 1;
                        MOVECOOLDOWN[i] = moveCooldownTime;

                        KartSettings.Audio.Play("Menu Move");
                    }

                    if (MOVE[i].x < 0 && MOVECOOLDOWN[i] <= 0 && CANMOVE[i])
                    {
                        selection[i].x -= 1;
                        MOVECOOLDOWN[i] = moveCooldownTime;

                        KartSettings.Audio.Play("Menu Move");
                    }

                    if (MOVE[i].y > 0 && MOVECOOLDOWN[i] <= 0 && CANMOVE[i])
                    {
                        selection[i].y -= 1;
                        MOVECOOLDOWN[i] = moveCooldownTime;

                        if (menu != 1)
                        {
                            KartSettings.Audio.Play("Menu Move");
                        }
                    }

                    if (MOVE[i].y < 0 && MOVECOOLDOWN[i] <= 0 && CANMOVE[i])
                    {
                        selection[i].y += 1;
                        MOVECOOLDOWN[i] = moveCooldownTime;

                        if (menu != 1)
                        {
                            KartSettings.Audio.Play("Menu Move");
                        }
                    }
                }
            }

            // Loop Selection if outside of Boundary
            for (int i = 0; i < selection.Length; i++)
            {
                if (selection[i].x > selectionSize.x - 1)
                {
                    selection[i].x = 0;
                }

                if (selection[i].x < 0)
                {
                    selection[i].x = selectionSize.x - 1;
                }

                if (selection[i].y > selectionSize.y - 1)
                {
                    selection[i].y = 0;
                }

                if (selection[i].y < 0)
                {
                    selection[i].y = selectionSize.y - 1;
                }
            }


            // Which Menu you are on

            // Game Start
            if (menu == 5)
            {
                KartSettings.Players.DisableJoining();

                KartSettings.start = true;

                Kart1.jumpUp = false;
                Kart2.jumpUp = false;
                Kart3.jumpUp = false;
                Kart4.jumpUp = false;

                switch (KartSettings.maxPlayers)
                {
                    case 1:
                        Kart1.Camera.on = true;
                        if (KartSettings.night)
                        {
                            Kart1.Light.LightOn = true;
                        }
                        else
                        {
                            Kart1.Light.LightOn = false;
                        }
                        break;

                    case 2:
                        Kart1.Camera.on = true;
                        Kart2.Camera.on = true;
                        if (KartSettings.night)
                        {
                            Kart1.Light.LightOn = true;
                            Kart2.Light.LightOn = true;
                        }
                        else
                        {
                            Kart1.Light.LightOn = false;
                            Kart2.Light.LightOn = false;
                        }
                        break;

                    case 3:
                        Kart1.Camera.on = true;
                        Kart2.Camera.on = true;
                        Kart3.Camera.on = true;
                        if (KartSettings.night)
                        {
                            Kart1.Light.LightOn = true;
                            Kart2.Light.LightOn = true;
                            Kart3.Light.LightOn = true;
                        }
                        else
                        {
                            Kart1.Light.LightOn = false;
                            Kart2.Light.LightOn = false;
                            Kart3.Light.LightOn = false;
                        }
                        break;

                    case 4:
                        Kart1.Camera.on = true;
                        Kart2.Camera.on = true;
                        Kart3.Camera.on = true;
                        Kart4.Camera.on = true;
                        if (KartSettings.night)
                        {
                            Kart1.Light.LightOn = true;
                            Kart2.Light.LightOn = true;
                            Kart3.Light.LightOn = true;
                            Kart4.Light.LightOn = true;
                        }
                        else
                        {
                            Kart1.Light.LightOn = false;
                            Kart2.Light.LightOn = false;
                            Kart3.Light.LightOn = false;
                            Kart4.Light.LightOn = false;
                        }
                        break;
                }

                for (int i = 0; i < selection.Length; i++)
                {
                    CountOffActive[i] = Instantiate(CountOff);

                    for (int j = 0; j < CountOffActive[i].transform.childCount; j++)
                    {
                        foreach (Transform child in CountOffActive[i].transform.GetChild(j))
                        {
                            switch (i)
                            {
                                case 0:
                                    child.gameObject.layer = LayerMask.NameToLayer("Kart1");
                                    break;

                                case 1:
                                    child.gameObject.layer = LayerMask.NameToLayer("Kart2");
                                    break;

                                case 2:
                                    child.gameObject.layer = LayerMask.NameToLayer("Kart3");
                                    break;

                                case 3:
                                    child.gameObject.layer = LayerMask.NameToLayer("Kart4");
                                    break;
                            }
                        }
                    }
                }

                KartSettings.Audio.StopSound("Menu Music");


                // Enable Music
                if (Kart1.Input.actions.FindAction("Light").ReadValue<float>() <= 0)
                {
                    CountOffActive[0].GetComponent<CountOff>().playSounds = true;
                }
                else
                {
                    Kart1.Light.LightOn = !Kart1.Light.LightOn;
                }


                switch (KartSettings.map)
                {
                    case 1:

                        break;

                    case 2:
                        Kart1.transform.position = new Vector3(-271, 1, 125.8f);
                        Kart2.transform.position = new Vector3(-269.45f, 1, 125.8f);
                        Kart3.transform.position = new Vector3(-267.9f, 1, 125.8f);
                        Kart4.transform.position = new Vector3(-266.35f, 1, 125.8f);

                        Kart1.transform.eulerAngles = new Vector3(0, 0, 0);
                        Kart2.transform.eulerAngles = new Vector3(0, 0, 0);
                        Kart3.transform.eulerAngles = new Vector3(0, 0, 0);
                        Kart4.transform.eulerAngles = new Vector3(0, 0, 0);

                        Kart1.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition & ~RigidbodyConstraints.FreezeRotation;
                        Kart2.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition & ~RigidbodyConstraints.FreezeRotation;
                        Kart3.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition & ~RigidbodyConstraints.FreezeRotation;
                        Kart4.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition & ~RigidbodyConstraints.FreezeRotation;

                        for (int i = 0; i < selection.Length; i++)
                        {
                            CountOffActive[i].transform.position = new Vector3(-271 + i * 1.55f, -.8f, 130);
                            CountOffActive[i].transform.eulerAngles = new Vector3(-10, 0, 0);
                        }

                        Destroy(TestMap);
                        Destroy(ChandlerMap);
                        Destroy(FureyMap);
                        break;

                    case 3:
                        Kart1.transform.position = new Vector3(250, 1, 497.6f);
                        Kart2.transform.position = new Vector3(250, 1, 499.2f);
                        Kart3.transform.position = new Vector3(250, 1, 500.8f);
                        Kart4.transform.position = new Vector3(250, 1, 502.4f);

                        Kart1.transform.eulerAngles = new Vector3(0, -90, 0);
                        Kart2.transform.eulerAngles = new Vector3(0, -90, 0);
                        Kart3.transform.eulerAngles = new Vector3(0, -90, 0);
                        Kart4.transform.eulerAngles = new Vector3(0, -90, 0);

                        Kart1.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition & RigidbodyConstraints.FreezeRotation;
                        Kart2.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition & RigidbodyConstraints.FreezeRotation;
                        Kart3.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition & RigidbodyConstraints.FreezeRotation;
                        Kart4.Rigidbody.constraints = ~RigidbodyConstraints.FreezePosition & RigidbodyConstraints.FreezeRotation;

                        for (int i = 0; i < selection.Length; i++)
                        {
                            CountOffActive[i].transform.position = new Vector3(246.12f, -0.94f, 497.6f + i * 1.6f);
                            CountOffActive[i].transform.eulerAngles = new Vector3(-10, -90, 0);
                        }

                        Destroy(TestMap);
                        Destroy(ChandlerMap);
                        Destroy(BelknapMap);
                        break;
                }

                if (KartSettings.night)
                {
                    Sun.transform.eulerAngles = new Vector3(-50, -30, 0);
                    Sun.enabled = false;
                    RenderSettings.ambientIntensity = 0;
                    RenderSettings.reflectionIntensity = 0;
                }
                else
                {
                    Sun.transform.eulerAngles = new Vector3(50, -30, 0);
                    Sun.enabled = true;
                    RenderSettings.ambientIntensity = 1;
                    RenderSettings.reflectionIntensity = 1;
                }

                Destroy(WholeMenu);
            }

            // Controls
            if (menu == 4)
            {
                KartSettings.Players.DisableJoining();

                selectionSize = new Vector2(3, 2);

                CANMOVE[0] = false;
                CANMOVE[1] = false;
                CANMOVE[2] = false;
                CANMOVE[3] = false;

                // Confirm Controls
                for (int i = 0; i < selection.Length; i++)
                {
                    // Ready Up
                    if (SELECT[i] && SELECTUP[i] && READY[i] == false)
                    {
                        READY[i] = true;

                        SELECTUP[i] = false;

                        KartSettings.Audio.Play("Menu Select");
                    }
                    else if (BACK[i] && BACKUP[i] && READY[i] == true)
                    {
                        READY[i] = false;

                        BACKUP[i] = false;

                        KartSettings.Audio.Play("Menu Back");
                    }
                }

                // All Players Ready
                for (int i = 0; i < KartSettings.maxPlayers; i++)
                {
                    if (READY[i] == false)
                    {
                        break;
                    }

                    if (i == KartSettings.maxPlayers - 1)
                    {
                        KartSettings.Audio.Play("Menu Start");
                        Kart1.Audio.Play("321 GO");

                        menu = 5;
                    }
                }

                // Go Back
                if (BACK[0] && BACKUP[0] && READY[0] == false)
                {
                    READY[0] = false;
                    READY[1] = false;
                    READY[2] = false;
                    READY[3] = false;

                    BACKUP[0] = false;

                    KartSettings.Audio.Play("Menu Back");

                    menu = 3;
                }
            }

            // Map Select
            if (menu == 3)
            {
                KartSettings.Players.DisableJoining();

                selectionSize = new Vector2(3, 2);

                CANMOVE[0] = true;
                CANMOVE[1] = false;
                CANMOVE[2] = false;
                CANMOVE[3] = false;


                // Selection Logic
                if (selection[0].x == 0 && selection[0].y == 1)
                {
                    if (previousSelection.x == 1 && previousSelection.y == 1)
                    {
                        selection[0] = new Vector2(0, 0);
                    }
                    else
                    {
                        selection[0] = new Vector2(1, 1);
                    }
                }

                if (selection[0].x == 2 && selection[0].y == 1)
                {
                    if (previousSelection.x == 1 && previousSelection.y == 1)
                    {
                        selection[0] = new Vector2(2, 0);
                    }
                    else
                    {
                        selection[0] = new Vector2(1, 1);
                    }
                }


                // Map 1 Disabled Logic
                if (selection[0].x == 1 && selection[0].y == 1)
                {
                    if (previousSelection.x == 1 && previousSelection.y == 0)
                    {
                        buttonRight = false;
                    }
                    else if (previousSelection.x == 2 && previousSelection.y == 0)
                    {
                        buttonRight = true;
                    }
                }
                if (selection[0].x == 1 && selection[0].y == 0 && previousSelection.x == 1 && previousSelection.y == 1)
                {
                    if (buttonRight)
                    {
                        selection[0] = new Vector2(2, 0);
                    }
                    else
                    {
                        selection[0] = new Vector2(1, 0);
                    }
                }
                if (selection[0].x == 0 && selection[0].y == 0)
                {
                    if (previousSelection.x == 1 && previousSelection.y == 0)
                    {
                        selection[0] = new Vector2(2, 0);
                    }
                    else if (previousSelection.x == 2 && previousSelection.y == 0)
                    {
                        selection[0] = new Vector2(1, 0);
                    }
                    else if (previousSelection.x == 1 && previousSelection.y == 1)
                    {
                        selection[0] = new Vector2(1, 0);
                    }
                }


                // Select Map and Time
                if (SELECT[0] && SELECTUP[0])
                {
                    if (selection[0].x == 0 && selection[0].y == 0)
                    {
                        KartSettings.map = 1;

                        SELECTUP[0] = false;

                        menu = 4;
                    }

                    if (selection[0].x == 1 && selection[0].y == 0)
                    {
                        KartSettings.map = 2;

                        SELECTUP[0] = false;

                        menu = 4;
                    }

                    if (selection[0].x == 2 && selection[0].y == 0)
                    {
                        KartSettings.map = 3;

                        SELECTUP[0] = false;

                        menu = 4;
                    }

                    if (selection[0].x == 1 && selection[0].y == 1)
                    {
                        if (KartSettings.night)
                        {
                            KartSettings.night = false;
                        }
                        else
                        {
                            KartSettings.night = true;
                        }
                    }

                    SELECTUP[0] = false;

                    KartSettings.Audio.Play("Menu Select");
                }


                // Go Back
                if (BACK[0] && BACKUP[0])
                {
                    // Reset Player 1's Selection to their Character
                    switch (KartSettings.character[1])
                    {
                        case "Elizabeth":
                            selection[0] = new Vector2(0, 0);
                            break;

                        case "Kim":
                            selection[0] = new Vector2(1, 0);
                            break;

                        case "Ben":
                            selection[0] = new Vector2(2, 0);
                            break;

                        case "Kelley":
                            selection[0] = new Vector2(0, 1);
                            break;

                        case "Kirsten":
                            selection[0] = new Vector2(1, 1);
                            break;

                        case "Justin":
                            selection[0] = new Vector2(2, 1);
                            break;
                    }

                    KartSettings.character[1] = "none";
                    KartSettings.character[2] = "none";
                    KartSettings.character[3] = "none";
                    KartSettings.character[4] = "none";

                    CANMOVE[0] = true;
                    CANMOVE[1] = true;
                    CANMOVE[2] = true;
                    CANMOVE[3] = true;

                    BACKUP[0] = false;

                    MenuAssets.ignoreCharacterColor = false;

                    KartSettings.Audio.Play("Menu Back");

                    menu = 2;
                }
            }

            // Character Select
            if (menu == 2)
            {
                if (KartSettings.joinedPlayers < KartSettings.maxPlayers)
                {
                    KartSettings.Players.EnableJoining();
                }
                else
                {
                    KartSettings.Players.DisableJoining();
                }

                selectionSize = new Vector2(3, 2);

                // Select Characters
                for (int i = 0; i < selection.Length; i++)
                {
                    if (SELECT[i] && SELECTUP[i] && KartSettings.maxPlayers >= i + 1 && KartSettings.character[i + 1] == "none")
                    {
                        if (selection[i].x == 0 && selection[i].y == 0)
                        {
                            KartSettings.character[i + 1] = "Elizabeth";
                        }

                        if (selection[i].x == 1 && selection[i].y == 0)
                        {
                            KartSettings.character[i + 1] = "Kim";
                        }

                        if (selection[i].x == 2 && selection[i].y == 0)
                        {
                            KartSettings.character[i + 1] = "Ben";
                        }

                        if (selection[i].x == 0 && selection[i].y == 1)
                        {
                            KartSettings.character[i + 1] = "Kelley";
                        }

                        if (selection[i].x == 1 && selection[i].y == 1)
                        {
                            KartSettings.character[i + 1] = "Kirsten";
                        }

                        if (selection[i].x == 2 && selection[i].y == 1)
                        {
                            KartSettings.character[i + 1] = "Justin";
                        }

                        CANMOVE[i] = false;

                        SELECTUP[i] = false;

                        KartSettings.Audio.Play("Menu Select");
                    }

                    // Go Back
                    if (BACK[i] && BACKUP[i] && KartSettings.maxPlayers >= i + 1 && KartSettings.character[i + 1] != "none")
                    {
                        KartSettings.character[i + 1] = "none";

                        CANMOVE[i] = true;

                        BACKUP[i] = false;

                        KartSettings.Audio.Play("Menu Back");
                    }
                }

                // All Players Selected
                for (int i = 0; i < KartSettings.maxPlayers; i++)
                {
                    if (KartSettings.character[i + 1] == "none")
                    {
                        break;
                    }

                    if (i == KartSettings.maxPlayers - 1)
                    {
                        selection[0] = new Vector2(1, 0);

                        KartSettings.night = false;

                        if (Kart1.Input.actions.FindAction("Item").ReadValue<float>() > 0)
                        {
                            MenuAssets.ignoreCharacterColor = true;
                        }

                        menu = 3;
                    }
                }

                // Go Back
                if (BACK[0] && BACKUP[0] && KartSettings.character[1] == "none")
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            // Player Select
            if (menu == 1)
            {
                KartSettings.Players.DisableJoining();

                selectionSize = new Vector2(4, 1);

                CANMOVE[0] = true;
                CANMOVE[1] = false;
                CANMOVE[2] = false;
                CANMOVE[3] = false;

                // Select Number of Players
                if (SELECT[0] && SELECTUP[0])
                {
                    KartSettings.maxPlayers = (int)selection[0].x + 1;

                    selectionSize = new Vector2(3, 2);

                    selection[0] = new Vector2(0, 0);
                    selection[1] = new Vector2(1, 0);
                    selection[2] = new Vector2(2, 0);
                    selection[3] = new Vector2(0, 1);

                    CANMOVE[0] = true;
                    CANMOVE[1] = true;
                    CANMOVE[2] = true;
                    CANMOVE[3] = true;

                    SELECTUP[0] = false;

                    KartSettings.Audio.Play("Menu Select");

                    menu = 2;
                }

                // Go Back
                if (BACK[0] && BACKUP[0])
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            // Title Screen
            if (menu == 0)
            {
                KartSettings.Players.EnableJoining();

                CANMOVE[0] = false;
                CANMOVE[1] = false;
                CANMOVE[2] = false;
                CANMOVE[3] = false;

                // Player 1 Joins
                if (KartSettings.joinedPlayers > 0)
                {
                    SELECTUP[0] = false;
                    BACKUP[0] = false;
                    selection[0] = new Vector2(0, 0);

                    KartSettings.Audio.Play("Menu Select");

                    menu = 1;
                }
            }
        }
    }

    public bool Button(float input)
    {
        if (input > 0)
        {
            return true;
        }

        return false;
    }
}