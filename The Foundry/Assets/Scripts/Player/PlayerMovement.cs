using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.Assertions.Must;

namespace FPSGame
{
    public enum CharacterState
    {
        Default = 0,
        Teleport = 1,
        Leaping = 2
    }

    public enum OrientationMethod
    {
        TowardsCamera,
        TowardsMovement,
    }

    public struct AICharacterInputs
    {
        public Vector3 MoveVector;
        public Vector3 LookVector;
    }

    public enum BonusOrientationMethod
    {
        None,
        TowardsGravity,
        TowardsGroundSlopeAndGravity,
    }

    public class PlayerMovement : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor;

        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;
        public float StableMovementSharpness = 15f;
        public float OrientationSharpness = 20f;
        public OrientationMethod OrientationMethod = OrientationMethod.TowardsCamera;
        public Vector3 LastStablePosition;

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 10f;
        public float AirAccelerationSpeed = 30f;
        public float Drag = 0f;

        [Header("Jumping")]
        public bool AllowJumpingWhenSliding = true;
        public float JumpUpSpeed = 10f;
        public float JumpScalableForwardSpeed = 0f;
        public float JumpPreGroundingGraceTime = 0.1f;
        public float JumpPostGroundingGraceTime = 0.1f;
        public int currentJump;
        public int maxJumps;

        [Header("Sliding")]
        [Tooltip("Determines the speed of the character before sliding (1.0 = use the current speed).")]
        public float slidingSpeedMultiplier = 1.5f;
        public bool isSliding = false;
        public float targetSlideSpeed = 60f;
        public float slideSharpness = 5.0f;
        public float maxSlideDuration = 3.0f;
        public float currentSlideDuration = 0.0f;

        /// <summary>
        /// The slide counter only starts when the character touches the ground.
        /// </summary>
        public bool hasContactedGroundBeforeSliding = false;
        private Vector3 slidingDirection = Vector3.zero;

        // Returns true when the player contacts the ground for the first time.
        // When this happens, the character's speed increases.
        private bool shouldBoostSpeed = false;

        [Header("Teleport")]
        public LayerMask teleportCastLayer;
        public Vector3 teleportTargetPosition;
        public float teleportSpeed = 30.0f;
        public float teleportStoppingDistance = 2.0f;
        public AnimationCurve teleportCurve;

        private Vector3 teleportStartPosition;
        private float teleportEta;
        private float currentTeleportTime = 0.0f;
        private Vector3 teleportVelocity = new Vector3();

        public delegate void OnTeleportBegins();
        public event OnTeleportBegins onTeleportBegins;

        public delegate void OnTeleportEnds();
        public event OnTeleportEnds onTeleportEnds;

        public GameObject teleportSoundEffect;

        [Header("Crouching")]
        public float crouchingSpeed = 5.0f;

        private bool wallDetected = false;
        private bool ledgeDetected = false;
        private bool shouldLeap = false;

        [Header("Leaping")]
        // The character can only leap on the wall that has this minimum angle.
        public LayerMask wallLayers = -1;
        public float minimumWallAngle = 60.0f;

        RaycastHit wallCastHitInfo;
        RaycastHit ledgeCastHitInfo;

        [Header("Misc")]
        public List<Collider> IgnoredColliders = new List<Collider>();
        public BonusOrientationMethod BonusOrientationMethod = BonusOrientationMethod.None;
        public float BonusOrientationSharpness = 10f;
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        public Transform MeshRoot;
        public Transform CameraFollowPoint;
        public Transform StandingCameraTransform;
        public Transform CrouchingCameraTransform;

        public float CrouchedCapsuleHeight = 1f;
        public float CrouchingSharpness = 30.0f;

        public CharacterState CurrentCharacterState { get; private set; }

        private Collider[] _probedColliders = new Collider[8];
        private RaycastHit[] _probedHits = new RaycastHit[8];
        private Vector2 rawMoveInput;
        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _jumpedThisFrame = false;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump = 0f;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private bool _shouldBeCrouching = false;
        private bool _isCrouching = false;

        private Vector3 lastInnerNormal = Vector3.zero;
        private Vector3 lastOuterNormal = Vector3.zero;

        private void Awake()
        {
            // Handle initial state
            TransitionToState(CharacterState.Default);

            // Assign the characterController to the motor
            Motor.CharacterController = this;
        }

        private void OnDrawGizmos()
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Teleport:
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(teleportTargetPosition, 1.0f);
                    break;
            }
        }

        /// <summary>
        /// Handles movement state transitions and enter/exit callbacks
        /// </summary>
        public void TransitionToState(CharacterState newState)
        {
            CharacterState tmpInitialState = CurrentCharacterState;
            OnStateExit(tmpInitialState, newState);
            CurrentCharacterState = newState;
            OnStateEnter(newState, tmpInitialState);
        }

        /// <summary>
        /// Event when entering a state
        /// </summary>
        public void OnStateEnter(CharacterState state, CharacterState fromState)
        {
            switch (state)
            {
                case CharacterState.Default:
                    {
                        break;
                    }

                case CharacterState.Teleport:
                    {
                        teleportStartPosition = transform.position;
                        teleportEta = Vector3.Distance(transform.position, teleportTargetPosition) / teleportSpeed;
                        currentTeleportTime = 0.0f;

                        Motor.SetCapsuleCollisionsActivation(false);
                        Motor.SetMovementCollisionsSolvingActivation(false);
                        Motor.SetGroundSolvingActivation(false);

                        onTeleportBegins?.Invoke();

                        break;
                    }
            }
        }

        /// <summary>
        /// Event when exiting a state
        /// </summary>
        public void OnStateExit(CharacterState state, CharacterState toState)
        {
            switch (state)
            {
                case CharacterState.Default:
                    {
                        break;
                    }

                case CharacterState.Teleport:
                    {
                        Motor.SetCapsuleCollisionsActivation(true);
                        Motor.SetMovementCollisionsSolvingActivation(true);
                        Motor.SetGroundSolvingActivation(true);

                        Motor.BaseVelocity = Vector3.zero;

                        onTeleportEnds?.Invoke();

                        break;
                    }
            }
        }

        /// <summary>
        /// This is called every frame by ExamplePlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs)
        {
            // Clamp input
            rawMoveInput = new Vector2(inputs.RawMoveAxisRight, inputs.RawMoveAxisForward);
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Move and look inputs
                        _moveInputVector = cameraPlanarRotation * moveInputVector;

                        switch (OrientationMethod)
                        {
                            case OrientationMethod.TowardsCamera:
                                _lookInputVector = cameraPlanarDirection;
                                break;
                            case OrientationMethod.TowardsMovement:
                                _lookInputVector = _moveInputVector.normalized;
                                break;
                        }

                        // Jumping input
                        if (inputs.JumpDown)
                        {
                            _timeSinceJumpRequested = 0f;
                            _jumpRequested = true;
                        }

                        // Crouching input
                        if (inputs.CrouchDown)
                        {
                            _shouldBeCrouching = true;

                            if (!_isCrouching)
                            {
                                Crouch();

                                //MeshRoot.localScale = new Vector3(1f, 0.5f, 1f);
                                //

                                //CameraFollowPoint.position = CrouchingCameraTransform.transform.position;

                                // Only slide when the character is moving.
                                if (Motor.Velocity.magnitude >= 0.90f * MaxStableMoveSpeed)
                                {
                                    Vector3 currentVelocity = Motor.Velocity;

                                    StartSliding(new Vector3(currentVelocity.x, 0.0f, currentVelocity.z).normalized);
                                }
                            }
                        }
                        else if (inputs.CrouchUp)
                        {
                            _shouldBeCrouching = false;
                        }

                        break;
                    }

                case CharacterState.Leaping:
                    if (rawMoveInput.y < 0.0f)
                    {
                        TransitionToState(CharacterState.Default);
                    }

                    break;

            }
        }

        private void Crouch()
        {
            _isCrouching = true;
            Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
        }

        /// <summary>
        /// This is called every frame by the AI script in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref AICharacterInputs inputs)
        {
            _moveInputVector = inputs.MoveVector;
            _lookInputVector = inputs.LookVector;
        }

        private Quaternion _tmpTransientRot;

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
            ScanWallAndLedge();
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        if (_lookInputVector.sqrMagnitude > 0f && OrientationSharpness > 0f)
                        {
                            // Smoothly interpolate from current to target look direction
                            Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                            // Set the current rotation (which will be used by the KinematicCharacterMotor)
                            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                        }

                        Vector3 currentUp = (currentRotation * Vector3.up);
                        if (BonusOrientationMethod == BonusOrientationMethod.TowardsGravity)
                        {
                            // Rotate from current up to invert gravity
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        else if (BonusOrientationMethod == BonusOrientationMethod.TowardsGroundSlopeAndGravity)
                        {
                            if (Motor.GroundingStatus.IsStableOnGround)
                            {
                                Vector3 initialCharacterBottomHemiCenter = Motor.TransientPosition + (currentUp * Motor.Capsule.radius);

                                Vector3 smoothedGroundNormal = Vector3.Slerp(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                                // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                                Motor.SetTransientPosition(initialCharacterBottomHemiCenter + (currentRotation * Vector3.down * Motor.Capsule.radius));
                            }
                            else
                            {
                                Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                            }
                        }
                        else
                        {
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Ground movement
                        if (Motor.GroundingStatus.IsStableOnGround)
                        {
                            // Handle sliding
                            if (isSliding && hasContactedGroundBeforeSliding)
                            {
                                if (shouldBoostSpeed)
                                {
                                    currentVelocity *= slidingSpeedMultiplier;

                                    shouldBoostSpeed = false;
                                }

                                float currentVelocityMagnitude = currentVelocity.magnitude;
                                Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, 1f - Mathf.Exp(-slideSharpness * deltaTime));
                            }

                            else
                            {
                                float currentVelocityMagnitude = currentVelocity.magnitude;

                                Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                                // Reorient velocity on slope
                                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                                // Calculate target velocity
                                Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                                Vector3 targetMovementVelocity = reorientedInput * (_isCrouching ? crouchingSpeed : MaxStableMoveSpeed);

                                // Smooth movement Velocity
                                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
                            }

                            // Set Player's Stable Position
                            LastStablePosition = transform.position;

                            // Not Jumping
                            currentJump = 0;
                        }
                        // Air movement
                        else
                        {
                            // Add move input
                            if (_moveInputVector.sqrMagnitude > 0f)
                            {
                                Vector3 addedVelocity = (isSliding && hasContactedGroundBeforeSliding ? transform.forward : _moveInputVector) * AirAccelerationSpeed * deltaTime;

                                Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                                // Limit air velocity from inputs
                                if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed)
                                {
                                    // clamp addedVel to make total vel not exceed max vel on inputs plane
                                    Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxAirMoveSpeed);
                                    addedVelocity = newTotal - currentVelocityOnInputsPlane;
                                }
                                else
                                {
                                    // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                    if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                                    {
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                                    }
                                }

                                // Prevent air-climbing sloped walls
                                if (Motor.GroundingStatus.FoundAnyGround)
                                {
                                    if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                                    {
                                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                    }
                                }

                                // Apply added velocity
                                currentVelocity += addedVelocity;
                            }

                            // Gravity
                            currentVelocity += Gravity * deltaTime;

                            // Drag
                            currentVelocity *= (1f / (1f + (Drag * deltaTime)));

                            // Leaving Ground without Jumping
                            if (currentJump == 0 && _jumpRequested == false)
                            {
                                currentJump = 1;
                            }
                        }

                        // Handle jumping
                        _jumpedThisFrame = false;
                        _timeSinceJumpRequested += deltaTime;
                        if (_jumpRequested)
                        {
                            // See if we actually are allowed to jump
                            if ((!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : (Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))) || currentJump < maxJumps)
                            {
                                // Allow the change of velocity
                                if (currentJump != 0)
                                {
                                    currentVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
                                }

                                // Calculate jump direction before ungrounding
                                Vector3 jumpDirection = Motor.CharacterUp;
                                if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                                {
                                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                                }

                                // Makes the character skip ground probing/snapping on its next update. 
                                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                                // No thanks. I think I'll keep it.
                                Motor.ForceUnground();

                                // Add to the return velocity and reset jump state
                                currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                                currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                                _jumpRequested = false;
                                _jumpConsumed = true;
                                _jumpedThisFrame = true;

                                currentJump += 1;
                            }
                        }

                        // Handle sliding.
                        if (isSliding && hasContactedGroundBeforeSliding)
                        {
                            if (shouldBoostSpeed)
                            {
                                shouldBoostSpeed = false;
                            }

                            currentSlideDuration += deltaTime;
                        }

                        // Take into account additive velocity
                        if (_internalVelocityAdd.sqrMagnitude > 0f)
                        {
                            currentVelocity += _internalVelocityAdd;
                            _internalVelocityAdd = Vector3.zero;
                        }
                        break;
                    }

                case CharacterState.Teleport:
                    {
                        Vector3 translationToTarget = (teleportTargetPosition - transform.position);
                        Vector3 directionToTarget = translationToTarget.normalized;

                        float distanceFromStart = Vector3.Distance(teleportStartPosition, teleportTargetPosition);
                        Debug.Log($"Distance: {distanceFromStart}");

                        float t = 1 - Vector3.Distance(transform.position, teleportTargetPosition) / Vector3.Distance(teleportStartPosition, teleportTargetPosition);
                        t = Mathf.Clamp01(t);
                        Debug.Log("t = " + t);

                        float realSpeed = teleportSpeed * teleportCurve.Evaluate(t) * deltaTime;

                        currentVelocity = directionToTarget * realSpeed;

                        currentTeleportTime += deltaTime;

                        if (distanceFromStart >= 4.5f)
                        {
                            Motor.SetPosition(Vector3.SmoothDamp(transform.position, teleportTargetPosition, ref teleportVelocity, 0.1f));
                        }
                        else
                        {
                            Motor.SetPosition(teleportTargetPosition);
                        }

                        if ((distanceFromStart <= 4.5f) || t >= 0.995f || currentTeleportTime >= 1.0f)
                        {
                            currentVelocity = Vector3.zero;
                            TransitionToState(CharacterState.Default);
                        }

                        break;
                    }

                case CharacterState.Leaping:
                    Vector3 wallRight = Vector3.Cross(Vector3.up, wallCastHitInfo.normal).normalized;
                    Vector3 wallUp = Vector3.Cross(wallCastHitInfo.normal, wallRight).normalized;

                    // Fix the player sliding left/right while leaping on the wall.
                    Vector3 rightMovementDirection = Vector3.Project(currentVelocity, wallRight);
                    currentVelocity -= rightMovementDirection;

                    if (rawMoveInput.y > 0.0f)
                    {
                        // Lift the character up.
                        currentVelocity += wallUp * 20.0f * deltaTime;
                    }
                    else
                    {
                        currentVelocity.y = 0.0f;
                    }

                    // Move the character forward so that when the player reaches the top, the player doesn't fall.
                    currentVelocity += -wallCastHitInfo.normal * deltaTime;

                    break;
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Handle jump-related values
                        {
                            // Handle jumping pre-ground grace period
                            if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                            {
                                _jumpRequested = false;
                            }

                            if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                            {
                                // If we're on a ground surface, reset jumping values
                                if (!_jumpedThisFrame)
                                {
                                    _jumpConsumed = false;
                                }
                                _timeSinceLastAbleToJump = 0f;
                            }
                            else
                            {
                                // Keep track of time since we were last able to jump (for grace period)
                                _timeSinceLastAbleToJump += deltaTime;
                            }
                        }

                        // Handle uncrouching
                        if (_isCrouching && !_shouldBeCrouching)
                        {
                            // Do an overlap test with the character's standing height to see if there are any obstructions
                            Motor.SetCapsuleDimensions(0.5f, 2f, 1f);
                            if (Motor.CharacterOverlap(
                                Motor.TransientPosition,
                                Motor.TransientRotation,
                                _probedColliders,
                                Motor.CollidableLayers,
                                QueryTriggerInteraction.Ignore) > 0)
                            {
                                // If obstructions, just stick to crouching dimensions
                                Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                            }
                            else
                            {
                                // If no obstructions, uncrouch
                                //MeshRoot.localScale = new Vector3(1f, 1f, 1f);



                                //CameraFollowPoint.position = StandingCameraTransform.position;

                                _isCrouching = false;

                                StopSliding();
                            }
                        }

                        // Handle sliding.
                        if (currentSlideDuration >= maxSlideDuration)
                        {
                            StopSliding();
                        }

                        UpdateCameraTargetTransform(deltaTime);

                        break;
                    }
            }
        }

        private void StartSliding(Vector3 slidingDirection)
        {
            if (isSliding)
                return;

            isSliding = true;
            hasContactedGroundBeforeSliding = false;
            shouldBoostSpeed = true;
            currentSlideDuration = 0f;

            this.slidingDirection = slidingDirection;
        }

        private void StopSliding()
        {
            if (!isSliding)
                return;

            isSliding = false;
            hasContactedGroundBeforeSliding = false;
            currentSlideDuration = 0.0f;

            slidingDirection = Vector3.zero;
        }

        public void TeleportTo(Transform targetTransform)
        {
            if (targetTransform == null)
            {
                return;
            }

            TeleportTo(targetTransform.position);
        }

        public void TeleportTo(Vector3 position)
        {
            if (!CanTeleportToLocation(position))
                return;

            // Raycast to check if there is an obstacle between the character and the target position.
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + Motor.CharacterTransformToCapsuleCenter, position - transform.position, out hitInfo, Vector3.Distance(transform.position, position), teleportCastLayer))
            {
                teleportTargetPosition = hitInfo.point;
            }
            //else
            {
                teleportTargetPosition = position;
            }

            // Reset the teleport velocity.
            teleportVelocity = Vector3.zero;

            if (ShouldCrouchAtTeleportPosition(teleportTargetPosition)) 
                Crouch();

            TransitionToState(CharacterState.Teleport);
            Debug.Log($"Teleport to {position}");

            Instantiate(teleportSoundEffect, transform.position, transform.rotation, null);
        }

        private bool CanTeleportToLocation(Vector3 teleportTargetPosition)
        {
            RaycastHit hitInfo;

            // --------------------------
            // Check the ceiling height.
            // --------------------------

            // Raycast down to get the floor.
            if (!Physics.Raycast(teleportTargetPosition, Vector3.down, out hitInfo, Motor.Capsule.height, teleportCastLayer, QueryTriggerInteraction.Ignore))
                goto checkWidth;

            Vector3 floorPosition = hitInfo.point;

            // Raycast up to get the ceiling.
            if (!Physics.Raycast(floorPosition, Vector3.up, out hitInfo, Motor.Capsule.height, teleportCastLayer, QueryTriggerInteraction.Ignore))
                goto checkWidth;

            Vector3 ceilingPosition = hitInfo.point;

            // The ceiling is too low, can't teleport.
            if (ceilingPosition.y - floorPosition.y < CrouchedCapsuleHeight)
                return false;

            // -------------------------------
            // Check the width of the hallway.
            // -------------------------------
            checkWidth:
         
            float capsuleDiameter = 2 * Motor.Capsule.radius;

            // Raycast to the left.
            if (!Physics.Raycast(teleportTargetPosition, Vector3.left, out hitInfo, capsuleDiameter, teleportCastLayer, QueryTriggerInteraction.Ignore))
                return true;

            Vector3 leftPosition = hitInfo.point;

            // Raycast to the right.
            if (!Physics.Raycast(leftPosition, Vector3.right, out hitInfo, capsuleDiameter, teleportCastLayer, QueryTriggerInteraction.Ignore))
                return true;

            Vector3 rightPosition = hitInfo.point;

            // The hallway is too narrow, can't teleport.
            if (rightPosition.y - leftPosition.y < capsuleDiameter)
                return false;

            return true;
        }

        private bool ShouldCrouchAtTeleportPosition(Vector3 teleportTargetPosition)
        {
            RaycastHit hitInfo;

            // Raycast upward to check if there is an obstacle above the player.
            if (!Physics.Raycast(teleportTargetPosition, Vector3.up, out hitInfo, Motor.Capsule.height, teleportCastLayer, QueryTriggerInteraction.Ignore))
                return false;

            Vector3 ceilingPosition = hitInfo.point;

            // Raycast down to measure the height from the ceiling to the floor.
            if (!Physics.Raycast(ceilingPosition, Vector3.down, out hitInfo, Motor.Capsule.height, teleportCastLayer, QueryTriggerInteraction.Ignore))
                return false;

            Vector3 floorPosition = hitInfo.point;

            return ceilingPosition.y - floorPosition.y < Motor.Capsule.height;
        }

        private void ScanWallAndLedge()
        {
            // Raycast forward to check if there is a wall ahead.
            Vector3 wallRaycastStart = transform.TransformPoint(Motor.CharacterTransformToCapsuleBottom);
            float wallRaycastDistance = Motor.Capsule.radius + 0.2f;
            wallDetected = Physics.Raycast(wallRaycastStart, transform.forward, out wallCastHitInfo, wallRaycastDistance, wallLayers, QueryTriggerInteraction.Ignore);
            Debug.DrawLine(wallRaycastStart, wallRaycastStart + transform.forward * wallRaycastDistance, Color.red);

            if (wallDetected && Vector3.Angle(wallCastHitInfo.normal, Vector3.up) >= minimumWallAngle)
            {
                // Raycast forward to check if there is a ledge ahead.
                Vector3 ledgeRaycastStart = transform.TransformPoint(Motor.CharacterTransformToCapsuleTop) + Vector3.up * 0.1f;
                float ledgeRaycastDistance = Motor.Capsule.radius + 0.2f;

                ledgeDetected = !Physics.Raycast(ledgeRaycastStart, transform.forward, out ledgeCastHitInfo, ledgeRaycastDistance, wallLayers, QueryTriggerInteraction.Ignore);
                Debug.DrawLine(ledgeRaycastStart, ledgeRaycastStart + transform.forward * ledgeRaycastDistance, Color.yellow);
            }
            else
            {
                wallDetected = false;
                ledgeDetected = false;
            }

            if (!Motor.GroundingStatus.IsStableOnGround && wallDetected && ledgeDetected && rawMoveInput.y >= 0.0f)
            {
                //shouldLeap = true;

                if (rawMoveInput.y > 0.0f)
                {
                    Debug.Log("Edge detected");
                    TransitionToState(CharacterState.Leaping);
                }
            }
            else if (CurrentCharacterState == CharacterState.Leaping)
            {
                //shouldLeap = false;

                TransitionToState(CharacterState.Default);
            }
        }

        private void UpdateCameraTargetTransform(float deltaTime)
        {
            if (_isCrouching)
            {
                CameraFollowPoint.position = Vector3.Lerp(CameraFollowPoint.transform.position, CrouchingCameraTransform.transform.position, 1f - Mathf.Exp(-CrouchingSharpness * deltaTime));
                MeshRoot.localScale = Vector3.Lerp(MeshRoot.localScale, new Vector3(1f, 0.5f, 1.0f), 1f - Mathf.Exp(-CrouchingSharpness * deltaTime));
            }
            else
            {
                CameraFollowPoint.position = Vector3.Lerp(CameraFollowPoint.transform.position, StandingCameraTransform.transform.position, 1f - Mathf.Exp(-CrouchingSharpness * deltaTime));
                MeshRoot.localScale = Vector3.Lerp(MeshRoot.localScale, Vector3.one, 1f - Mathf.Exp(-CrouchingSharpness * deltaTime));
            }
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // Handle landing and leaving ground
            if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLanded();
            }
            else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLeaveStableGround();
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (IgnoredColliders.Count == 0)
            {
                return true;
            }

            if (IgnoredColliders.Contains(coll))
            {
                return false;
            }

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            if (isSliding && !hasContactedGroundBeforeSliding)
            {
                hasContactedGroundBeforeSliding = true;
            }
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        _internalVelocityAdd += velocity;
                        break;
                    }
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        protected void OnLanded()
        {
        }

        protected void OnLeaveStableGround()
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}

