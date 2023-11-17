using UnityEngine;

public class MainMenuCameraScript : MonoBehaviour
{
    [SerializeField] GameObject pivotTarget;
    [SerializeField] float rotationSpeed = -10;
    private void Update()
    {
        if (pivotTarget)
        {
            transform.RotateAround(
                pivotTarget.transform.position,
                Vector3.up,
                Time.deltaTime
                * rotationSpeed
                );
        }
    }
}
