/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public partial class MarioMachine : SuperStateMachine {

    void Idle_EnterState()
    {
    }

    void Idle_SuperUpdate()
    {
        if (IsSliding() && moveDirection == Vector3.zero)
        {
            currentState = MarioStates.Slide;
            return;
        }

        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (!isTakingFallDamage && input.Current.JumpDown)
        {
            currentJumpProfile = ResolveJump();
            currentState = MarioStates.Jump;
            return;
        }

        if (!isTakingFallDamage && input.Current.StrikeDown)
        {
            currentState = MarioStates.Strike;
            return;
        }

        if (!isTakingFallDamage && input.Current.Trigger)
        {
            currentState = MarioStates.Crouch;
            return;
        }

        if (input.Current.MoveInput != Vector3.zero)
        {
            currentState = MarioStates.Run;
            return;
        }
        else
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 60.0f * controller.deltaTime);
            moveDirection = Math3d.SetVectorLength(moveDirection, Mathf.Abs(moveSpeed));
        }

        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (!anim.IsPlaying("idle") && !anim.IsPlaying("land") && !anim.IsPlaying("bum_drop_recover"))
        {
            anim.CrossFade("idle", 0.3f);
        }
    }

    private Vector3 cachedDirection;

    void Run_EnterState()
    {
        cachedDirection = lookDirection;

        anim.CrossFade("run_redux", 0.15f);

        sound.StartFootsteps(anim["run_redux"].length / 1.7f / 2f, 0.185f);
    }

    void Run_SuperUpdate()
    {
        lookDirection = cachedDirection;

        RunSmokeEffect.enableEmission = false;

        if (!isTakingFallDamage && input.Current.JumpDown)
        {
            currentJumpProfile = ResolveJump();
            currentState = MarioStates.Jump;
            return;
        }

        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (!isTakingFallDamage && input.Current.Trigger)
        {
            currentState = MarioStates.Crouch;
            return;
        }

        if (!isTakingFallDamage && input.Current.StrikeDown)
        {
            if (moveSpeed > runSpeed * 0.9f)
            {
                transform.position += controller.up * 0.3f;
                verticalMoveSpeed = 6.0f;
                sound.PlayGroundDive();
                currentState = MarioStates.Dive;
                return;
            }
            else
            {
                currentState = MarioStates.Strike;
                return;
            }
        }

        if (IsSliding())
        {
            if (Vector3.Angle(Math3d.ProjectVectorOnPlane(controller.currentGround.Hit.normal, lookDirection), SlopeDirection()) < 90.0f)
            {
                currentState = MarioStates.Slide;
                return;
            }
        }

        Vector3 wallCollisionNormal = Vector3.zero;
        float wallCollisionAngle = 0;

        if (input.Current.MoveInput != Vector3.zero)
        {
            if (Vector3.Angle(input.Current.MoveInput, lookDirection) > 110)
            {
                if (IsSliding())
                {
                    currentState = MarioStates.Slide;
                    return;
                }
                else
                {
                    if (moveSpeed > runSpeed * 0.56f)
                    {
                        currentState = MarioStates.Stop;
                        return;
                    }
                    else if (moveSpeed == 0)
                    {
                        lookDirection = input.Current.MoveInput;
                    }
                }
            }

            Vector3 moveDirectionProjected = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);

            RotateLookDirection(input.Current.MoveInput, turnSpeed);

            float targetSpeed = isTakingFallDamage ? runSpeed * 0.3f : runSpeed;

            if (!IsSliding())
            {
                SuperCollision col;

                if (HasWallCollided(out col) && Vector3.Angle(moveDirectionProjected, -col.normal) < 65.0f)
                {
                    moveSpeed = runSpeed * 0.2f;
                    wallCollisionNormal = col.normal;
                    wallCollisionAngle = Vector3.Angle(moveDirectionProjected, -col.normal);
                }
                else
                {
                    float acceleration = targetSpeed * input.Current.MoveMagnitude >= moveSpeed ? SuperMath.BoundedInterpolation(new float[] { 1.3f, 3f, 6f }, new float[] { 16, 12, 8, 6 }, moveSpeed) : 16;
                    moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed * input.Current.MoveMagnitude, acceleration * controller.deltaTime);
                }
            }
            else
            {
                float t = Mathf.InverseLerp(20.0f, 60.0f, GroundAngle());
                float deccelerationModifier = Mathf.Lerp(1.0f, 3.0f, t);
                float decceleration = SuperMath.BoundedInterpolation(new float[] { 1.3f, 3f, 6f }, new float[] { 1 * deccelerationModifier, 4 * deccelerationModifier, 7 * deccelerationModifier, 15 * deccelerationModifier}, moveSpeed);
                moveSpeed = Mathf.MoveTowards(moveSpeed, 0, decceleration * controller.deltaTime);

                if (moveSpeed == 0)
                {
                    currentState = MarioStates.Slide;
                    return;
                }
            }

            Vector3 f = Vector3.Cross(controller.currentGround.Hit.normal, Vector3.Cross(lookDirection, Vector3.up));
            moveDirection = Math3d.SetVectorLength(f, moveSpeed);
        }
        else
        {
            if (IsSliding())
            {
                currentState = MarioStates.Slide;
                return;
            }
            else
            {
                if (moveSpeed > runSpeed * 0.66f)
                {
                    currentState = MarioStates.Stop;
                    return;
                }
                else
                {
                    currentState = MarioStates.Idle;
                    return;
                }
            }
        }

        Vector3 previousLookDirection = cachedDirection;
        cachedDirection = lookDirection;

        float artRotationSpeed = 6.0f;

        if (wallCollisionNormal != Vector3.zero)
        {
            if (wallCollisionAngle < 20.0f)
            {
                if (!anim.IsPlaying("wall_push"))
                {
                    anim.CrossFade("wall_push", 0.05f);
                    sound.EndFootsteps();
                    sound.StartFootsteps(anim["creep"].length / 2f, 0.135f);
                }

                chestBendAngle = 0;

                chestTwistAngle = Mathf.MoveTowards(chestTwistAngle, 0, 200.0f * controller.deltaTime);
            }
            else
            {
                if (Vector3.Angle(Math3d.ProjectVectorOnPlane(controller.up, moveDirection), Vector3.Cross(wallCollisionNormal, controller.up)) > 90)
                {
                    if (!anim.IsPlaying("wall_slide_left"))
                    {
                        anim.CrossFade("wall_slide_left", 0.05f);
                        sound.EndFootsteps();
                    }

                    chestBendAngle = 0;

                    chestTwistAngle = Mathf.MoveTowards(chestTwistAngle, 0, 200.0f * controller.deltaTime);
                }
                else
                {
                    if (!anim.IsPlaying("wall_slide_right"))
                    {
                        anim.CrossFade("wall_slide_right", 0.05f);
                        sound.EndFootsteps();
                    }

                    chestBendAngle = 0;

                    chestTwistAngle = Mathf.MoveTowards(chestTwistAngle, 0, 200.0f * controller.deltaTime);
                }
            }

            artUpDirection = Vector3.RotateTowards(artUpDirection, controller.up, artRotationSpeed * controller.deltaTime, 0);

            lookDirection = -wallCollisionNormal;
        }
        else
        {
            if (input.Current.MoveMagnitude > 0.5)
            {
                if (!anim.IsPlaying("run_redux"))
                {
                    anim.CrossFade("run_redux", 0.15f);
                    sound.EndFootsteps();
                    sound.StartFootsteps(anim["run_redux"].length / 1.7f / 2f, 0.185f);
                }

                if (!SuperMath.Timer(timeEnteredState, 1.0f))
                {
                    RunSmokeEffect.enableEmission = true;
                }

                if (GroundAngle() > 15.0f)
                {
                    var projectedSlopeDirection = Math3d.ProjectVectorOnPlane(controller.up, SlopeDirection());
                    var dot = Vector3.Dot(projectedSlopeDirection, lookDirection);

                    dot = Mathf.Clamp(dot * 2, -1, 1);

                    float artAngle = Mathf.Lerp(0, GroundAngle(), Mathf.Abs(dot));

                    Vector3 right = Vector3.Cross(controller.up, lookDirection);

                    artUpDirection = Vector3.RotateTowards(artUpDirection, (Quaternion.AngleAxis(artAngle * Mathf.Sign(dot), right) * controller.up).normalized, artRotationSpeed * controller.deltaTime, 0);
                }
                else
                {
                    artUpDirection = Vector3.RotateTowards(artUpDirection, controller.up, artRotationSpeed * controller.deltaTime, 0);
                }

                float lerpValue = Mathf.InverseLerp(runSpeed * 0.5f, runSpeed * 0.9f, moveSpeed);
                chestBendAngle = Mathf.Lerp(0, -18, lerpValue);

                Vector3 previousRight = Vector3.Cross(controller.up, previousLookDirection);

                bool turningRight = Vector3.Angle(previousLookDirection, previousRight) > Vector3.Angle(lookDirection, previousRight);

                float lookAngleDifference = Vector3.Angle(previousLookDirection, lookDirection) * controller.deltaTime;

                if (lookAngleDifference > 0.05f)
                    chestTwistAngle = turningRight ? Mathf.MoveTowards(chestTwistAngle, -18.0f, 200.0f * controller.deltaTime) : Mathf.MoveTowards(chestTwistAngle, 18.0f, 200.0f * controller.deltaTime);
                else
                    chestTwistAngle = Mathf.MoveTowards(chestTwistAngle, 0, 200.0f * controller.deltaTime);
                
            }
            else if (input.Current.MoveMagnitude > 0.25)
            {
                if (!anim.IsPlaying("walk"))
                {
                    anim.CrossFade("walk", 0.15f);
                    sound.EndFootsteps();
                    sound.StartFootsteps(anim["walk"].length / 2f, 0.125f);
                }

                artUpDirection = Vector3.RotateTowards(artUpDirection, controller.up, artRotationSpeed * controller.deltaTime, 0);

                chestBendAngle = 0;

                chestTwistAngle = Mathf.MoveTowards(chestTwistAngle, 0, 200.0f * controller.deltaTime);
            }
            else
            {
                if (!anim.IsPlaying("creep"))
                {
                    anim.CrossFade("creep", 0.15f);
                    sound.EndFootsteps();
                    sound.StartFootsteps(anim["creep"].length / 2f, 0.95f);
                }

                artUpDirection = Vector3.RotateTowards(artUpDirection, controller.up, artRotationSpeed * controller.deltaTime, 0);

                chestBendAngle = 0;

                chestTwistAngle = Mathf.MoveTowards(chestTwistAngle, 0, 200.0f * controller.deltaTime);
            }
        }
    }

    void Run_ExitState()
    {
        chestBendAngle = 0;

        chestTwistAngle = 0;

        artUpDirection = controller.up;

        RunSmokeEffect.enableEmission = false;

        sound.EndFootsteps();
    }

    void Stop_EnterState()
    {
        anim.CrossFade("stop", 0.1f);

        sound.PlaySkid();

        RunSmokeEffect.enableEmission = true;
    }

    void Stop_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (Vector3.Angle(input.Current.MoveInput, lookDirection) > 110 && input.Current.JumpDown)
        {
            lookDirection *= -1;
            currentJumpProfile = jumpSideFlip;
            currentState = MarioStates.Jump;
            return;
        }

        moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 60.0f * controller.deltaTime);
        moveDirection = Math3d.SetVectorLength(moveDirection, moveSpeed);

        if (input.Current.MoveInput != Vector3.zero)
        {
            if (Vector3.Angle(input.Current.MoveInput, lookDirection) < 110)
            {
                currentState = MarioStates.Run;
                return;
            }
            else
            {
                if (moveSpeed == 0)
                {
                    lookDirection *= -1;
                    currentState = MarioStates.Turn;
                    return;
                }
            }
        }

        if (moveSpeed == 0)
        {
            currentState = MarioStates.Idle;
            return;
        }
    }

    void Stop_ExitState()
    {
        RunSmokeEffect.enableEmission = false;
    }

    void Turn_EnterState()
    {
        anim.Play("turn");
    }

    void Turn_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (input.Current.JumpDown)
        {
            currentJumpProfile = jumpSideFlip;
            currentState = MarioStates.Jump;
            return;
        }

        if (input.Current.MoveInput != Vector3.zero)
        {
            Vector3 f = Vector3.Cross(controller.currentGround.Hit.normal, Vector3.Cross(lookDirection, Vector3.up));

            RotateLookDirection(input.Current.MoveInput, turnSpeed);

            float acceleration = 20.0f * controller.deltaTime;

            moveSpeed = Mathf.MoveTowards(moveSpeed, runSpeed, acceleration);

            moveDirection = Math3d.SetVectorLength(f, moveSpeed);
        }
        else
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 15.0f * controller.deltaTime);
            moveDirection = Math3d.SetVectorLength(moveDirection, moveSpeed);
        }

        if (Timer(timeEnteredState, anim["turn"].length))
        {
            currentState = MarioStates.Idle;
        }
    }

    private Vector3 targetDirection;

    void Slide_EnterState()
    {
        targetDirection = SlopeDirection();

        Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);

        moveDirection = Math3d.ProjectVectorOnPlane(GroundNormal(), planarMoveDirection);
        moveDirection = Math3d.SetVectorLength(moveDirection, Mathf.Abs(moveSpeed));

        RunSmokeEffect.enableEmission = true;

        anim.Play("slide");

        sound.PlaySlide();
    }

    void Slide_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (!IsContinueSliding() && (input.Current.JumpDown || input.Current.StrikeDown))
        {
            currentState = MarioStates.SFlip;
            return;
        }

        if (!goldMario)
        {
            BodySlam();
        }
        else
        {
            GoldBodySlam();
        }

        SuperCollision col;

        Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);

        if (HasWallCollided(out col))
        {
            if (Vector3.Angle(-planarMoveDirection.normalized, col.normal) < 75.0f)
            {
                moveSpeed = -2.0f;
                lookDirection = Vector3.Reflect(-Math3d.ProjectVectorOnPlane(controller.up, moveDirection), col.normal).normalized;

                Instantiate(WallHitStarEffect, col.point, Quaternion.LookRotation(col.normal));

                sound.PlayHeavyKnockback();
                currentState = MarioStates.Knockback;
                return;
            }
        }

        Vector3 planarSlopeDirection = Math3d.ProjectVectorOnPlane(controller.up, SlopeDirection());

        Vector3 projectedInput = Math3d.ProjectVectorOnPlane(GroundNormal(), input.Current.MoveInput).normalized;

        bool movingDownSlope = Vector3.Angle(planarMoveDirection, planarSlopeDirection) < 90;
        bool feetFirst = Vector3.Angle(lookDirection, planarSlopeDirection) > 140;

        moveDirection = Math3d.ProjectVectorOnPlane(GroundNormal(), moveDirection);
        moveDirection = Math3d.SetVectorLength(moveDirection, Mathf.Abs(moveSpeed));

        if (IsContinueSliding())
        {
            if (movingDownSlope && input.Current.MoveInput != Vector3.zero && Vector3.Angle(input.Current.MoveInput, planarSlopeDirection) < 110.0f && Vector3.Angle(planarMoveDirection, planarSlopeDirection) < 70.0f)
            {
                targetDirection = Vector3.RotateTowards(targetDirection, projectedInput, turnSpeed * 0.4f * controller.deltaTime, 0);
                targetDirection = SuperMath.ClampAngleOnPlane(SlopeDirection(), targetDirection, 70.0f, GroundNormal());
            }
            else
            {
                targetDirection = SlopeDirection();
            }

            Vector3 horizontalMovement = Math3d.ProjectVectorOnPlane(targetDirection, moveDirection);
            Vector3 slopingMovement = moveDirection - horizontalMovement;

            horizontalMovement = Vector3.MoveTowards(horizontalMovement, Vector3.zero, 5.0f * controller.deltaTime);
            slopingMovement = Vector3.MoveTowards(slopingMovement, targetDirection * maxSlideSpeed, 20.0f * controller.deltaTime);

            moveDirection = horizontalMovement + slopingMovement;
            moveSpeed = moveDirection.magnitude;

            if (Vector3.Angle(lookDirection, planarSlopeDirection) > 90 && Vector3.Angle(lookDirection, planarMoveDirection) > 90)
            {
                moveSpeed *= -1;
            }

            if (movingDownSlope)
            {
                if (feetFirst)
                {
                    RotateLookDirection(-targetDirection, turnSpeed * 0.05f);
                }
                else
                {
                    RotateLookDirection(targetDirection, turnSpeed * 0.15f);
                }
            }
        }
        else
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 15 * controller.deltaTime);
            moveDirection = Math3d.SetVectorLength(moveDirection, Mathf.Abs(moveSpeed));

            if (input.Current.MoveInput != Vector3.zero)
            {
                moveDirection = Vector3.RotateTowards(moveDirection, projectedInput, turnSpeed * 0.1f * controller.deltaTime, 0);
            }

            if (moveSpeed < 0)
            {
                RotateLookDirection(-planarMoveDirection, turnSpeed * 0.1f);
            }
            else
            {
                RotateLookDirection(planarMoveDirection, turnSpeed * 0.1f);
            }

        }

        if (moveSpeed == 0)
        {
            currentState = MarioStates.SlideRecover;
            return;
        }

        if (Mathf.Abs(moveSpeed) < 4.0f)
        {
            RunSmokeEffect.enableEmission = false;
        }
        else
        {
            RunSmokeEffect.enableEmission = true;
        }

        artUpDirection = GroundNormal();
    }

    void Slide_ExitState()
    {
        sound.Stop();

        artUpDirection = controller.up;

        RunSmokeEffect.enableEmission = false;
    }

    private bool slopeJumping;
    private bool bouncing;
    private bool hasStruck;
    private Vector3 jumpPeak;

    void Jump_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();

        bouncing = false;

        hasStruck = false;

        if (currentJumpProfile != jumpTriple)
        {
            slopeJumping = IsSliding();
        }
        else
        {
            slopeJumping = false;
        }

        if (slopeJumping && moveSpeed > runSpeed * 0.5f)
        {
            moveSpeed = runSpeed * 0.5f;
        }

        verticalMoveSpeed = CalculateJumpSpeed(currentJumpProfile.JumpHeight, currentJumpProfile.Gravity);

        if (currentJumpProfile.InitialForwardVelocity != 0)
        {
            moveSpeed += currentJumpProfile.InitialForwardVelocity;
        }

        moveDirection += lookDirection * moveSpeed + controller.up * verticalMoveSpeed;

        anim.CrossFade(currentJumpProfile.Animation, currentJumpProfile.CrossFadeTime);

        jumpPeak = transform.position;

        PlayJumpSound();
    }

    void Jump_SuperUpdate()
    {
        if (currentJumpProfile != jumpLong && input.Current.TriggerDown)
        {
            currentState = MarioStates.GroundPoundPrepare;
            return;
        }
        if (currentJumpProfile == jumpKick)
        {
            if (!hasStruck && SuperMath.Timer(timeEnteredState, 0.05f) && !SuperMath.Timer(timeEnteredState, 1.2f))
            {
                GameObject struckObject;

                if (Strike(out struckObject, GetKickOrigin(), GetKickOffset(), controller.radius * 1.5f))
                {
                    TriggerableObject trigger = struckObject.GetComponent<TriggerableObject>();

                    EnemyMachine machine = struckObject.GetComponent<EnemyMachine>();                    

                    if (machine != null)
                    {
                        moveSpeed = -5.0f;

                        if (goldMario)
                        {
                            machine.GetStruck(lookDirection, 10.0f, 15.0f);
                            machine.MakeGold();
                        }
                        else
                        {
                            machine.GetStruck(lookDirection, 10.0f, 15.0f, 0.3f);
                        }

                        Instantiate(EnemyHitEffect, GetKickPosition(), Quaternion.LookRotation(-lookDirection));
                    }
                    else if (trigger != null)
                    {
                        moveSpeed = -12.0f;

                        trigger.Strike();

                        Instantiate(EnemyHitEffect, GetKickPosition(), Quaternion.LookRotation(-lookDirection));
                    }
                    else
                    {
                        moveSpeed = -12.0f;

                        Instantiate(EnemyHitEffect, GetKickPosition(), Quaternion.LookRotation(-lookDirection));
                    }

                    sound.PlayImpact();

                    hasStruck = true;

                    return;

                }
            }
        }

        if (verticalMoveSpeed < 0)
        {
            if (SuperMath.PointAbovePlane(controller.up, jumpPeak, transform.position))
            {
                jumpPeak = transform.position;
            }

            GameObject struckObject;

            if (FootStrike(out struckObject))
            {
                GoombaMachine goomba = struckObject.GetComponent<GoombaMachine>();

                if (goomba)
                {
                    goomba.KillEnemy();
                }

                verticalMoveSpeed = 10.0f;
                bouncing = true;
            }

            if (AcquiringGround())
            {
                currentState = MarioStates.Land;
                return;
            }

            Vector3 ledgePosition;
            GameObject grabbedLedge;

            if (CanGrabLedge(out ledgePosition, out grabbedLedge))
            {
                GrabLedge(ledgePosition);
                controller.currentlyClampedTo = grabbedLedge.transform;
                currentState = MarioStates.Hang;
                return;
            }

            if (currentJumpProfile.FallAnimation != null && !anim.IsPlaying(currentJumpProfile.FallAnimation))
            {
                anim.CrossFade(currentJumpProfile.FallAnimation, 0.2f);
            }
        }
        else
        {
            SuperCollision c;

            if (HasHeadCollided(out c))
            {
                verticalMoveSpeed = 0;

                var trigger = c.gameObject.GetComponent<TriggerableObject>();

                if (trigger != null)
                {
                    trigger.BottomHit();
                }
            }

            if (currentJumpProfile.CanControlHeight && !bouncing && !input.Current.Jump)
            {
                verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, 0, 160.0f * controller.deltaTime);
            }
        }

        SuperCollision col;

        Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);

        if (HasWallCollided(out col))
        {
            if (WallCollisionAngle(-col.normal, moveDirection) < 40.0f)
            {
                Vector3 r = Vector3.Cross(controller.up, col.normal);

                Vector3 relativeMagnitude = Math3d.ProjectVectorOnPlane(r, planarMoveDirection);

                if (relativeMagnitude.magnitude > 5.0f)
                {
                    Vector3 o = controller.OffsetPosition(controller.head.Offset) + (controller.up * controller.radius);

                    RaycastHit hit;

                    if (col.gameObject.GetComponent<Collider>().Raycast(new Ray(o, Math3d.ProjectVectorOnPlane(controller.up, col.point - o)), out hit, controller.radius * 1.5f))
                    {
                        if (Vector3.Angle(-planarMoveDirection.normalized, col.normal) < 40.0f && Vector3.Angle(controller.up, hit.normal) > 70.0f)
                        {
                            lookDirection = Vector3.Reflect(lookDirection, r);
                            moveSpeed = -2.0f;
                            verticalMoveSpeed = 0;

                            wallHitNormal = col.normal;
                            wallHitTime = Time.time;

                            sound.PlayWallHit();

                            currentState = MarioStates.Fall;
                            return;
                        }
                    }
                    else
                    {
                        moveSpeed = 0;
                    }
                }
                else
                {
                    moveSpeed = 0;
                }
            }
        }

        if (input.Current.Strike)
        {
            if (currentJumpProfile.CanKick && (moveSpeed < 9.0f || verticalMoveSpeed > CalculateJumpSpeed(currentJumpProfile.JumpHeight, currentJumpProfile.Gravity) * 0.85f) && !slopeJumping)
            {
                currentJumpProfile = jumpKick;
                currentState = MarioStates.Jump;
                return;
            }
            else if (currentJumpProfile.CanDive)
            {
                sound.PlayDive();
                currentState = MarioStates.Dive;
                return;
            }
        }

        Vector3 horizontalMovement = Vector3.zero;

        if (!slopeJumping)
        {
            if (input.Current.MoveInput != Vector3.zero)
            {
                Vector3 relativeMoveInput = Math3d.ProjectVectorOnPlane(Vector3.Cross(controller.up, lookDirection), input.Current.MoveInput);
                float relativeAcceleration = Mathf.Clamp(relativeMoveInput.magnitude, -0.7f, 0.7f) * 1/0.7f;

                float targetMovement;

                if (Vector3.Angle(lookDirection, relativeMoveInput) < 90)
                {
                    targetMovement = relativeAcceleration * Mathf.Max(runSpeed, currentJumpProfile == jumpLong ? runSpeed + currentJumpProfile.InitialForwardVelocity : runSpeed);
                }
                else
                {
                    targetMovement = relativeAcceleration * -runSpeed * 0.5f;
                }

                moveSpeed = Mathf.MoveTowards(moveSpeed, targetMovement, 14.0f * controller.deltaTime);

                horizontalMovement = Math3d.ProjectVectorOnPlane(lookDirection, input.Current.MoveInput) * 220.0f * controller.deltaTime;

            }
            else
            {
                moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 8.0f * controller.deltaTime);
            }
        }

        verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, -currentJumpProfile.MaximumGravity, currentJumpProfile.Gravity * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed + horizontalMovement;
    }

    void Jump_ExitState()
    {
        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;
    }

    void Land_EnterState()
    {
        lastLandTime = Time.time;

        controller.EnableClamping();
        controller.EnableSlopeLimit();

        verticalMoveSpeed = 0;

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;

        if (ShouldHaveFallDamage())
        {
            if (FallDamage())
            {
                currentState = MarioStates.KnockbackForwards;
                moveSpeed = 3.0f;
                return;
            }
        }

        sound.PlayLand();

        anim.CrossFade("land", 0.05f);
    }

    void Land_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (!isTakingFallDamage && input.Current.JumpDown)
        {
            if (!IsSliding() && currentJumpProfile == jumpLong && input.Current.Trigger)
            {
                currentJumpProfile = jumpLong;
            }
            else
            {
                currentJumpProfile = ResolveJump();
            }

            sound.Stop();

            currentState = MarioStates.Jump;
            return;
        }

        if (!isTakingFallDamage && input.Current.MoveInput != Vector3.zero)
        {
            if (Vector3.Angle(input.Current.MoveInput, lookDirection) > 110)
            {
                if (IsSliding())
                {
                    currentState = MarioStates.Slide;
                    return;
                }
                else
                {
                    if (moveSpeed > runSpeed * 0.66f)
                    {
                        currentState = MarioStates.Stop;
                        return;
                    }
                }
            }
            else
            {
                if (IsSliding())
                {
                    moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 10.0f * controller.deltaTime);
                }
                else
                {
                    moveSpeed = Mathf.MoveTowards(moveSpeed, runSpeed, runSpeed / 1.36f * controller.deltaTime);
                }
            }
        }
        else
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 80.0f * controller.deltaTime);
        }

        Vector3 f = Vector3.Cross(controller.currentGround.Hit.normal, Vector3.Cross(lookDirection, Vector3.up));
        moveDirection = Math3d.SetVectorLength(f, moveSpeed);

        if (Time.time > timeEnteredState + 0.10f)
        {
            currentState = MarioStates.Idle;
            return;
        }
    }

    void Dive_EnterState()
    {
        anim.Play("dive");

        controller.DisableClamping();
        controller.DisableSlopeLimit();

        moveSpeed += 6.1f;

        jumpPeak = transform.position;
    }

    void Dive_SuperUpdate()
    {
        SuperCollision col;
        Vector3 planarMoveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);

        if (SuperMath.PointAbovePlane(controller.up, jumpPeak, transform.position))
        {
            jumpPeak = transform.position;
        }

        if (HasWallCollided(out col))
        {
            if (Vector3.Angle(-planarMoveDirection.normalized, col.normal) < 75.0f)
            {
                moveSpeed = -2.0f;
                lookDirection = Vector3.Reflect(-lookDirection, col.normal);

                Instantiate(WallHitStarEffect, col.point, Quaternion.LookRotation(col.normal));

                sound.PlayHeavyKnockback();
                currentState = MarioStates.AirKnockback;
                return;
            }
        }

        if (AcquiringGround())
        {
            verticalMoveSpeed = 0;

            if (ShouldHaveFallDamage())
            {
                if (FallDamage())
                {
                    moveSpeed = Mathf.Clamp(5.0f, 0, moveSpeed);
                    verticalMoveSpeed = 0;
                    currentState = MarioStates.KnockbackForwards;
                    return;
                }
            }

            sound.PlayDiveLand();
            
            currentState = MarioStates.Slide;
            return;
        }

        if (!goldMario)
        {
            BodySlam();
        }
        else
        {
            GoldBodySlam();
        }

        Vector3 right = Vector3.Cross(controller.up, lookDirection);
        Vector3 horizontalMovement = Vector3.zero;

        if (input.Current.MoveInput != Vector3.zero)
        {
            Vector3 relativeMoveInput = Math3d.ProjectVectorOnPlane(right, input.Current.MoveInput);

            float targetMovement;

            if (Vector3.Angle(lookDirection, relativeMoveInput) < 90)
            {
                targetMovement = relativeMoveInput.magnitude * (runSpeed + 6.1f);
            }
            else
            {
                targetMovement = relativeMoveInput.magnitude * runSpeed * 0.5f;
            }

            moveSpeed = Mathf.MoveTowards(moveSpeed, targetMovement, 8.0f * controller.deltaTime);

            horizontalMovement = Math3d.ProjectVectorOnPlane(lookDirection, input.Current.MoveInput) * 220.0f * controller.deltaTime;

        }
        else
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 8.0f * controller.deltaTime);
        }

        verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, -30.0f, 35.0f * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed + horizontalMovement;

        Vector3 r = Vector3.Cross(lookDirection, controller.up);

        float ratio = verticalMoveSpeed < 0 ? Mathf.Abs(verticalMoveSpeed / 20.0f) : 0;
        float angle = Mathf.Lerp(0, 70.0f, ratio);

        artUpDirection = Quaternion.AngleAxis(-angle, r) * controller.up;
    }

    void Dive_ExitState()
    {
        artUpDirection = controller.up;

        controller.EnableClamping();
        controller.EnableSlopeLimit();
    }

    void SFlip_EnterState()
    {
        anim.Play("slide_flip");

        sound.PlaySlideSpin();

        currentJumpProfile = fallProfile;

        controller.DisableClamping();
        controller.DisableSlopeLimit();

        verticalMoveSpeed = 9.0f;

        jumpPeak = transform.position;

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;
    }

    void SFlip_SuperUpdate()
    {
        if (verticalMoveSpeed < 0 && AcquiringGround())
        {
            verticalMoveSpeed = 0;
            currentState = MarioStates.Land;
            return;
        }

        SuperCollision col;

        if (HasWallCollided(out col) && WallCollisionAngle(col.normal, moveDirection) < 65.0f)
        {
            moveSpeed = runSpeed * 0.2f;
        }

        verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, -currentJumpProfile.MaximumGravity, currentJumpProfile.Gravity * controller.deltaTime);

        if (Timer(timeEnteredState, anim["slide_flip"].length - 0.15f))
        {
            if (!anim.IsPlaying("fall"))
                anim.CrossFade("fall", 0.15f);
        }

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;
    }

    void SFlip_ExitState()
    {
        controller.EnableClamping();
        controller.EnableSlopeLimit();

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;
    }

    private Vector3 wallHitNormal;
    private float wallHitTime;

    void Fall_EnterState()
    {
        currentJumpProfile = fallProfile;

        jumpPeak = transform.position;

        controller.DisableClamping();
        controller.DisableSlopeLimit();

        anim.CrossFade("fall", 0.1f);
    }

    void Fall_SuperUpdate()
    {
        if (AcquiringGround())
        {
            verticalMoveSpeed = 0;

            currentState = MarioStates.Land;

            return;
        }

        SuperCollision col;

        if (HasWallCollided(out col) && WallCollisionAngle(col.normal, moveDirection) < 65.0f)
        {
            moveSpeed = runSpeed * 0.2f;
        }

        if (input.Current.JumpDown && Time.time < wallHitTime + 0.2f)
        {
            Instantiate(WallKickSmokeEffect, transform.position + controller.up * (controller.height + controller.radius * 2) * 0.75f, Quaternion.LookRotation(wallHitNormal));

            lookDirection *= -1;
            moveSpeed = 0;
            currentJumpProfile = jumpWall;
            currentState = MarioStates.Jump;
            return;
        }

        verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, -25.0f, 35.0f * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;
    }

    void Fall_ExitState()
    {
        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;

        controller.EnableClamping();
        controller.EnableSlopeLimit();
    }

    void Knockback_EnterState()
    {
        anim.Play("ground_knockback");
    }

    void Knockback_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.AirKnockback;
            return;
        }

        moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 3.0f * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed);

        if (moveSpeed == 0)
        {
            if (status.CurrentHealth == 0)
            {
                currentState = MarioStates.DeathFront;
                return;
            }
            else
            {
                currentState = MarioStates.KnockbackRecover;
                return;
            }
        }
    }

    void KnockbackForwards_EnterState()
    {
        if (anim.IsPlaying("air_knockback_back"))
        {
            anim["ground_knockback_back"].time = 0.21f;
        }

        anim.Play("ground_knockback_back");
    }

    void KnockbackForwards_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.AirKnockbackForwards;
            return;
        }

        moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 3.0f * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed);

        if (moveSpeed == 0)
        {
            if (status.CurrentHealth == 0)
            {
                currentState = MarioStates.DeathBack;
                return;
            }
            else
            {
                currentState = MarioStates.KnockbackForwardsRecover;
                return;
            }
        }
    }

    void AirKnockback_EnterState()
    {
        anim.Play("air_knockback");

        controller.DisableClamping();
        controller.DisableSlopeLimit();

        verticalMoveSpeed = -1.0f;
    }

    void AirKnockback_SuperUpdate()
    {
        if (AcquiringGround())
        {
            verticalMoveSpeed = 0;

            controller.EnableClamping();
            controller.EnableSlopeLimit();

            Instantiate(GroundFallSmokeEffect, transform.position, Quaternion.identity);

            sound.PlayLandHurt();

            currentState = MarioStates.Knockback;

            return;
        }

        verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, -25.0f, 35.0f * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;
    }

    void AirKnockbackForwards_EnterState()
    {
        anim.Play("air_knockback_back");

        controller.DisableClamping();
        controller.DisableSlopeLimit();

        verticalMoveSpeed = -1.0f;
    }

    void AirKnockbackForwards_SuperUpdate()
    {
        if (AcquiringGround())
        {
            verticalMoveSpeed = 0;

            controller.EnableClamping();
            controller.EnableSlopeLimit();

            Instantiate(GroundFallSmokeEffect, transform.position, Quaternion.identity);

            sound.PlayLandHurt();

            currentState = MarioStates.KnockbackForwards;

            return;
        }

        verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, -25.0f, 35.0f * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;
    }

    private bool exitCrouch;
    private float crouchExitTime;

    void Crouch_EnterState()
    {
        exitCrouch = false;

        anim.CrossFade("crouch", 0.1f);
    }

    void Crouch_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (IsSliding())
        {
            currentState = MarioStates.Slide;
            return;
        }

        if (!exitCrouch)
        {
            if (input.Current.JumpDown)
            {
                if (moveSpeed != 0 && !Timer(timeEnteredState, 0.4f))
                {
                    currentJumpProfile = jumpLong;
                    currentState = MarioStates.Jump;
                }
                else if (moveSpeed != 0)
                {
                    currentJumpProfile = jumpStandard;
                    currentState = MarioStates.Jump;
                }
                else
                {
                    currentJumpProfile = jumpBackFlip;
                    currentState = MarioStates.Jump;
                }
                return;
            }

            if (Time.time > timeEnteredState + anim["crouch"].length)
            {
                if (!anim.IsPlaying("crouch_idle"))
                    anim.Play("crouch_idle");

                if (!input.Current.Trigger && moveDirection == Vector3.zero)
                {
                    exitCrouch = true;
                    crouchExitTime = Time.time;

                    anim["crouch"].speed = -1;
                    anim["crouch"].time = anim["crouch"].length;
                    anim.Play("crouch");

                    return;
                }

                if (GroundAngle() > 30.0f)
                {
                    moveDirection = Vector3.MoveTowards(moveDirection, SlopeDirection() * 13.0f, controller.deltaTime * 30.0f);
                }
                else
                {
                    moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 15 * controller.deltaTime);

                    moveDirection = Math3d.SetVectorLength(moveDirection, Mathf.Abs(moveSpeed));
                }
            }
        }
        else
        {
            if (Time.time > crouchExitTime + anim["crouch"].length)
            {
                exitCrouch = false;
                anim["crouch"].speed = 1;
                anim.Play("idle");
                currentState = MarioStates.Idle;
                return;
            }
        }
    }

    void Hang_EnterState()
    {
        moveDirection = Vector3.zero;

        sound.PlayHang();

        anim.Play("hang");
    }

    void Hang_SuperUpdate()
    {
        if (input.Current.JumpDown)
        {
            currentState = MarioStates.Climb;
            return;
        }

        if (input.Current.Trigger)
        {
            moveSpeed = -2.0f;
            verticalMoveSpeed = -3.0f;
            currentState = MarioStates.Fall;
            controller.currentlyClampedTo = null;

            sound.PlayWallHit();

            return;
        }

        Vector3 ledgePosition;
        GameObject grabbedLedge;

        if (!CanGrabLedge(out ledgePosition, out grabbedLedge))
        {
            moveSpeed = -2.0f;
            verticalMoveSpeed = -3.0f;
            currentState = MarioStates.Fall;
            controller.currentlyClampedTo = null;
            return;
        }

        if (SuperMath.Timer(timeEnteredState, 0.25f))
        {
            if (input.Current.MoveInput != Vector3.zero)
            {
                if (Vector3.Angle(lookDirection, input.Current.MoveInput) < 90.0f)
                {
                    currentState = MarioStates.Climb;
                    return;
                }
                else
                {
                    moveSpeed = -2.0f;
                    verticalMoveSpeed = -3.0f;
                    currentState = MarioStates.Fall;
                    controller.currentlyClampedTo = null;

                    sound.PlayWallHit();
                    return;
                }
            }
        }
    }

    void Climb_EnterState()
    {
        anim["climb"].speed = 1.5f;

        sound.PlayClimb();

        anim.CrossFade("climb", 0.1f);
    }

    void Climb_SuperUpdate()
    {
        if (SuperMath.Timer(timeEnteredState, anim["climb"].length / 1.5f))
        {
            transform.position = ClimbTarget();

            anim.Play("idle");

            currentState = MarioStates.Idle;
        }
    }

    void Climb_ExitState()
    {
        controller.EnableClamping();
        controller.EnableSlopeLimit();

        controller.currentlyClampedTo = null;
    }

    void SlideRecover_EnterState()
    {
        anim.Play("slide_recover");
    }

    void SlideRecover_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (Timer(timeEnteredState, anim["slide_recover"].length))
        {
            currentState = MarioStates.Idle;
        }
    }

    void KnockbackRecover_EnterState()
    {
        anim.Play("ground_recover");
    }

    void KnockbackRecover_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (Timer(timeEnteredState, anim["ground_recover"].length))
        {
            currentState = MarioStates.Idle;
        }
    }

    void KnockbackRecover_ExitState()
    {
        status.StartInvincible();
    }

    void KnockbackForwardsRecover_EnterState()
    {
        anim.Play("ground_knockback_back_recover");
    }

    void KnockbackForwardsRecover_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (Timer(timeEnteredState, anim["ground_knockback_back_recover"].length))
        {
            currentState = MarioStates.Idle;
        }
    }

    void KnockbackForwardsRecover_ExitState()
    {
        status.StartInvincible();
    }

    void GroundPoundPrepare_EnterState()
    {
        anim.Play("bum_drop");

        sound.StopVoices();
        sound.PlaySpin();

        verticalMoveSpeed = 2.0f;
        moveSpeed = 0.0f;

        jumpPeak = transform.position;
    }

    void GroundPoundPrepare_SuperUpdate()
    {
        moveDirection = controller.up * verticalMoveSpeed;

        if (Timer(timeEnteredState, anim["bum_drop"].length))
        {
            currentState = MarioStates.GroundPound;
        }
    }

    void GroundPound_EnterState()
    {
        sound.PlayGroundPoundDown();

        verticalMoveSpeed = -20.0f;
    }

    void GroundPound_SuperUpdate()
    {
        moveDirection = controller.up * verticalMoveSpeed;

        GameObject struckObject;

        if (FootStrike(out struckObject))
        {
            var machine = struckObject.GetComponent<EnemyMachine>();

            if (machine)
                machine.KillEnemy();
        }

        bool alreadyTriggered = false;

        if (AcquiringGround())
        {
            if (!alreadyTriggered)
            {
                var trigger = controller.currentGround.Transform.GetComponent<TriggerableObject>();

                if (trigger != null)
                {
                    if (trigger.GroundPound())
                    {
                        return;
                    }
                }
            }

            currentState = MarioStates.GroundPoundRecover;
            return;
        }
    }

    void GroundPoundRecover_EnterState()
    {
        controller.EnableClamping();
        controller.EnableSlopeLimit();

        Instantiate(GroundPoundSmokeEffect, transform.position + controller.up * 0.15f, Quaternion.identity);

        sound.PlayGroundPound();

        if (ShouldHaveFallDamage())
        {
            if (FallDamage())
            {
                currentState = MarioStates.Knockback;
                return;
            }
        }

        verticalMoveSpeed = 0.0f;
        moveDirection = controller.up * verticalMoveSpeed;

        anim.Play("bum_drop_recover");

        SmartCamera.Shake(0.25f, 5.0f, 0.25f);
    }

    void GroundPoundRecover_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        if (Timer(timeEnteredState, anim["bum_drop_recover"].length * 0.25f))
        {
            currentState = MarioStates.Idle;
        }
    }

    void Strike_EnterState()
    {
        strikeCount++;

        if (strikeCount == 1 && moveSpeed == 0)
        {
            moveSpeed = runSpeed / 3.0f;
        }

        anim.Rewind(ResolveStrike());
        anim.Play(ResolveStrike());

        if (strikeCount == 1)
            sound.PlaySinglePunch();
        else if (strikeCount == 2)
            sound.PlayDoublePunch();
        else
            sound.PlayKick();

        hasStruck = false;
    }

    int strikeCount = 0;

    void Strike_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        float decceleration = SuperMath.BoundedInterpolation(new float[] { 0 }, new float[] { 80, 10 }, moveSpeed);
        moveSpeed = Mathf.MoveTowards(moveSpeed, 0, decceleration * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed);

        if (!hasStruck && Timer(timeEnteredState, anim[ResolveStrike()].length - 0.12f))
        {
            GameObject struckObject;

            if (Strike(out struckObject, GetPunchOrigin(), GetPunchOffset()))
            {
                TriggerableObject trigger = struckObject.GetComponent<TriggerableObject>();
                EnemyMachine machine = struckObject.GetComponent<EnemyMachine>();

                if (trigger != null)
                {
                    trigger.Strike();

                    Instantiate(EnemyHitEffect, GetPunchPosition(), Quaternion.LookRotation(-lookDirection));
                }

                if (machine != null)
                {
                    if (goldMario)
                    {
                        machine.MakeGold();
                    }

                    machine.GetStruck(lookDirection, 20.0f, 5.0f);

                    Instantiate(EnemyHitEffect, GetPunchPosition(), Quaternion.LookRotation(-lookDirection));
                }
                else
                {
                    Instantiate(EnemyHitEffect, GetPunchPosition(), Quaternion.LookRotation(-lookDirection));
                }

                sound.PlayImpact();
                hasStruck = true;
                moveSpeed = -13.0f;
            }
        }

        if (Timer(timeEnteredState, anim[ResolveStrike()].length))
        {
            if (!anim.IsPlaying("idle"))
            {
                anim.CrossFade("idle", 0.3f);
            }

            if (input.Current.StrikeDown && strikeCount != 3)
            {
                currentState = MarioStates.Strike;
                return;
            }
        }

        if (Timer(timeEnteredState, anim[ResolveStrike()].length + 0.2f))
        {
            strikeCount = 0;
            currentState = MarioStates.Idle;
            return;
        }
    }

    bool staggerForward;

    void Stagger_EnterState()
    {
        controller.EnableClamping();
        controller.EnableSlopeLimit();

        if (staggerForward)
        {
            anim.CrossFade("hit_light_front", 0.15f);
        }
        else
        {
            anim.CrossFade("hit_light_back", 0.15f);
        }
    }

    void Stagger_SuperUpdate()
    {
        if (!MaintainingGround())
        {
            currentState = MarioStates.Fall;
            return;
        }

        moveSpeed = Mathf.MoveTowards(moveSpeed, 0, 8.0f * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed);

        if (moveSpeed == 0)
        {
            currentState = MarioStates.Idle;
            return;
        }
    }

    void Stagger_ExitState()
    {
        status.StartInvincible();
    }

    Vector3 teleportTarget;

    void TeleportOut_EnterState()
    {
        moveSpeed = 0;
        verticalMoveSpeed = 0;
        moveDirection = Vector3.zero;

        anim.CrossFade("idle", 0.3f);

        if (!goldMario)
        {
            transparencyShaderSwapper.SwapNew();
            transparencyFade.FadeOut(1);
        }

        sound.PlayTeleport();

        GameObject.FindObjectOfType<GameMaster>().FadeWhiteMatteOut(1.5f);
    }

    void TeleportOut_SuperUpdate()
    {
        if (SuperMath.Timer(timeEnteredState, 2))
        {
            transform.position = teleportTarget;

            var boulders = GameObject.FindObjectsOfType<RollingBallPath>();

            foreach (var boulder in boulders)
            {
                if (Vector3.Distance(boulder.transform.position, transform.position) < 10.0f)
                {
                    Destroy(boulder.gameObject);
                }
            }

            currentState = MarioStates.TeleportIn;
            return;
        }
    }

    void TeleportIn_EnterState()
    {
        if (!goldMario)
            transparencyFade.FadeIn(1);

        sound.PlayTeleport();

        GameObject.FindObjectOfType<GameMaster>().FadeWhiteMatteIn(1.5f);
    }

    void TeleportIn_SuperUpdate()
    {
        if (SuperMath.Timer(timeEnteredState, 1))
        {
            currentState = MarioStates.Idle;
            return;
        }
    }

    void TeleportIn_ExitState()
    {
        if (!goldMario)
            transparencyShaderSwapper.SwapOriginal();
    }

    void EnterLevel_EnterState()
    {
        anim["enter_level"].speed *= 1.5f;

        anim.Play("enter_level");

        sound.PlayFlipIntoLevel();

        // verticalMoveSpeed = -5.0f;

        moveDirection = verticalMoveSpeed * controller.up;

        controller.DisableClamping();
        controller.DisableSlopeLimit();
    }

    void EnterLevel_SuperUpdate()
    {
        jumpPeak = transform.position;

        verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, -jumpStandard.MaximumGravity, jumpStandard.Gravity * controller.deltaTime);

        moveDirection = verticalMoveSpeed * controller.up;

        if (AcquiringGround())
        {
            currentState = MarioStates.Land;
            return;
        }
    }

    void MegaSpring_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();

        anim.Play(jumpDouble.Animation);
    }

    void MegaSpring_SuperUpdate()
    {
        jumpPeak = transform.position;

        if (verticalMoveSpeed < 0 && AcquiringGround())
        {
            currentState = MarioStates.Land;
            return;
        }

        if (!anim.IsPlaying(jumpDouble.FallAnimation))
        {
            anim.CrossFade(jumpDouble.FallAnimation, 0.2f);
        }

        verticalMoveSpeed = Mathf.MoveTowards(verticalMoveSpeed, -jumpStandard.MaximumGravity, jumpStandard.Gravity * controller.deltaTime);

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;
    }

    void DeathFront_EnterState()
    {
        anim.Play("death_front");

        sound.PlayDie();

        GameObject.FindObjectOfType<GameMaster>().GameOver();
    }

    void DeathBack_EnterState()
    {
        anim.Play("death_back");

        sound.PlayDie();

        GameObject.FindObjectOfType<GameMaster>().GameOver();
    }
}
