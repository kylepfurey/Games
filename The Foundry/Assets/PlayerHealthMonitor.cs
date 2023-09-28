using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;
public class PlayerHealthMonitor : MonoBehaviour
{
    public Health m_Health;
    private bool isDead; //bool to stop repeated deaths when enemy is shot while dead

    private GunLogic gun;
    // Start is called before the first frame update
    void Start()
    {
        m_Health = GetComponent<Health>();
        m_Health.onDead += DeathListener;
    }

    private void Awake()
    {
        gun = GetComponentInChildren<GunLogic>();
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
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script != this)
                {
                    script.enabled = false;
                }
            }

            if (ControllerDirtyTag.instance != null)
            {
                ControllerDirtyTag.instance.gameObject.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.None;
            FpsUIManager.instance.currentState = FpsUIManager.UIState.dead;

            //KO the player lol
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.drag = 15f;
            rb.useGravity = true;
            rb.mass = 20f;
            rb.angularDrag = 2f;
            rb.velocity = -transform.forward;
            rb.angularVelocity = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));

            if (gun != null)
            {
                gun.gunTargetPosition.parent = null;
            }

            gameObject.GetComponent<PlayerCombat>().fireInterval = 100f;

        }

    }

    //private void StartDea
}
