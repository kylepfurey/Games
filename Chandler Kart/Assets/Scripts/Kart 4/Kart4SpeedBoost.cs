using UnityEngine;

public class Kart4SpeedBoost : MonoBehaviour
{
    public Kart4 Kart4;
    public MeshRenderer meshRenderer1;
    public MeshRenderer meshRenderer2;

    public Material Boost;
    public Material Drift;

    void FixedUpdate()
    {
        if ((Kart4.boost || Kart4.drift) && Kart4.isGrounded && ((Kart4.transform.eulerAngles.x >= 45 && Kart4.transform.eulerAngles.x <= 315) || (Kart4.transform.eulerAngles.z >= 45 && Kart4.transform.eulerAngles.z <= 315) == false))
        {
            meshRenderer1.enabled = true;
            meshRenderer2.enabled = true;

            if (Kart4.drift && (Kart4.boost == false))
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