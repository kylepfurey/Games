using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(GameManager.Player.Camera.transform.position);
    }
}
