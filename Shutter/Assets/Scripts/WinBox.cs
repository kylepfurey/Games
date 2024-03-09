using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBox : MonoBehaviour
{
    public FadeImages fade = null;
    public float time = 2;

    private void Start()
    {
        gameObject.active = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameManager.Player.active = false;

            AudioManager.Instance.Play("Start", 0.2f);

            Invoke("Load", time);

            fade.Fade();
        }
    }

    private void Load()
    {
        Destroy(AudioManager.Instance.gameObject);

        Destroy(FindObjectOfType<MonsterAI>().gameObject);

        Destroy(FindObjectOfType<MonsterAI>().gameObject);

        if (GameManager.hardmode)
        {
            Destroy(FindObjectOfType<MonsterAI>().gameObject);
        }
        else
        {
            GameManager.hardmode = true;
        }

        SceneManager.LoadScene("Title");
    }
}
