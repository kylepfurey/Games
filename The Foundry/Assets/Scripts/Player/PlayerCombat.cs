using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FPSGame
{
    public enum PlayerCombatMode
    {
        AssaultRifle = 0,
        Teleport = 1,
    }

    public class PlayerCombat : MonoBehaviour
    {
        public PlayerController playerController;
        public PlayerMovement playerMovement;
        public PlayerCamera playerCamera;

        private PlayerCombatMode playerCombatMode = PlayerCombatMode.AssaultRifle;
        public PlayerCombatMode PlayerCombatMode => playerCombatMode;

        [Tooltip("The transform where projectiles are spawned.")]
        public Transform muzzleTransform;

        [Header("Assault Rifle Mode")]
        public GameObject arProjectilePrefab;

        public float cooldown;

        public float bonusDamage = 0.0f;

        [Tooltip("Inverse of fire rate")]
        public float fireInterval = 3.0f;

        private Coroutine fireCoroutine = null;

        [Header("Teleport Mode")]
        public float maxTeleportDistance = 25.0f;

        public float teleportCooldown = 3.0f;

        [FormerlySerializedAs("projectilePrefab")]
        public GameObject teleportProjectilePrefab;

        private bool canFireTeleportProjectile = false;

        private GameObject latestFiredProjectile = null;
        public bool HasTeleportProjectile => latestFiredProjectile;

        private Vector3 teleportTargetPosition;
        private RaycastHit hitInfo;

        private float currentTeleportCooldown = 0.0f;
        public float CurrentTeleportCooldown => currentTeleportCooldown;

        [Header("Flashlight")]

        [SerializeField]
        private GameObject flashLight;
        public GameObject FlashLight => flashLight;

        public delegate void OnFlashLightUpdated(bool isOn);
        public event OnFlashLightUpdated onFlashLightUpdated;

        [Header("Weapon")]

        public GameObject currentGun;
        public RawImage cursor;
        public RawImage cursorShadow;
        public TextMeshProUGUI ammo;
        public TextMeshProUGUI ammoShadow;
        public TextMeshProUGUI reserve;
        public TextMeshProUGUI reserveShadow;
        public Image scope;

        // Gun Variables
        public int Weapon;
        public int lastWeapon;
        public bool[] unlocked;
        public GameObject[] Gun;
        public GameObject[] Projectile;

        // Shooting Variables
        public float[] fireSpeed;
        public float[] tapFireSpeed;
        public int[] numberOfShots;
        public Vector2[] bulletSpread;

        // Reload Variables
        public int[] currentAmmo;
        public int[] maxAmmo;
        public int[] ammoReserveType;
        public int[] ammoReserve;
        public float[] reloadTimeStart;
        public float[] reloadAmmoRestockTime;
        public float reloadTimer;

        // Burst Variables
        public float[] burstTimeStart;
        public float burstTimer;

        void Start()
        {
            if (!playerController)
            {
                playerController = FindObjectOfType<PlayerController>();
            }

            if (!playerMovement)
            {
                playerMovement = GetComponent<PlayerMovement>();
            }

            if (!playerCamera)
            {
                playerCamera = playerController.CharacterCamera;
            }
        }

        private void OnDrawGizmos()
        {
            if (muzzleTransform)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(muzzleTransform.position, 0.3f);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(muzzleTransform.position, muzzleTransform.position + muzzleTransform.forward * maxTeleportDistance);
            }

            // Draw the teleport position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(teleportTargetPosition, 0.3f);
        }

        private void Update()
        {
            if (currentTeleportCooldown > 0.0f)
            {
                currentTeleportCooldown -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            UpdateTeleportTargetPosition();
        }

        private void SwitchCombatMode(PlayerCombatMode playerCombatMode)
        {
            // Don't switch weapon mode if the new and old modes are the same.
            if (playerCombatMode == this.playerCombatMode)
            {
                return;
            }

            StopFiring();
            this.playerCombatMode = playerCombatMode;
        }

        private void UpdateTeleportTargetPosition()
        {
            RaycastHit hitInfo;
            float rayCastLength = maxTeleportDistance;
            if (Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out hitInfo, rayCastLength, -1, QueryTriggerInteraction.Ignore))
            {
                //teleportTargetPosition = hitInfo.point;
                teleportTargetPosition = hitInfo.point + hitInfo.normal * 0.5f;
            }
            else
            {
                teleportTargetPosition = muzzleTransform.position + muzzleTransform.forward * rayCastLength;
            }
        }

        public void SetPlayerInput(ref PlayerCharacterInputs playerInput)
        {
            // Weapon Switch Mode.
            if (playerInput.SwitchAutomatic)
            {
                SwitchCombatMode(PlayerCombatMode.AssaultRifle);
            }
            else if (playerInput.SwitchTeleport)
            {
                SwitchCombatMode(PlayerCombatMode.Teleport);
            }

            // Weapon Appearance.
            if (lastWeapon != Weapon)
            {
                switch (Weapon)
                {
                    case 0:
                        if (currentGun != null)
                        {
                            Destroy(currentGun);
                        }

                        currentGun = null;

                        cursor.enabled = false;
                        cursorShadow.enabled = false;
                        ammo.enabled = false;
                        ammoShadow.enabled = false;
                        reserve.enabled = false;
                        reserveShadow.enabled = false;
                        break;

                    case 1:
                        if (currentGun != null)
                        {
                            Destroy(currentGun);
                        }

                        currentGun = Instantiate(Gun[Weapon]);
                        currentGun.transform.SetParent(transform, false);
                        flashLight = currentGun.transform.GetChild(0).gameObject;

                        cursor.enabled = true;
                        cursor.color = new Vector4(255, 150, 0, 255) / 255;
                        cursorShadow.enabled = true;
                        ammo.enabled = true;
                        ammoShadow.enabled = true;
                        reserve.enabled = true;
                        reserveShadow.enabled = true;
                        reserve.color = cursor.color;
                        break;

                    case 2:
                        if (currentGun != null)
                        {
                            Destroy(currentGun);
                        }

                        currentGun = Instantiate(Gun[Weapon]);
                        currentGun.transform.SetParent(transform, false);
                        flashLight = currentGun.transform.GetChild(0).gameObject;

                        cursor.enabled = true;
                        cursor.color = new Vector4(26, 255, 0, 255) / 255;
                        cursorShadow.enabled = true;
                        ammo.enabled = true;
                        ammoShadow.enabled = true;
                        reserve.enabled = true;
                        reserveShadow.enabled = true;
                        reserve.color = cursor.color;
                        break;

                    case 3:
                        if (currentGun != null)
                        {
                            Destroy(currentGun);
                        }

                        currentGun = Instantiate(Gun[Weapon]);
                        currentGun.transform.SetParent(transform, false);
                        flashLight = currentGun.transform.GetChild(0).gameObject;

                        cursor.enabled = true;
                        cursor.color = new Vector4(255, 13, 0, 255) / 255;
                        cursorShadow.enabled = true;
                        ammo.enabled = true;
                        ammoShadow.enabled = true;
                        reserve.enabled = true;
                        reserveShadow.enabled = true;
                        reserve.color = cursor.color;
                        break;

                    case 4:
                        if (currentGun != null)
                        {
                            Destroy(currentGun);
                        }

                        currentGun = Instantiate(Gun[Weapon]);
                        currentGun.transform.SetParent(transform, false);
                        flashLight = currentGun.transform.GetChild(0).gameObject;

                        cursor.enabled = true;
                        cursor.color = new Vector4(0, 160, 255, 255) / 255;
                        cursorShadow.enabled = true;
                        ammo.enabled = true;
                        ammoShadow.enabled = true;
                        reserve.enabled = true;
                        reserveShadow.enabled = true;
                        reserve.color = cursor.color;
                        break;

                    case 5:
                        if (currentGun != null)
                        {
                            Destroy(currentGun);
                        }

                        currentGun = Instantiate(Gun[Weapon]);
                        currentGun.transform.SetParent(transform, false);
                        flashLight = currentGun.transform.GetChild(0).gameObject;

                        cursor.enabled = true;
                        cursor.color = new Vector4(160, 0, 255, 255) / 255;
                        cursorShadow.enabled = true;
                        ammo.enabled = true;
                        ammoShadow.enabled = true;
                        reserve.enabled = true;
                        reserveShadow.enabled = true;
                        reserve.color = cursor.color;
                        break;
                }

                lastWeapon = Weapon;
            }

            // Ammo Text.
            ammo.text = currentAmmo[Weapon].ToString();
            ammoShadow.text = currentAmmo[Weapon].ToString();
            reserve.text = ammoReserve[ammoReserveType[Weapon]].ToString();
            reserveShadow.text = ammoReserve[ammoReserveType[Weapon]].ToString();

            if (Weapon == 1)
            {
                reserve.text = "∞";
                reserveShadow.text = "∞";
            }

            // Firing.
            if (!playerInput.Fire && cooldown > tapFireSpeed[Weapon])
            {
                cooldown = tapFireSpeed[Weapon];
            }

            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                cooldown = 0;
            }

            if (playerInput.Fire && cooldown <= 0 && Weapon != 0 && (/*(reloadTimer > reloadTimeStart[Weapon] - reloadAmmoRestockTime[Weapon]) || */reloadTimer <= 0) && burstTimer <= 0 && currentAmmo[Weapon] > 0)
            {
                if (reloadTimer > 0)
                {
                    lastWeapon = 0;
                }

                // Fire to cancel reload
                if (reloadTimer > reloadTimeStart[Weapon] - reloadAmmoRestockTime[Weapon] && reloadTimer != 0)
                {
                    reloadTimer = 0;

                    GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }

                if (transform.GetComponent<Health>().godMode)
                {
                    cooldown = 0.02f + Time.deltaTime;
                }
                else
                {
                    cooldown = fireSpeed[Weapon];
                    currentAmmo[Weapon] -= 1;
                }

                if (numberOfShots[Weapon] > 1)
                {
                    for (int currentBullet = 0; currentBullet < numberOfShots[Weapon]; currentBullet++)
                    {
                        StartFiring();
                        StopFiring();
                    }
                }
                else
                {
                    StartFiring();
                    StopFiring();
                }

                if (burstTimeStart[Weapon] > 0)
                {
                    burstTimer = burstTimeStart[Weapon];
                }
            }
            else if (Weapon != 0 && (playerInput.Fire || playerController.autoReload) && cooldown <= 0 && currentAmmo[Weapon] <= 0 && reloadTimer <= 0)
            {
                if (ammoReserve[ammoReserveType[Weapon]] > 0)
                {
                    reloadTimer = reloadTimeStart[Weapon];
                }
            }

            // Reloading.
            if (ammoReserveType[Weapon] == 0)
            {
                // Pistol reserve is infinite.
                ammoReserve[ammoReserveType[Weapon]] = 999;
            }

            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;

                if (Weapon != 0)
                {
                    GetComponentInChildren<GunLogic>().PlayReloadAnimation();
                }

                if (reloadTimer == 0)
                {
                    GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }
            else if (reloadTimer < 0)
            {
                reloadTimer = 0;

                if (Weapon != 0)
                {
                    GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }

            if (reloadTimer != 0 && reloadTimer < reloadTimeStart[Weapon] - reloadAmmoRestockTime[Weapon] && currentAmmo[Weapon] < maxAmmo[Weapon])
            {
                int max = maxAmmo[Weapon];
                int current = currentAmmo[Weapon];

                for (int i = 0; i < max - current; i++)
                {
                    if (ammoReserve[ammoReserveType[Weapon]] <= 0)
                    {
                        break;
                    }

                    currentAmmo[Weapon]++;
                    ammoReserve[ammoReserveType[Weapon]]--;
                }
            }

            // Burst Fire.
            if (burstTimer > 0)
            {
                if (burstTimer > 0)
                {
                    burstTimer -= Time.deltaTime;
                }
                else
                {
                    burstTimer = 0;
                }

                if (burstTimer <= 0 && currentAmmo[Weapon] > 0)
                {
                    StartFiring();
                    StopFiring();

                    currentAmmo[Weapon] -= 1;
                }
            }

            // Teleporting and Special Abilities.
            playerCamera.Camera.fieldOfView = 75;
            scope.enabled = false;

            if (Weapon == 5)
            {
                // Scoping.
                if (playerController.RMB)
                {
                    playerCamera.Camera.fieldOfView = 10;
                    scope.enabled = true;

                    GetComponentInChildren<GunBeam>().On = false;
                }
                else
                {
                    playerCamera.Camera.fieldOfView = 75;
                    scope.enabled = false;

                    GetComponentInChildren<GunBeam>().On = true;
                }
            }
            else if (Weapon != 0)
            {
                if (playerInput.Teleport)
                {
                    Teleport();
                }
            }

            // Destroy teleport projectile.
            if (playerInput.DestroyTeleportProjectile)
            {
                if (latestFiredProjectile)
                {
                    Destroy(latestFiredProjectile);
                }

                latestFiredProjectile = null;
            }

            // Toggle flash light.
            if (playerInput.ToggleFlashLight)
            {
                flashLight.SetActive(!flashLight.activeSelf);
                onFlashLightUpdated?.Invoke(flashLight.activeSelf);
            }
        }

        private void StartFiring()
        {
            switch (playerCombatMode)
            {
                case PlayerCombatMode.AssaultRifle:
                    if (fireCoroutine == null)
                    {
                        fireCoroutine = StartCoroutine(FireLoop());
                    }
                    break;

                case PlayerCombatMode.Teleport:
                    if (canFireTeleportProjectile && currentTeleportCooldown <= 0.0f)
                    {
                        FireTeleportProjectile();
                        canFireTeleportProjectile = false;
                    }
                    break;
            }
        }

        private void StopFiring()
        {
            switch (playerCombatMode)
            {
                case PlayerCombatMode.AssaultRifle:
                    if (fireCoroutine != null)
                    {
                        StopCoroutine(fireCoroutine);
                        fireCoroutine = null;
                    }
                    break;

                case PlayerCombatMode.Teleport:
                    canFireTeleportProjectile = true;
                    break;
            }
        }

        private IEnumerator FireLoop()
        {
            while (true)
            {
                FireARProjectile();
                yield return new WaitForSeconds(fireInterval);
            }
        }


        public delegate void FireARCallback(); //Added a callback here for the gun logic to listen to
        public FireARCallback fireARCallback;
        private void FireARProjectile()
        {
            Transform muzzleTransform = GetMuzzleTransform();

            GameObject projectileObject = Instantiate(Projectile[Weapon], muzzleTransform.position, Quaternion.Euler(muzzleTransform.eulerAngles
                + new Vector3(UnityEngine.Random.Range(-bulletSpread[Weapon].y, bulletSpread[Weapon].y), UnityEngine.Random.Range(-bulletSpread[Weapon].x, bulletSpread[Weapon].x), 0)));

            fireARCallback();
        }

        public delegate void FireTeleportCallback(); //Added a callback here for the gun logic to listen to
        public FireTeleportCallback fireTeleportCallback;
        private void FireTeleportProjectile()
        {
            Transform muzzleTransform = GetMuzzleTransform();

            GameObject projectileObject = Instantiate(teleportProjectilePrefab, muzzleTransform.position, muzzleTransform.rotation, null);

            if (projectileObject)
            {
                TeleportProjectile projectile = projectileObject.GetComponent<TeleportProjectile>();
                if (projectile)
                {
                    projectile.targetPosition = teleportTargetPosition;
                }

                latestFiredProjectile = projectileObject;
            }
            fireTeleportCallback();
        }

        private void Teleport()
        {
            if (latestFiredProjectile)
            {
                playerMovement.TeleportTo(latestFiredProjectile.transform);
                Destroy(latestFiredProjectile.gameObject);
                currentTeleportCooldown = teleportCooldown;
            }
            else
            {
                if (currentTeleportCooldown <= 0.0f)
                {
                    FireTeleportProjectile();
                }
            }
        }

        private Transform GetMuzzleTransform()
        {
            return muzzleTransform;
        }

        public void ResetTeleportCooldown()
        {
            teleportCooldown = 0.0f;
        }
        private void OnDisable()
        {
            Destroy(currentGun);

            if (ammo != null && ammoShadow != null && reserve != null && reserveShadow != null && cursor != null && cursorShadow != null)
            {
                ammo.text = "";
                ammoShadow.text = "";
                reserve.text = "";
                reserveShadow.text = "";

                cursor.color = new Vector4(0, 0, 0, 0);
                cursorShadow.color = new Vector4(0, 0, 0, 0);
            }
        }
    }
}