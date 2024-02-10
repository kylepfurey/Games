using UnityEngine.UI;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private float speed;
    private Image fade;

    void Start()
    {
        fade = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fade.color.a <= speed)
        {
            Destroy(gameObject);
        }

        Vector4 color = fade.color;
        color -= new Vector4(0, 0, 0, speed * Time.deltaTime);
        fade.color = color;
    }
}
