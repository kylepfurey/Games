using System.Collections;
using UnityEngine;

public class CaveTrigger : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject winCamera;
    [SerializeField] GameObject rollingTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || !GameManager.player.oxygen || !GameManager.player.helmet || !GameManager.player.suit)
            return;

        Debug.Log("You win!");

        StartCoroutine(RestartCoroutine());


        winCamera.SetActive(true);
        rollingTarget.SetActive(true);
        winCamera.transform.position = playerCamera.transform.position;
        winCamera.transform.rotation = playerCamera.transform.rotation;
        winCamera.transform.localScale = playerCamera.transform.localScale;
        GameManager.player.gameObject.SetActive(false);
        playerCamera.SetActive(false);
    }
    private IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
