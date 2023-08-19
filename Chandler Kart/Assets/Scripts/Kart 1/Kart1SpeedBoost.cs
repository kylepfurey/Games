using UnityEngine;

public class Kart1SpeedBoost : MonoBehaviour
{
    public Kart1 Kart1;
    public MeshRenderer meshRenderer1;
    public MeshRenderer meshRenderer2;

    public Material Boost;
    public Material Drift;

    void FixedUpdate()
    {
        if ((Kart1.boost || Kart1.drift) && Kart1.isGrounded && ((Kart1.transform.eulerAngles.x >= 45 && Kart1.transform.eulerAngles.x <= 315) || (Kart1.transform.eulerAngles.z >= 45 && Kart1.transform.eulerAngles.z <= 315) == false))
        {
            meshRenderer1.enabled = true;
            meshRenderer2.enabled = true;

            if (Kart1.drift && (Kart1.boost == false))
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