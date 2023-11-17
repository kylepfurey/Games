using UnityEngine;

public class DeathwaveScript : MonoBehaviour
{
    [SerializeField] private float expansionRate;
    private void Update()
    {
        if (!GameManager.player.lose)
        {
            Expand();
        }
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

        if (other.tag != "Player")
            return;

        GameManager.player.lose = true;
        GameManager.audio.PlayOnce("Game Over");
    }
}
