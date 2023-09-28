using UnityEngine;

public class EnviornmentControl : MonoBehaviour
{
    [SerializeField] private int MovementState;
    [SerializeField] private GameObject[] MovementPoints;      // Positions the beam will move to.
    [SerializeField] private float[] MovementSpeed;         // The speed the beam will move to the corresponding state.

    [SerializeField] private Vector3 AutoRotateSpeed;       // The speed the beam will automatically rotate every frame.

    [SerializeField] private int RotationState;             // Which rotation state the beam is currently rotating towards.
    [SerializeField] private GameObject[] RotationPoints;      // Rotations the beam will rotate to.
    [SerializeField] private float[] RotationSpeed;         // The speed the beam will rotate to the corresponding state.

    void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    void Movement()
    {
        // Beam Movement
        if (MovementPoints.Length > 0)
        {
            if (MovementState > MovementPoints.Length - 1)
            {
                MovementState = 0;
            }

            if (Mathf.Abs(Vector3.Distance(transform.position, MovementPoints[MovementState].transform.position)) > MovementSpeed[MovementState])
            {
                transform.position = Vector3.MoveTowards(transform.position, MovementPoints[MovementState].transform.position, MovementSpeed[MovementState]);
            }
            else
            {
                transform.position = MovementPoints[MovementState].transform.position;
                MovementState++;
            }
        }
    }

    void Rotation()
    {
        // Beam Rotation
        if (RotationPoints.Length > 0 && AutoRotateSpeed == Vector3.zero)
        {
            if (RotationState > RotationPoints.Length - 1)
            {
                RotationState = 0;
            }

            if (Mathf.Abs(Vector3.Distance(transform.eulerAngles, RotationPoints[RotationState].transform.eulerAngles)) > RotationSpeed[RotationState])
            {
                transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.eulerAngles, RotationPoints[RotationState].transform.eulerAngles, RotationSpeed[RotationState]));
            }
            else
            {
                transform.rotation = Quaternion.Euler(RotationPoints[RotationState].transform.eulerAngles);
                RotationState++;
            }
        }
        else if (AutoRotateSpeed != Vector3.zero)
        {
            transform.Rotate(AutoRotateSpeed);
        }
    }
}