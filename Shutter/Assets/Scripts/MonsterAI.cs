using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent AI = null;

    public Vector2 xBounds = new Vector2(-4.5f, 28.6f);
    public Vector2 yBounds = new Vector2(0, 4.5f);
    public Vector2 zBounds = new Vector2(-26.5f, 18.75f);

    NavMeshPath path = null;

    public MonsterState state = MonsterState.Stalking;

    private bool wait = false;

    public float waitTime = 2;

    public float maxDistance = 0.5f;

    public float directionDistance = 0.75f;

    public Vector3 offset = new Vector3(0, 1, 0);

    public float wanderSpeed = 2;

    public float chaseSpeed = 3.3f;

    private bool piano = false;

    public Animator animator;

    public int number = 1;

    public enum MonsterState { Stalking, Searching, Chasing };

    private void Start()
    {
        AI = GetComponentInChildren<NavMeshAgent>();

        AI.destination = GameManager.Player.transform.position;

        path = AI.path;

        AudioManager.Instance.Play("Spawned");

        AudioManager.Instance.Play("Crawling" + number);

        AudioManager.Instance.Play("Breathing" + number);
    }

    private void Update()
    {
        AudioManager.Instance.Get("Crawling" + number).volume = (1 - Vector3.Distance(transform.position, GameManager.Player.transform.position) / 13) * (AI.velocity.magnitude / 2);

        if (GameManager.lightSwitches > 0)
        {
            AudioManager.Instance.Get("Breathing" + number).volume = (1 - Vector3.Distance(transform.position, GameManager.Player.transform.position) / 13) * (AI.velocity.magnitude / 2);
        }
        else
        {
            AudioManager.Instance.Get("Breathing1").volume = 1;
            AudioManager.Instance.Get("Breathing2").volume = 0;
            AudioManager.Instance.Get("Breathing3").volume = 0;
        }

        if (Search(true))
        {
            AudioManager.Instance.Get("Crawling" + number).volume *= 0.5f;

            if (GameManager.lightSwitches > 0)
            {
                AudioManager.Instance.Get("Breathing" + number).volume *= 0.5f;
            }
        }

        animator.SetFloat("Speed", AI.velocity.magnitude / 2);

        if (!wait)
        {
            switch (state)
            {
                case MonsterState.Stalking:

                    Wait();

                    AI.speed = wanderSpeed;

                    break;

                case MonsterState.Searching:

                    if (Search(false) || DistanceSquared(transform.position, AI.destination) < maxDistance * maxDistance)
                    {
                        BeginWait();
                    }

                    AI.speed = wanderSpeed;

                    break;

                case MonsterState.Chasing:

                    if (DistanceSquared(transform.position, AI.destination) < maxDistance * maxDistance)
                    {
                        EndChase();
                    }
                    else if (Search(false))
                    {
                        Chase();
                    }

                    break;
            }
        }
        else if (Search(false))
        {
            AI.ResetPath();

            Vector3 rotation = transform.eulerAngles;
            transform.LookAt(GameManager.Player.transform.position);
            transform.eulerAngles = new Vector3(rotation.x, transform.eulerAngles.y, rotation.z);

            if (!piano)
            {
                AudioManager.Instance.Play("Piano");

                piano = true;

                Invoke("Piano", 20);
            }
        }
        else
        {
            AI.ResetPath();
        }
    }

    private void Wait()
    {
        wait = true;

        Invoke("EndWait", waitTime);
    }

    private void EndWait()
    {
        wait = false;

        if (Search(false))
        {
            Chase();

            AudioManager.Instance.Play(Random.Range(0, 2) == 0 ? "Spotted" : "Spotted 2");
        }
        else
        {
            Wander();
        }
    }

    private bool Search(bool ignoreFace)
    {
        if (Physics.Raycast(transform.position + offset, Camera.main.transform.position - (transform.position + offset), out RaycastHit hit))
        {
            if (hit.collider.tag == "Player" && (ignoreFace || (IsFacingPlayer(Camera.main.transform.position - (transform.position + offset)) || (DistanceSquared(transform.position, GameManager.Player.transform.position) < 4f * 4f && GameManager.Player.Rigidbody.velocity.magnitude > 0.5f))))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsFacingPlayer(Vector3 hitDirection)
    {
        if (DistanceSquared(transform.forward, hitDirection.normalized) < directionDistance * directionDistance)
        {
            return true;
        }

        return false;
    }

    private void Wander()
    {
        AI.destination = new Vector3(Random.Range(xBounds.x, xBounds.y), Random.Range(yBounds.x, yBounds.y), Random.Range(zBounds.x, zBounds.y));

        state = MonsterState.Searching;
    }

    private void Chase()
    {
        NavMesh.CalculatePath(AI.transform.position, GameManager.Player.transform.position, NavMesh.AllAreas, path);

        AI.SetPath(path);

        AI.speed = chaseSpeed;

        state = MonsterState.Chasing;
    }

    private void EndChase()
    {
        state = MonsterState.Stalking;
    }

    private void BeginWait()
    {
        state = MonsterState.Stalking;
    }

    private void Piano()
    {
        piano = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioManager.Instance.Stop("Crawling1");

            AudioManager.Instance.Stop("Breathing1");

            AudioManager.Instance.Stop("Crawling2");

            AudioManager.Instance.Stop("Breathing2");

            AudioManager.Instance.Stop("Crawling3");

            AudioManager.Instance.Stop("Breathing3");

            GameManager.Instance.death();

            Destroy(gameObject);
        }
    }

    private static float DistanceSquared(Vector3 pointA, Vector3 pointB)
    {
        float xDistance = pointA.x - pointB.x;
        float yDistance = pointA.y - pointB.y;
        float zDistance = pointA.z - pointB.z;

        return xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
    }
}
