using UnityEngine;

public class Levitate : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float height = .05f;
    private float timer = 0;

    public void FixedUpdate()
    {
        transform.position += new Vector3(0, Mathf.Sin(timer * speed) * height, 0);
        timer += Time.deltaTime;
    }
}