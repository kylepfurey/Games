using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // References
    public PlayerInput Input;
    public Camera Camera;
    public Rigidbody Rigidbody;

    void Start()
    {
        
    }

    void FixedUpdate()
    {

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