using UnityEngine;

public class Kart2SpeedBoost : MonoBehaviour
{
    public Kart2 Kart2;
    public MeshRenderer meshRenderer1;
    public MeshRenderer meshRenderer2;

    public Material Boost;
    public Material Drift;

    void FixedUpdate()
    {
        if ((Kart2.boost || Kart2.drift) && Kart2.isGrounded && ((Kart2.transform.eulerAngles.x >= 45 && Kart2.transform.eulerAngles.x <= 315) || (Kart2.transform.eulerAngles.z >= 45 && Kart2.transform.eulerAngles.z <= 315) == false))
        {
            meshRenderer1.enabled = true;
            meshRenderer2.enabled = true;

            if (Kart2.drift && (Kart2.boost == false))
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