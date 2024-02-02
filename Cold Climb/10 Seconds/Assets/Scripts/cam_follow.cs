using UnityEngine;

public class cam_follow : MonoBehaviour
{

    [SerializeField] GameObject follow;

    [SerializeField] Transform camTran;

    Vector3 followP = Vector3.zero;

    public float camHeight = 5;
    public float camDistance = -10;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Player
    }

    // Update is called once per frame
    void Update()
    {
        followP = follow.transform.position;
        camTran.position = new Vector3(0, followP.y + camHeight, camDistance);
    }
}
