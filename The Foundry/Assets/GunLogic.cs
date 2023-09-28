using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSGame;

public class GunLogic : MonoBehaviour
{
    //vars to influence gun lerprate
    public Transform gunTargetPosition;
    public float moveSpeed = 5f;
    public float rotSpeed = 5f;

    //vars for visual impact
    public GameObject muzzleflashPrefab;
    public GameObject teleportFXPrefab;
    public Transform muzzleflashPosition;
    public Transform recoilPosition;

    private PlayerCombat playerCombat;

    public bool testBool;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gunTargetPosition = GameObject.Find("GunLerpTarget").transform;

        //Get a reference to the shoot script, hook up with callback, and then unparent
        playerCombat = GetComponentInParent<PlayerCombat>();
        playerCombat.fireARCallback += FireVisual;
        playerCombat.fireTeleportCallback += TeleportVisual;
        transform.parent = playerCombat.transform;

        //anim ref
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (testBool)
        {
            testBool = false;
            PlayReloadAnimation();
        }

        //dirtylerp the gun to its intended position
        transform.position = Vector3.Lerp(transform.position, gunTargetPosition.position, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, gunTargetPosition.rotation, Time.deltaTime * rotSpeed);
    }

    //Call this when shooting the gun!
    public void FireVisual()
    {
        transform.position = recoilPosition.position;
        transform.rotation = recoilPosition.rotation;
        GameObject.Instantiate(muzzleflashPrefab, muzzleflashPosition);

    }

    //Call this when teleporting!
    public void TeleportVisual()
    {
        transform.position = recoilPosition.position;
        transform.rotation = recoilPosition.rotation;
        GameObject.Instantiate(teleportFXPrefab, muzzleflashPosition);

    }

    //Call this when reloading!
    public void PlayReloadAnimation()
    {
        animator.SetTrigger("Reload");
    }

    public void StopReloadAnimation()
    {
        animator.ResetTrigger("Reload");
    }

    public void OnDestroy()
    {
        if (playerCombat != null)
        {
            playerCombat.fireARCallback -= FireVisual;
            playerCombat.fireTeleportCallback -= TeleportVisual;
        }
    }
}