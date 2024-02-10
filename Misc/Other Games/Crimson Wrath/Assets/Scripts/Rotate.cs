using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 speed;

    private void Update()
    {
        transform.eulerAngles += speed * Time.deltaTime;
    }
}
