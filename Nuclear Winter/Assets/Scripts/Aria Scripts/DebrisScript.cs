using UnityEngine;

public class DebrisScript : MonoBehaviour
{
    MeshFilter myFilter
    {
        get
        {
            if (_filter == null)
                _filter = GetComponentInChildren<MeshFilter>();
            return _filter;
        }
    }
    MeshFilter _filter;
    [SerializeField] MeshCollider myCollider;
    [SerializeField] Rigidbody myRigidbody;

    private void Start()
    {
        if (GetComponent<MeshCollider>())
        {
            myCollider = GetComponent<MeshCollider>();

            if (!myCollider.sharedMesh)
            {
                MeshFilter filter = null;

                if (GetComponentInChildren<MeshFilter>())
                {
                    filter = GetComponentInChildren<MeshFilter>();
                }
                else if (GetComponent<MeshFilter>())
                {
                    filter = GetComponent<MeshFilter>();
                }

                if (filter)
                {
                    if (filter.sharedMesh)
                    {
                        myCollider.sharedMesh = filter.sharedMesh;
                    }
                    else
                    {
                        print(name + " broke!");
                        Destroy(gameObject);
                    }
                }
                else
                {
                    print(name + " broke!");
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            print(name + " broke!");
            Destroy(gameObject);
        }

        if (GetComponent<Rigidbody>())
        {
            myRigidbody = GetComponent<Rigidbody>();
        }
        else
        {
            print(name + " broke!");
            Destroy(gameObject);
        }

        //FindReferences();
    }
    private void FindReferences()
    {
        if (myFilter && myCollider)
        {
            myCollider.sharedMesh = myFilter.sharedMesh;
        }
    }
    private void FixedUpdate()
    {
        Gravity();
    }
    private void Gravity()
    {
        if (myRigidbody)
        {
            myRigidbody.velocity +=
                (GameManager.instance.Gravity
                * -1
                * Vector3.up
                * GameManager.timeScale
                );
        }
    }

}
