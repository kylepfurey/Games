using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KinematicCharacterController;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEditor;

namespace FPSGame
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInput playerInput;

        // Movement inputs.
        private InputAction actionMove, actionLook, actionJump, actionCrouch;

        // Combat inputs.
        private InputAction actionSwitchAutomatic, actionSwitchTeleport, actionFire, actionTeleport, actionDestroyTeleportProjectile, actionToggleFlashLight,
            actionSwitchRight, actionSwitchLeft, action1, action2, action3, action4, action5, actionReload;

        // Misc inputs.
        private InputAction actionExit, actionRestart;
        public bool RMB;
        public bool PAUSE;
        public bool PAUSE_UP;
        public GameObject PauseMenu;
        public StartMenuScript menu;

        private bool shouldJump = false;
        private bool shouldFire = false;
        private bool shouldTeleport = false;
        private bool shouldSwitchAutomatic = false;
        private bool shouldSwitchTeleport = false;
        private bool shouldDestroyTeleportProjectile = false;
        private bool shouldToggleFlashLight = false;

        public PlayerMovement Character;
        public PlayerCamera CharacterCamera;
        public PlayerCombat CharacterCombat;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";

        public float controllerSpeedX;
        public float controllerSpeedY;

        public bool autoReload;

        private void OnValidate()
        {
            if (!playerInput)
            {
                playerInput = GetComponent<PlayerInput>();
            }

            if (!Character)
            {
                Character = FindObjectOfType<PlayerMovement>();
            }

            if (!CharacterCamera)
            {
                CharacterCamera = FindObjectOfType<PlayerCamera>();
            }

            if (!CharacterCombat)
            {
                CharacterCombat = FindObjectOfType<PlayerCombat>();
            }
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            InitializeInput();

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(CharacterCombat.GetComponentsInChildren<Collider>());
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && menu == null)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (menu == null)
            {
                HandleCharacterInput();
            }

            // Right Mouse Button
            if (playerInput.actions.FindAction("Teleport").ReadValue<float>() > 0)
            {
                RMB = true;
            }
            else
            {
                RMB = false;
            }

            // Pausing
            PAUSE = Mathf.Abs(playerInput.actions.FindAction("Pause").ReadValue<float>()) > 0;

            if (!PAUSE)
            {
                PAUSE_UP = true;
            }

            if (PAUSE && PAUSE_UP && menu == null)
            {
                OnInputAction_Pause();
            }

            if (PAUSE)
            {
                PAUSE_UP = false;
            }
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void InitializeInput()
        {
            actionMove = playerInput.actions.FindAction("Move");
            actionLook = playerInput.actions.FindAction("Look");    // Controller Only

            actionJump = playerInput.actions.FindAction("Jump");
            actionJump.performed += OnInputAction_Jump;

            actionCrouch = playerInput.actions.FindAction("Crouch");

            actionFire = playerInput.actions.FindAction("Fire");
            actionFire.performed += OnInputAction_Fire;

            actionTeleport = playerInput.actions.FindAction("Teleport");
            actionTeleport.performed += OnInputAction_Teleport;

            actionReload = playerInput.actions.FindAction("Reload");
            actionReload.performed += OnInputAction_Reload;

            actionSwitchAutomatic = playerInput.actions.FindAction("SwitchAutomatic");
            actionSwitchAutomatic.performed += OnInputAction_SwitchAutomatic;

            actionSwitchTeleport = playerInput.actions.FindAction("SwitchTeleport");
            actionSwitchTeleport.performed += OnInputAction_SwitchTeleport;

            actionSwitchRight = playerInput.actions.FindAction("SwitchRight");
            actionSwitchRight.performed += OnInputAction_Right;

            actionSwitchLeft = playerInput.actions.FindAction("SwitchLeft");
            actionSwitchLeft.performed += OnInputAction_Left;

            action1 = playerInput.actions.FindAction("Weapon 1");
            action1.performed += OnInputAction_1;

            action2 = playerInput.actions.FindAction("Weapon 2");
            action2.performed += OnInputAction_2;

            action3 = playerInput.actions.FindAction("Weapon 3");
            action3.performed += OnInputAction_3;

            action4 = playerInput.actions.FindAction("Weapon 4");
            action4.performed += OnInputAction_4;

            action5 = playerInput.actions.FindAction("Weapon 5");
            action5.performed += OnInputAction_5;

            actionExit = playerInput.actions.FindAction("Exit");
            actionExit.performed += OnInputAction_Exit;

            actionRestart = playerInput.actions.FindAction("Restart");
            actionRestart.performed += OnInputAction_Restart;

            actionDestroyTeleportProjectile = playerInput.actions.FindAction("DestroyTeleportProjectile");
            actionDestroyTeleportProjectile.performed += OnInputAction_DestroyTeleportProjectile;

            actionToggleFlashLight = playerInput.actions.FindAction("FlashLight");
            actionToggleFlashLight.performed += OnInputAction_ToggleFlashLight;
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float lookAxisUp = Input.GetAxisRaw(MouseYInput);
            float lookAxisRight = Input.GetAxisRaw(MouseXInput);

            Vector2 rightStick = actionLook.ReadValue<Vector2>();

            if (Mathf.Abs(rightStick.x) > 0 || Mathf.Abs(rightStick.y) > 0)
            {
                lookAxisUp = rightStick.y * controllerSpeedY * Time.deltaTime;
                lookAxisRight = rightStick.x * controllerSpeedX * Time.deltaTime;
            }

            if (RMB)
            {
                lookAxisUp /= 2;
                lookAxisRight /= 2;
            }

            // The new input system really sucks when it comes to handling mouse input.
            //Vector2 lookInput = actionLook.ReadValue<Vector2>();
            //float mouseLookAxisUp = lookInput.y;
            //float mouseLookAxisRight = lookInput.x;
            //Debug.Log($"mouseLookAxisUp: {mouseLookAxisUp}"); 
            //Debug.Log($"mouseLookAxisRight: {mouseLookAxisRight}");

            Vector3 lookInputVector = new Vector3(lookAxisRight, lookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, 0.0f, lookInputVector);
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Moving
            Vector2 moveInput = actionMove.ReadValue<Vector2>();
            characterInputs.RawMoveAxisForward = moveInput.y;
            characterInputs.RawMoveAxisRight = moveInput.x;
            characterInputs.MoveAxisForward = moveInput.y;
            characterInputs.MoveAxisRight = moveInput.x;

            // Camera rotation
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;

            // Jumping
            characterInputs.JumpDown = shouldJump;
            shouldJump = false;

            // Crouching
            characterInputs.CrouchDown = actionCrouch.IsPressed();
            characterInputs.CrouchUp = !actionCrouch.IsPressed();

            // Switch to automatic rifle mode.
            characterInputs.SwitchAutomatic = shouldSwitchAutomatic;
            shouldSwitchAutomatic = false;

            // Switch to teleport mode.
            characterInputs.SwitchTeleport = shouldSwitchTeleport;
            shouldSwitchTeleport = false;

            // Firing
            characterInputs.Fire = shouldFire;

            // Teleport.
            characterInputs.Teleport = shouldTeleport;
            shouldTeleport = false;

            // Destroy teleport projectile.
            characterInputs.DestroyTeleportProjectile = shouldDestroyTeleportProjectile;
            shouldDestroyTeleportProjectile = false;

            // Toggle flashlight.
            characterInputs.ToggleFlashLight = shouldToggleFlashLight;
            shouldToggleFlashLight = false;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
            CharacterCombat.SetPlayerInput(ref characterInputs);
        }

        private void OnInputAction_Jump(InputAction.CallbackContext obj)
        {
            shouldJump = true;
        }

        private void OnInputAction_SwitchAutomatic(InputAction.CallbackContext obj)
        {
            shouldSwitchAutomatic = true;
        }

        private void OnInputAction_SwitchTeleport(InputAction.CallbackContext obj)
        {
            shouldSwitchTeleport = true;
        }

        private void OnInputAction_Fire(InputAction.CallbackContext obj)
        {
            shouldFire = obj.action.IsPressed();
        }

        private void OnInputAction_Teleport(InputAction.CallbackContext obj)
        {
            shouldTeleport = true;
        }

        private void OnInputAction_DestroyTeleportProjectile(InputAction.CallbackContext obj)
        {
            shouldDestroyTeleportProjectile = true;
        }

        private void OnInputAction_ToggleFlashLight(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.Weapon != 0)
            {
                shouldToggleFlashLight = true;
            }
        }

        private void OnInputAction_Reload(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.ammoReserve[CharacterCombat.ammoReserveType[CharacterCombat.Weapon]] > 0 && CharacterCombat.currentAmmo[CharacterCombat.Weapon] < CharacterCombat.maxAmmo[CharacterCombat.Weapon] && CharacterCombat.reloadTimer == 0)
            {
                CharacterCombat.reloadTimer = CharacterCombat.reloadTimeStart[CharacterCombat.Weapon];
            }
        }

        private void OnInputAction_Right(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.Weapon != 0 && (CharacterCombat.reloadTimer > CharacterCombat.reloadTimeStart[CharacterCombat.Weapon] - CharacterCombat.reloadAmmoRestockTime[CharacterCombat.Weapon] || CharacterCombat.reloadTimer == 0) && CharacterCombat.burstTimer <= 0)
            {
                int oldWeapon = CharacterCombat.Weapon;

                CharacterCombat.Weapon += 1;

                if (CharacterCombat.Weapon > CharacterCombat.Gun.Length - 1)
                {
                    CharacterCombat.Weapon = 1;
                }

                for (int i = 1; i < CharacterCombat.Gun.Length; i++)
                {
                    if (CharacterCombat.unlocked[CharacterCombat.Weapon])
                    {
                        break;
                    }

                    CharacterCombat.Weapon++;

                    if (CharacterCombat.Weapon > CharacterCombat.Gun.Length - 1)
                    {
                        CharacterCombat.Weapon = 1;
                    }
                }

                if (oldWeapon != CharacterCombat.Weapon)
                {
                    CharacterCombat.reloadTimer = 0;

                    transform.parent.GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }
        }

        private void OnInputAction_Left(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.Weapon != 0 && (CharacterCombat.reloadTimer > CharacterCombat.reloadTimeStart[CharacterCombat.Weapon] - CharacterCombat.reloadAmmoRestockTime[CharacterCombat.Weapon] || CharacterCombat.reloadTimer == 0) && CharacterCombat.burstTimer <= 0)
            {
                int oldWeapon = CharacterCombat.Weapon;

                CharacterCombat.Weapon -= 1;

                if (CharacterCombat.Weapon <= 0)
                {
                    CharacterCombat.Weapon = CharacterCombat.Gun.Length - 1;
                }

                for (int i = 1; i < CharacterCombat.Gun.Length; i++)
                {
                    if (CharacterCombat.unlocked[CharacterCombat.Weapon])
                    {
                        break;
                    }

                    CharacterCombat.Weapon--;

                    if (CharacterCombat.Weapon <= 0)
                    {
                        CharacterCombat.Weapon = CharacterCombat.Gun.Length - 1;
                    }
                }

                if (oldWeapon != CharacterCombat.Weapon)
                {
                    CharacterCombat.reloadTimer = 0;

                    transform.parent.GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }
        }

        private void OnInputAction_1(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.Weapon != 1)
            {
                if (CharacterCombat.Weapon != 0 && (CharacterCombat.reloadTimer > CharacterCombat.reloadTimeStart[CharacterCombat.Weapon] - CharacterCombat.reloadAmmoRestockTime[CharacterCombat.Weapon] || CharacterCombat.reloadTimer == 0) && CharacterCombat.burstTimer <= 0)
                {
                    if (CharacterCombat.unlocked[1] || Character.GetComponent<Health>().godMode)
                    {
                        CharacterCombat.Weapon = 1;
                    }

                    CharacterCombat.reloadTimer = 0;

                    transform.parent.GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }
        }

        private void OnInputAction_2(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.Weapon != 2)
            {
                if (CharacterCombat.Weapon != 0 && (CharacterCombat.reloadTimer > CharacterCombat.reloadTimeStart[CharacterCombat.Weapon] - CharacterCombat.reloadAmmoRestockTime[CharacterCombat.Weapon] || CharacterCombat.reloadTimer == 0) && CharacterCombat.burstTimer <= 0)
                {
                    if (CharacterCombat.unlocked[2] || Character.GetComponent<Health>().godMode)
                    {
                        CharacterCombat.Weapon = 2;
                    }

                    CharacterCombat.reloadTimer = 0;

                    transform.parent.GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }
        }

        private void OnInputAction_3(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.Weapon != 3)
            {
                if (CharacterCombat.Weapon != 0 && (CharacterCombat.reloadTimer > CharacterCombat.reloadTimeStart[CharacterCombat.Weapon] - CharacterCombat.reloadAmmoRestockTime[CharacterCombat.Weapon] || CharacterCombat.reloadTimer == 0) && CharacterCombat.burstTimer <= 0)
                {
                    if (CharacterCombat.unlocked[3] || Character.GetComponent<Health>().godMode)
                    {
                        CharacterCombat.Weapon = 3;
                    }

                    CharacterCombat.reloadTimer = 0;

                    transform.parent.GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }
        }

        private void OnInputAction_4(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.Weapon != 4)
            {
                if (CharacterCombat.Weapon != 0 && (CharacterCombat.reloadTimer > CharacterCombat.reloadTimeStart[CharacterCombat.Weapon] - CharacterCombat.reloadAmmoRestockTime[CharacterCombat.Weapon] || CharacterCombat.reloadTimer == 0) && CharacterCombat.burstTimer <= 0)
                {
                    if (CharacterCombat.unlocked[4] || Character.GetComponent<Health>().godMode)
                    {
                        CharacterCombat.Weapon = 4;
                    }

                    CharacterCombat.reloadTimer = 0;

                    transform.parent.GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }
        }

        private void OnInputAction_5(InputAction.CallbackContext obj)
        {
            if (CharacterCombat.Weapon != 5)
            {
                if (CharacterCombat.Weapon != 0 && (CharacterCombat.reloadTimer > CharacterCombat.reloadTimeStart[CharacterCombat.Weapon] - CharacterCombat.reloadAmmoRestockTime[CharacterCombat.Weapon] || CharacterCombat.reloadTimer == 0) && CharacterCombat.burstTimer <= 0)
                {
                    if (CharacterCombat.unlocked[5] || Character.GetComponent<Health>().godMode)
                    {
                        CharacterCombat.Weapon = 5;
                    }

                    CharacterCombat.reloadTimer = 0;

                    transform.parent.GetComponentInChildren<GunLogic>().StopReloadAnimation();
                }
            }
        }

        private void OnInputAction_Exit(InputAction.CallbackContext obj)
        {
            Application.Quit();
        }

        private void OnInputAction_Restart(InputAction.CallbackContext obj)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnInputAction_Pause()
        {
            if (menu == null && SceneManager.GetActiveScene().name != "StartMenu")
            {
                Cursor.lockState = CursorLockMode.None;
                menu = Instantiate(PauseMenu).GetComponent<StartMenuScript>();
                menu.Paused = true;
            }
        }

        private void OnDestroy()
        {
            actionMove = playerInput.actions.FindAction("Move");
            actionLook = playerInput.actions.FindAction("Look");    // Controller Only

            actionJump = playerInput.actions.FindAction("Jump");
            actionJump.performed -= OnInputAction_Jump;

            actionCrouch = playerInput.actions.FindAction("Crouch");

            actionFire = playerInput.actions.FindAction("Fire");
            actionFire.performed -= OnInputAction_Fire;

            actionTeleport = playerInput.actions.FindAction("Teleport");
            actionTeleport.performed -= OnInputAction_Teleport;

            actionReload = playerInput.actions.FindAction("Reload");
            actionReload.performed -= OnInputAction_Reload;

            actionSwitchAutomatic = playerInput.actions.FindAction("SwitchAutomatic");
            actionSwitchAutomatic.performed -= OnInputAction_SwitchAutomatic;

            actionSwitchTeleport = playerInput.actions.FindAction("SwitchTeleport");
            actionSwitchTeleport.performed -= OnInputAction_SwitchTeleport;

            actionSwitchRight = playerInput.actions.FindAction("SwitchRight");
            actionSwitchRight.performed -= OnInputAction_Right;

            actionSwitchLeft = playerInput.actions.FindAction("SwitchLeft");
            actionSwitchLeft.performed -= OnInputAction_Left;

            action1 = playerInput.actions.FindAction("Weapon 1");
            action1.performed -= OnInputAction_1;

            action2 = playerInput.actions.FindAction("Weapon 2");
            action2.performed -= OnInputAction_2;

            action3 = playerInput.actions.FindAction("Weapon 3");
            action3.performed -= OnInputAction_3;

            action4 = playerInput.actions.FindAction("Weapon 4");
            action4.performed -= OnInputAction_4;

            action5 = playerInput.actions.FindAction("Weapon 5");
            action5.performed -= OnInputAction_5;

            actionExit = playerInput.actions.FindAction("Exit");
            actionExit.performed -= OnInputAction_Exit;

            actionRestart = playerInput.actions.FindAction("Restart");
            actionRestart.performed -= OnInputAction_Restart;

            actionDestroyTeleportProjectile = playerInput.actions.FindAction("DestroyTeleportProjectile");
            actionDestroyTeleportProjectile.performed -= OnInputAction_DestroyTeleportProjectile;

            actionToggleFlashLight = playerInput.actions.FindAction("FlashLight");
            actionToggleFlashLight.performed -= OnInputAction_ToggleFlashLight;
        }
    }
}