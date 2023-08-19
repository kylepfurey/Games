using UnityEngine;

public class TestSpeedBoost : MonoBehaviour
{
    public TestKart Kart0;
    public MeshRenderer meshRenderer1;
    public MeshRenderer meshRenderer2;

    public Material Boost;
    public Material Drift;

    void FixedUpdate()
    {
        if ((Kart0.boost || Kart0.drift) && Kart0.isGrounded && ((Kart0.transform.eulerAngles.x >= 45 && Kart0.transform.eulerAngles.x <= 315) || (Kart0.transform.eulerAngles.z >= 45 && Kart0.transform.eulerAngles.z <= 315) == false))
        {
            meshRenderer1.enabled = true;
            meshRenderer2.enabled = true;

            if (Kart0.drift && (Kart0.boost == false))
            {
                meshRenderer1.material = Drift;
                meshRenderer2.material = Drift;
            }
            else
            {
                meshRenderer1.material = Boost;
                meshRenderer2.material = Boost;
            }
        }
        else
        {
            meshRenderer1.enabled = false;
            meshRenderer2.enabled = false;
        }
    }
}