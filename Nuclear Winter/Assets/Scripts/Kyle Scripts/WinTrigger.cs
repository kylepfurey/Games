using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManager.player.oxygen && GameManager.player.helmet && GameManager.player.suit)
        {
            GameManager.player.winText.start = true;
            GameManager.player.winFade.SetActive(true);
            GameManager.player.start = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
