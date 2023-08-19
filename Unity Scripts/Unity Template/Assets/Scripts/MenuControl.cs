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
    public int[] MOVECOOLDOWN;
    public int moveCooldownTime;
    public bool[] CANMOVE;
    public bool[] SELECT;
    public bool[] SELECTUP;
    public bool[] BACK;
    public bool[] BACKUP;

    public bool[] READY;

    void Start()
    {
        for (int i = 0; i < selection.Length; i++)
        {
            SELECTUP[i] = true;
            BACKUP[i] = true;
        }
    }

    void FixedUpdate()
    {
        // Controls
        if (menuMoving == false)
        {
            MOVE[0] = Player.Input.actions.FindAction("Move").ReadValue<Vector2>();
            SELECT[0] = Button(Player.Input.actions.FindAction("Select").ReadValue<float>());
            BACK[0] = Button(Player.Input.actions.FindAction("Back").ReadValue<float>());
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
            }

            if (MOVE[i].x < 0 && MOVECOOLDOWN[i] <= 0 && CANMOVE[i])
            {
                selection[i].x -= 1;
                MOVECOOLDOWN[i] = moveCooldownTime;
            }

            if (MOVE[i].y > 0 && MOVECOOLDOWN[i] <= 0 && CANMOVE[i])
            {
                selection[i].y -= 1;
                MOVECOOLDOWN[i] = moveCooldownTime;
            }


            if (MOVE[i].y < 0 && MOVECOOLDOWN[i] <= 0 && CANMOVE[i])
            {
                selection[i].y += 1;
                MOVECOOLDOWN[i] = moveCooldownTime;

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