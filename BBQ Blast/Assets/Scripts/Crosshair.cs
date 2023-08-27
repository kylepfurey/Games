using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Vector2[] distance;
    public Vector2[] size;
    public Player Player;

    public GameObject Left;
    public GameObject Right;
    public GameObject Up;
    public GameObject Down;
    public GameObject Center;

    void Start()
    {
        gameObject.layer = Player.gameObject.layer;

        foreach (Transform child in transform)
        {
            child.gameObject.layer = Player.gameObject.layer;
            child.GetChild(0).gameObject.layer = Player.gameObject.layer;
        }
    }

    void Update()
    {
        Left.transform.localPosition = new Vector3(-distance[Player.weapon].x, 0, 0);
        Right.transform.localPosition = new Vector3(distance[Player.weapon].x, 0, 0);
        Up.transform.localPosition = new Vector3(0, distance[Player.weapon].y, 0);
        Down.transform.localPosition = new Vector3(0, -distance[Player.weapon].y, 0);

        Left.transform.localScale = new Vector3(size[Player.weapon].x, size[Player.weapon].y, .0001f);
        Right.transform.localScale = new Vector3(size[Player.weapon].x, size[Player.weapon].y, .0001f);
        Up.transform.localScale = new Vector3(size[Player.weapon].y, size[Player.weapon].x, .0001f);
        Down.transform.localScale = new Vector3(size[Player.weapon].y, size[Player.weapon].x, .0001f);
        Center.transform.localScale = new Vector3(size[Player.weapon].y, size[Player.weapon].y, .001f);

        if (distance[Player.weapon].x == 0 && distance[Player.weapon].y == 0)
        {
            Left.transform.localScale = new Vector3(.0001f, .0001f, .0001f);
            Right.transform.localScale = new Vector3(.0001f, .0001f, .0001f);
            Up.transform.localScale = new Vector3(.0001f, .0001f, .0001f);
            Down.transform.localScale = new Vector3(.0001f, .0001f, .0001f);
        }
    }
}