using UnityEngine;

public class Kart3SpeedBoost : MonoBehaviour
{
    public Kart3 Kart3;
    public MeshRenderer meshRenderer1;
    public MeshRenderer meshRenderer2;

    public Material Boost;
    public Material Drift;

    void FixedUpdate()
    {
        if ((Kart3.boost || Kart3.drift) && Kart3.isGrounded && ((Kart3.transform.eulerAngles.x >= 45 && Kart3.transform.eulerAngles.x <= 315) || (Kart3.transform.eulerAngles.z >= 45 && Kart3.transform.eulerAngles.z <= 315) == false))
        {
            meshRenderer1.enabled = true;
            meshRenderer2.enabled = true;

            if (Kart3.drift && (Kart3.boost == false))
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