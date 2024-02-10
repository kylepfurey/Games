using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class p_movement : MonoBehaviour
{

    //input
    public PlayerInput input;
    public Animator animation;
    public bool jump_down = false;

    //Player jump strength
    public float jumpStr = 6;
    //player wall jump strength
    public float wallJstr = 10;

    //player gravity multiplier, affects fall speed
    public float gravMult = 2;

    float gravBase = 2;

    //wall jump towards the right
    Vector3 lWJ = new Vector3(0, 1, 0);
    //wall jump towards the left
    Vector3 rWJ = new Vector3(0, 1, 0);
    //Player rigidbody
    [SerializeField] Rigidbody rb;
    public SkinnedMeshRenderer renderer;
    public Material gold;


    //speed variables
    float speedDif;
    float moveSpeed;
    public float maxSpeed = 10;
    public float accelRate = 5;
    public float decelRate = 2.5f;
    public float airAccel = 1;
    public float airDecel = 0;

    //reference to collision scripts
    public wcoll_script wall_script;
    public g_script ground_script;
    public GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (Random.Range(0, 101) == 0)
        {
            renderer.material = gold;
        }

    }
    // Update is called once per frame
    void Update()
    {

        Jump(wall_script.LR);

        Exit();

    }
    private void FixedUpdate()
    {
        Move();

        // Player model
        if (rb.velocity.x > 0.1f)
        {
            model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, 90, model.transform.eulerAngles.z);
            animation.SetBool("Walk", true);
        }
        else if (rb.velocity.x < -0.1f)
        {
            model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, 270, model.transform.eulerAngles.z);
            animation.SetBool("Walk", true);
        }
        else
        {
            animation.SetBool("Walk", false);
        }
    }

    public bool Button(string inputName)
    {
        return 0 < input.actions.FindAction(inputName).ReadValue<float>();
    }

    public float Axis(string inputName)
    {
        return input.actions.FindAction(inputName).ReadValue<float>();
    }

    //Jump function, uses impulse force for best feeling jump
    //it can only activate for one frame, so im confident in putting it in Update() instead of FixedUpdate()
    //also contains the code for wall jumping
    void Jump(bool side)
    {
        if (Button("Jump") == false)
        {
            jump_down = false;
            animation.ResetTrigger("Jump");
        }
        if (Button("Jump") && !jump_down && ground_script.jumping == false)
        {
            rb.velocity = new Vector3(rb.velocity.x, 1, 0);
            rb.AddForce(Vector3.up * jumpStr, ForceMode.Impulse);
            ground_script.jumping = true;
            jump_down = true;
            GameManager.Instance.Audio.Play("Jump");
            animation.SetTrigger("Jump");
        }

        //side = false is left direction, side = true is right direction
        if (Button("Jump") && side == false && !jump_down /*&& ground_script.canWJ == true*/ && ground_script.jumping == true && wall_script.wTouching == true)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(lWJ * jumpStr, ForceMode.Impulse);
            rb.AddForce(wallJstr * Vector3.right, ForceMode.Impulse);
            ground_script.canWJ = false;

            jump_down = true;
            GameManager.Instance.Audio.PlayOnce("Jump");
            animation.SetTrigger("Jump");
        }
        else if (Button("Jump") && side == true && !jump_down /*&& ground_script.canWJ == true*/ && ground_script.jumping == true && wall_script.wTouching == true)
        {
            rb.velocity = Vector3.zero;

            rb.AddForce(rWJ * jumpStr, ForceMode.Impulse);
            rb.AddForce(wallJstr * Vector3.left, ForceMode.Impulse);
            ground_script.canWJ = false;

            jump_down = true;
            GameManager.Instance.Audio.PlayOnce("Jump");
            animation.SetTrigger("Jump");
        }

    }

    //Basic Vector3 movement using velocity, not using forces here so it feel tighter and more controlled
    void Move()
    {
        if (ground_script.jumping == true)
        {
            accelRate = airAccel;
            decelRate = airDecel;
        }
        else
        {
            accelRate = 5;
            decelRate = 0.1f;
        }

        if (Axis("Movement") > 0)
        {
            speedDif = maxSpeed - rb.velocity.x;
            moveSpeed = speedDif * accelRate;
            rb.AddForce(moveSpeed * Vector3.right, ForceMode.Acceleration);

            if (!ground_script.jumping)
            {
                GameManager.Instance.Audio.PlayOnce("Walk");
            }
        }
        else
        if (Axis("Movement") < 0)
        {
            speedDif = maxSpeed - -rb.velocity.x;
            moveSpeed = speedDif * accelRate;
            rb.AddForce(moveSpeed * Vector3.left, ForceMode.Acceleration);

            if (!ground_script.jumping)
            {
                GameManager.Instance.Audio.PlayOnce("Walk");
            }
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), decelRate);
        }


        Gravity(gravMult);

        /*if(rb.velocity.y < 0)
        {
            Gravity(gravMult*1.5f);
        }*/


    }

    void Gravity(float grav)
    {
        gravBase = grav;
        //Source used: https://forum.unity.com/threads/how-to-increase-falling-speed.1380096/
        rb.AddForce(Vector3.down * gravBase * rb.mass, ForceMode.Force);
    }

    private void Exit()
    {
        if (Button("Exit"))
        {
            Application.Quit();
        }
    }

}
