using UnityEngine;

public class BurnMark : MonoBehaviour
{
    public float age;
    public float timer;
    public SpriteRenderer Texture;
    public bool fade = true;

    void Start()
    {
        transform.Translate(new Vector3(0,0,Random.Range(0, 0.05f)));    
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > age)
        {
            Destroy(gameObject);
        }


        if (timer > (age / 2) && fade)
        {
            Texture.color = new Vector4(Texture.color.r, Texture.color.g, Texture.color.b, (age - timer) / (age / 2));
        }
    }
}