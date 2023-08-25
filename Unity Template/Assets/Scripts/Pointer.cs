using UnityEngine;

public class Pointer : MonoBehaviour
{
    public Player Player;
    public GameObject Ray;  // Parent is Pivot
    public GameObject PointerDot;

    public Ray hitscan;
    public RaycastHit hitpoint;
    public float rayMaxDistance;
    public float rayCurrentDistance;

    public Vector3 pointerPosition;

    public bool randomDeviation;
    public Vector2 deviation;

    void Update()
    {
        hitscan = Player.Camera.ScreenPointToRay(new Vector3(Player.Camera.pixelWidth / 2, Player.Camera.pixelHeight / 2 + deviation.y, 0));

        // Deviation
        if (randomDeviation)
        {
            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-Random.Range(-deviation.y, deviation.y), Random.Range(-deviation.x, deviation.x), 0)) * Vector3.forward;
        }
        else
        {
            hitscan.direction = Quaternion.Euler(Player.Camera.transform.eulerAngles + new Vector3(-deviation.y, deviation.x, 0)) * Vector3.forward;
        }

        // Check Hitscan
        if (Physics.Raycast(hitscan, out hitpoint, rayMaxDistance))
        {
            // Calculate Pointer Hit Position
            pointerPosition = hitpoint.point;

            // Ray Distance
            rayCurrentDistance = Mathf.Abs(Vector3.Distance(Player.Camera.transform.position, pointerPosition));
            Ray.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, rayCurrentDistance);
            Ray.transform.localPosition = new Vector3(0, 0, rayCurrentDistance / 2);
            Ray.transform.parent.transform.localEulerAngles = new Vector3(-deviation.y, deviation.x, 0);
        }
        else
        {
            // Calculate Position
            PointerDot.transform.position = Player.Camera.transform.position;
            PointerDot.transform.eulerAngles = new Vector3(Player.Camera.transform.eulerAngles.x + -deviation.y, Player.Camera.transform.eulerAngles.y + deviation.x, 0);
            PointerDot.transform.Translate(new Vector3(0, 0, rayMaxDistance));
            pointerPosition = PointerDot.transform.position;

            // Ray Distance
            Ray.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, rayMaxDistance);
            Ray.transform.localPosition = new Vector3(0, 0, rayMaxDistance / 2);
            Ray.transform.parent.transform.localEulerAngles = new Vector3(-deviation.y, deviation.x, 0);
        }

        // Move Pointer Dot
        PointerDot.transform.position = pointerPosition;
    }
}