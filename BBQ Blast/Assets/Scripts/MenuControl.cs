using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public Player Player;

    // Current Displayed Menu and Assets
    public int menu = 0;
    public bool menuMoving;

    // Current Selection (one for each player)
    public Vector2[] selection;
    public Vector2 selectionSize;

    // Input Variables
    public Vector2[] MOVE;
    public float[] MOVE_COOLDOWN;
    public float MOVE_COOLDOWN_TIME;
    public bool[] MOVE_ENABLED;
    public bool[] SELECT;
    public bool[] SELECT_UP;
    public bool[] BACK;
    public bool[] BACK_UP;

    public bool[] READY;

    void Start()
    {
        for (int i = 0; i < selection.Length; i++)
        {
            SELECT_UP[i] = true;
            BACK_UP[i] = true;
        }
    }

    void FixedUpdate()
    {
        // Controls
        if (menuMoving == false)
        {
            MOVE[0] = Player.Input.actions.FindAction("Menu Move").ReadValue<Vector2>();
            SELECT[0] = Button(Player.Input.actions.FindAction("Menu Select").ReadValue<float>());
            BACK[0] = Button(Player.Input.actions.FindAction("Menu Back").ReadValue<float>());
        }
        else
        {
            for (int i = 0; i < selection.Length; i++)
            {
                MOVE[i] = new Vector2(0, 0);
                SELECT[i] = false;
                BACK[i] = false;
            }
        }


        // Input Cooldowns
        for (int i = 0; i < selection.Length; i++)
        {
            if (MOVE_COOLDOWN[i] > 0 && MOVE[i].x == 0 && MOVE[i].y == 0)
            {
                MOVE_COOLDOWN[i] = 0;
            }
            else if (MOVE_COOLDOWN[i] > 0)
            {
                MOVE[i].x = 0;
                MOVE[i].y = 0;
                MOVE_COOLDOWN[i] -= 1;
            }

            if (SELECT[i] == false)
            {
                SELECT_UP[i] = true;
            }

            if (BACK[i] == false)
            {
                BACK_UP[i] = true;
            }
        }


        // Move
        for (int i = 0; i < selection.Length; i++)
        {
            if (MOVE[i].x > 0 && MOVE_COOLDOWN[i] <= 0 && MOVE_ENABLED[i])
            {
                selection[i].x += 1;
                MOVE_COOLDOWN[i] = MOVE_COOLDOWN_TIME;
            }

            if (MOVE[i].x < 0 && MOVE_COOLDOWN[i] <= 0 && MOVE_ENABLED[i])
            {
                selection[i].x -= 1;
                MOVE_COOLDOWN[i] = MOVE_COOLDOWN_TIME;
            }

            if (MOVE[i].y > 0 && MOVE_COOLDOWN[i] <= 0 && MOVE_ENABLED[i])
            {
                selection[i].y -= 1;
                MOVE_COOLDOWN[i] = MOVE_COOLDOWN_TIME;
            }


            if (MOVE[i].y < 0 && MOVE_COOLDOWN[i] <= 0 && MOVE_ENABLED[i])
            {
                selection[i].y += 1;
                MOVE_COOLDOWN[i] = MOVE_COOLDOWN_TIME;

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

        if (menu == 0)
        {

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