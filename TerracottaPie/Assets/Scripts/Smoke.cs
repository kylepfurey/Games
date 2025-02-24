using System.Threading.Tasks;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public float delay = 35;
    public float speed = 1;
    private bool start = false;

    private async void Start()
    {
        await Task.Delay((int)(delay * 1000));

        start = true;
    }

    private void Update()
    {
        if (start)
        {
            Vector3 pos = transform.position;

            pos.y += speed * Time.deltaTime;

            transform.position = pos;
        }
    }
}
