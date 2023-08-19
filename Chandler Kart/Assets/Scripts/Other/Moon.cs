using UnityEngine;

public class Moon : MonoBehaviour
{
    public TestKart Kart0;
    public Kart1 Kart1;
    public Kart2 Kart2;
    public Kart3 Kart3;
    public Kart4 Kart4;

    public Vector3 distance;
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
            if (Kart1.KartSettings.night == false && Kart1.KartSettings.Menu.menu == 5)
            {
                Destroy(gameObject);
            }

            switch (gameObject.layer)
            {
                case 10:
                    transform.position = distance + Kart0.transform.position;
                    break;

                case 6:
                    transform.position = distance + Kart1.transform.position;
                    break;

                case 7:
                    transform.position = distance + Kart2.transform.position;
                    break;

                case 8:
                    transform.position = distance + Kart3.transform.position;
                    break;

                case 9:
                    transform.position = distance + Kart4.transform.position;
                    break;
            }
        }
    }
}