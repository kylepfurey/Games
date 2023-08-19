using UnityEngine;

public class ItemBox : MonoBehaviour
{
    // Location
    public Vector3 originalPosition;

    // Kart Data
    public TestKart Kart0;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;

    // Color Change
    public Renderer Renderer;
    public Material Red;
    public Material Orange;
    public Material Yellow;
    public Material Green;
    public Material Cyan;
    public Material Blue;
    public Material Purple;
    public Material Pink;

    // Timer Value
    public float timer = 0;

    void Start()
    {
        originalPosition = transform.position;
    }

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


        // Timer Increment
        timer = timer + 1;

        // Timer Reset
        if (timer >= 120)
        {
            timer = 0;
            transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
        }


        // Color Change
        switch (timer)
        {
            case 0:
                Renderer.material = Red;
                break;
            case 15:
                Renderer.material = Orange;
                break;
            case 30:
                Renderer.material = Yellow;
                break;
            case 45:
                Renderer.material = Green;
                break;
            case 60:
                Renderer.material = Cyan;
                break;
            case 75:
                Renderer.material = Blue;
                break;
            case 90:
                Renderer.material = Purple;
                break;
            case 105:
                Renderer.material = Pink;
                break;
        }


        // Levitation Equation: Sin(Timer / speedValue) / heightValue + currentHeight
        transform.position = new Vector3(transform.position.x, Mathf.Sin(timer / 10) / 50 + transform.position.y, transform.position.z);


        // Rotation
        transform.eulerAngles += new Vector3(0, -2, 3);
    }

    // Entering Collision
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        // Kart 0 
        if (other.tag == "Kart0")
        {
            transform.position = new Vector3(transform.position.x, originalPosition.y - 100, transform.position.z);
            timer = 0;

            if (Kart0.item == 0)
            {
                int itemRoll;

                switch (Kart0.KartSettings.maxPlayers)
                {
                    case 1:
                        itemRoll = Random.Range(1, 3);

                        if (itemRoll == 2)
                        {
                            Kart0.item = 4;
                        }
                        else
                        {
                            Kart0.item = itemRoll;
                        }
                        break;

                    case 2:
                        if (Kart0.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart0.item = 2;
                            }
                            else
                            {
                                Kart0.item = itemRoll;
                            }
                        }
                        else if (Kart0.placement == 2)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart0.item = 3;
                            }
                            else
                            {
                                Kart0.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart0.item = Random.Range(1, 5);
                        }
                        break;

                    case 3:
                        if (Kart0.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart0.item = 2;
                            }
                            else
                            {
                                Kart0.item = itemRoll;
                            }
                        }
                        else if (Kart0.placement == 3)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart0.item = 3;
                            }
                            else
                            {
                                Kart0.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart0.item = Random.Range(1, 5);
                        }
                        break;

                    case 4:
                        if (Kart0.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart0.item = 2;
                            }
                            else
                            {
                                Kart0.item = itemRoll;
                            }
                        }
                        else if (Kart0.placement == 4)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart0.item = 3;
                            }
                            else
                            {
                                Kart0.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart0.item = Random.Range(1, 5);
                        }
                        break;
                }
            }
        }

        // Kart 1
        if (other.tag == "Kart1")
        {
            transform.position = new Vector3(transform.position.x, originalPosition.y - 100, transform.position.z);
            timer = 0;

            if (Kart1.item == 0)
            {
                Kart1.Audio.Play("Item");

                int itemRoll;

                switch (Kart1.KartSettings.maxPlayers)
                {
                    case 1:
                        itemRoll = Random.Range(1, 3);

                        if (itemRoll == 2)
                        {
                            Kart1.item = 4;
                        }
                        else
                        {
                            Kart1.item = itemRoll;
                        }
                        break;

                    case 2:
                        if (Kart1.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart1.item = 2;
                            }
                            else
                            {
                                Kart1.item = itemRoll;
                            }
                        }
                        else if (Kart1.placement == 2)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart1.item = 3;
                            }
                            else
                            {
                                Kart1.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart1.item = Random.Range(1, 5);
                        }
                        break;

                    case 3:
                        if (Kart1.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart1.item = 2;
                            }
                            else
                            {
                                Kart1.item = itemRoll;
                            }
                        }
                        else if (Kart1.placement == 3)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart1.item = 3;
                            }
                            else
                            {
                                Kart1.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart1.item = Random.Range(1, 5);
                        }
                        break;

                    case 4:
                        if (Kart1.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart1.item = 2;
                            }
                            else
                            {
                                Kart1.item = itemRoll;
                            }
                        }
                        else if (Kart1.placement == 4)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart1.item = 3;
                            }
                            else
                            {
                                Kart1.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart1.item = Random.Range(1, 5);
                        }
                        break;
                }
            }

            Kart1.Audio.Play("Item Box");
        }

        // Kart 2
        if (other.tag == "Kart2")
        {
            transform.position = new Vector3(transform.position.x, originalPosition.y - 100, transform.position.z);
            timer = 0;

            if (Kart2.item == 0)
            {
                Kart1.Audio.Play("Item");

                int itemRoll;

                switch (Kart2.KartSettings.maxPlayers)
                {
                    case 1:
                        itemRoll = Random.Range(1, 3);

                        if (itemRoll == 2)
                        {
                            Kart2.item = 4;
                        }
                        else
                        {
                            Kart2.item = itemRoll;
                        }
                        break;

                    case 2:
                        if (Kart2.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart2.item = 2;
                            }
                            else
                            {
                                Kart2.item = itemRoll;
                            }
                        }
                        else if (Kart2.placement == 2)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart2.item = 3;
                            }
                            else
                            {
                                Kart2.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart2.item = Random.Range(1, 5);
                        }
                        break;

                    case 3:
                        if (Kart2.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart2.item = 2;
                            }
                            else
                            {
                                Kart2.item = itemRoll;
                            }
                        }
                        else if (Kart2.placement == 3)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart2.item = 3;
                            }
                            else
                            {
                                Kart2.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart2.item = Random.Range(1, 5);
                        }
                        break;

                    case 4:
                        if (Kart2.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart2.item = 2;
                            }
                            else
                            {
                                Kart2.item = itemRoll;
                            }
                        }
                        else if (Kart2.placement == 4)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart2.item = 3;
                            }
                            else
                            {
                                Kart2.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart2.item = Random.Range(1, 5);
                        }
                        break;
                }
            }

            Kart1.Audio.Play("Item Box");
        }

        // Kart 3
        if (other.tag == "Kart3")
        {
            transform.position = new Vector3(transform.position.x, originalPosition.y - 100, transform.position.z);
            timer = 0;

            if (Kart3.item == 0)
            {
                Kart1.Audio.Play("Item");

                int itemRoll;

                switch (Kart3.KartSettings.maxPlayers)
                {
                    case 1:
                        itemRoll = Random.Range(1, 3);

                        if (itemRoll == 2)
                        {
                            Kart3.item = 4;
                        }
                        else
                        {
                            Kart3.item = itemRoll;
                        }
                        break;

                    case 2:
                        if (Kart3.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart3.item = 2;
                            }
                            else
                            {
                                Kart3.item = itemRoll;
                            }
                        }
                        else if (Kart3.placement == 2)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart3.item = 3;
                            }
                            else
                            {
                                Kart3.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart3.item = Random.Range(1, 5);
                        }
                        break;

                    case 3:
                        if (Kart3.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart3.item = 2;
                            }
                            else
                            {
                                Kart3.item = itemRoll;
                            }
                        }
                        else if (Kart3.placement == 3)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart3.item = 3;
                            }
                            else
                            {
                                Kart3.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart3.item = Random.Range(1, 5);
                        }
                        break;

                    case 4:
                        if (Kart3.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart3.item = 2;
                            }
                            else
                            {
                                Kart3.item = itemRoll;
                            }
                        }
                        else if (Kart3.placement == 4)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart3.item = 3;
                            }
                            else
                            {
                                Kart3.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart3.item = Random.Range(1, 5);
                        }
                        break;
                }
            }

            Kart1.Audio.Play("Item Box");
        }

        // Kart 4
        if (other.tag == "Kart4")
        {
            transform.position = new Vector3(transform.position.x, originalPosition.y - 100, transform.position.z);
            timer = 0;

            if (Kart4.item == 0)
            {
                Kart1.Audio.Play("Item");

                int itemRoll;

                switch (Kart4.KartSettings.maxPlayers)
                {
                    case 1:
                        itemRoll = Random.Range(1, 3);

                        if (itemRoll == 2)
                        {
                            Kart4.item = 4;
                        }
                        else
                        {
                            Kart4.item = itemRoll;
                        }
                        break;

                    case 2:
                        if (Kart4.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart4.item = 2;
                            }
                            else
                            {
                                Kart4.item = itemRoll;
                            }
                        }
                        else if (Kart4.placement == 2)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart4.item = 3;
                            }
                            else
                            {
                                Kart4.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart4.item = Random.Range(1, 5);
                        }
                        break;

                    case 3:
                        if (Kart4.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart4.item = 2;
                            }
                            else
                            {
                                Kart4.item = itemRoll;
                            }
                        }
                        else if (Kart4.placement == 3)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart4.item = 3;
                            }
                            else
                            {
                                Kart4.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart4.item = Random.Range(1, 5);
                        }
                        break;

                    case 4:
                        if (Kart4.placement == 1)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 3)
                            {
                                Kart4.item = 2;
                            }
                            else
                            {
                                Kart4.item = itemRoll;
                            }
                        }
                        else if (Kart4.placement == 4)
                        {
                            itemRoll = Random.Range(1, 5);

                            if (itemRoll == 2)
                            {
                                Kart4.item = 3;
                            }
                            else
                            {
                                Kart4.item = itemRoll;
                            }
                        }
                        else
                        {
                            Kart4.item = Random.Range(1, 5);
                        }
                        break;
                }
            }

            Kart1.Audio.Play("Item Box");
        }
    }
}