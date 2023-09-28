using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;

public class BossdroneDeathLogic : MonoBehaviour
{
    public Health m_Health;
    private bool isDead; //bool to stop repeated deaths when enemy is shot while dead

    public GameObject victorySoundEffect;

    void Start()
    {
        m_Health = GetComponent<Health>();
        m_Health.onDead += DeathListener;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeathListener()
    {
        if (isDead == false)
        {
            isDead = true;
            FpsUIManager.instance.currentState = FpsUIManager.UIState.victory;
            if (ControllerDirtyTag.instance != null)
            {
                ControllerDirtyTag.instance.gameObject.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.None;

            Instantiate(victorySoundEffect, transform.position, transform.rotation, null);
            GameObject.Find("Boss Music").GetComponent<AudioSource>().Stop();

            Destroy(gameObject);
        }

    }
}
