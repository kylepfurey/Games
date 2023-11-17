using UnityEngine;

public class HeatwaveScript : MonoBehaviour
{
    [SerializeField] private float expansionRate;
    private void Update()
    {
        Expand();
    }
    private void Expand()
    {
        transform.localScale +=
            Vector3.one
            * expansionRate
            * GameManager.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        IceScript ice = other.gameObject.GetComponent<IceScript>();
        if (ice == null)
            return;

        ice.Melt();
    }
}
