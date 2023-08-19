using UnityEngine;

public class ItemBanana : MonoBehaviour
{
    // Rigidbody for Gravity
    public Rigidbody Rigidbody;

    // Kart Data
    public TestKart Kart0;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;

    public int followKart;
    public bool placed = false;

    public bool good = false;

    void FixedUpdate()
    {
        // Initialize Classes
        GameObject Object0 = GameObject.FindWithTag("Kart0");
        if (Object0 != null)
        {
            Kart0 = Object0.GetComponent<TestKart>();
        }

        GameObject Object1 = GameObject.FindWithTag("Kart1");
        if (Object1 != null)
        {
            Kart1 = Object1.GetComponent<Kart1>();
        }

        GameObject Object2 = GameObject.FindWithTag("Kart2");
        if (Object2 != null)
        {
            Kart2 = Object2.GetComponent<Kart2>();
        }

        GameObject Object3 = GameObject.FindWithTag("Kart3");
        if (Object3 != null)
        {
            Kart3 = Object3.GetComponent<Kart3>();
        }

        GameObject Object4 = GameObject.FindWithTag("Kart4");
        if (Object4 != null)
        {
            Kart4 = Object4.GetComponent<Kart4>();
        }

        if (Object0 != null && Object1 != null && Object2 != null && Object3 != null && Object4 != null)
        {
            good = true;
        }


        if (good)
        {
            // Spawned by Kart 0
            if (Mathf.Abs(Vector3.Distance(transform.position, Kart0.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart1.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart0.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart2.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart0.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart3.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart0.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart4.transform.position)))
            {
                followKart = 0;
            }

            // Spawned by Kart 1
            if (Mathf.Abs(Vector3.Distance(transform.position, Kart1.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart0.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart1.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart2.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart1.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart3.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart1.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart4.transform.position)))
            {
                followKart = 1;
            }

            // Spawned by Kart 2
            if (Mathf.Abs(Vector3.Distance(transform.position, Kart2.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart1.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart2.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart0.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart2.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart3.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart2.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart4.transform.position)))
            {
                followKart = 2;
            }

            // Spawned by Kart 3
            if (Mathf.Abs(Vector3.Distance(transform.position, Kart3.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart1.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart3.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart2.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart3.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart0.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart3.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart4.transform.position)))
            {
                followKart = 3;
            }

            // Spawned by Kart 4
            if (Mathf.Abs(Vector3.Distance(transform.position, Kart4.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart1.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart4.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart2.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart4.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart3.transform.position)) &&
                Mathf.Abs(Vector3.Distance(transform.position, Kart4.transform.position)) < Mathf.Abs(Vector3.Distance(transform.position, Kart0.transform.position)))
            {
                followKart = 4;
            }


            if (placed == false)
            {
                // Spawned by Kart 0
                if (followKart == 0)
                {
                    if (Kart0.bananaState == 1)
                    {
                        transform.position = Kart0.position;
                        transform.eulerAngles = Kart0.rotation;
                    }
                    else
                    {
                        // Flipped Over
                        if ((transform.eulerAngles.x >= 45 && transform.eulerAngles.x <= 315) || (transform.eulerAngles.z >= 45 && transform.eulerAngles.z <= 315))
                        {
                            transform.eulerAngles = new Vector3(0, Kart0.rotation.y, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector3(Kart0.rotation.x, Kart0.rotation.y, Kart0.rotation.z);
                        }

                        Rigidbody.useGravity = true;
                        Rigidbody.constraints = ~RigidbodyConstraints.FreezePositionY;

                        placed = true;
                    }
                }

                // Spawned by Kart 1
                if (followKart == 1)
                {
                    if (Kart1.bananaState == 1)
                    {
                        transform.position = Kart1.position;
                        transform.eulerAngles = Kart1.rotation;
                    }
                    else
                    {
                        // Flipped Over
                        if ((transform.eulerAngles.x >= 45 && transform.eulerAngles.x <= 315) || (transform.eulerAngles.z >= 45 && transform.eulerAngles.z <= 315))
                        {
                            transform.eulerAngles = new Vector3(0, Kart1.rotation.y, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector3(Kart1.rotation.x, Kart1.rotation.y, Kart1.rotation.z);
                        }

                        Rigidbody.useGravity = true;
                        Rigidbody.constraints = ~RigidbodyConstraints.FreezePositionY;

                        placed = true;
                    }
                }

                // Spawned by Kart 2
                if (followKart == 2)
                {
                    if (Kart2.bananaState == 1)
                    {
                        transform.position = Kart2.position;
                        transform.eulerAngles = Kart2.rotation;
                    }
                    else
                    {
                        // Flipped Over
                        if ((transform.eulerAngles.x >= 45 && transform.eulerAngles.x <= 315) || (transform.eulerAngles.z >= 45 && transform.eulerAngles.z <= 315))
                        {
                            transform.eulerAngles = new Vector3(0, Kart2.rotation.y, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector3(Kart2.rotation.x, Kart2.rotation.y, Kart2.rotation.z);
                        }

                        Rigidbody.useGravity = true;
                        Rigidbody.constraints = ~RigidbodyConstraints.FreezePositionY;

                        placed = true;
                    }
                }

                // Spawned by Kart 3
                if (followKart == 3)
                {
                    if (Kart3.bananaState == 1)
                    {
                        transform.position = Kart3.position;
                        transform.eulerAngles = Kart3.rotation;
                    }
                    else
                    {
                        // Flipped Over
                        if ((transform.eulerAngles.x >= 45 && transform.eulerAngles.x <= 315) || (transform.eulerAngles.z >= 45 && transform.eulerAngles.z <= 315))
                        {
                            transform.eulerAngles = new Vector3(0, Kart3.rotation.y, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector3(Kart3.rotation.x, Kart3.rotation.y, Kart3.rotation.z);
                        }

                        Rigidbody.useGravity = true;
                        Rigidbody.constraints = ~RigidbodyConstraints.FreezePositionY;

                        placed = true;
                    }
                }

                // Spawned by Kart 4
                if (followKart == 4)
                {
                    if (Kart4.bananaState == 1)
                    {
                        transform.position = Kart4.position;
                        transform.eulerAngles = Kart4.rotation;
                    }
                    else
                    {
                        // Flipped Over
                        if ((transform.eulerAngles.x >= 45 && transform.eulerAngles.x <= 315) || (transform.eulerAngles.z >= 45 && transform.eulerAngles.z <= 315))
                        {
                            transform.eulerAngles = new Vector3(0, Kart4.rotation.y, 0);
                        }
                        else
                        {
                            transform.eulerAngles = new Vector3(Kart4.rotation.x, Kart4.rotation.y, Kart4.rotation.z);
                        }

                        Rigidbody.useGravity = true;
                        Rigidbody.constraints = ~RigidbodyConstraints.FreezePositionY;

                        placed = true;
                    }
                }
            }
        }
    }
}