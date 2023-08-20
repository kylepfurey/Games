using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerInput Input;

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