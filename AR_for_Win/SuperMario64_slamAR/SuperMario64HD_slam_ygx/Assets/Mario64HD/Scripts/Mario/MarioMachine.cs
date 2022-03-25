/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

[RequireComponent(typeof(MarioInput))]
[RequireComponent(typeof(SuperCharacterController))]
public partial class MarioMachine : SuperStateMachine
{
    public enum MarioStates
    {
        Idle,
        Run,
        Stop,
        Turn,
        Slide,
        Jump,
        Land,
        Dive,
        SFlip,
        Fall,
        Knockback,
        KnockbackForwards,
        AirKnockback,
        AirKnockbackForwards,
        KnockbackRecover,
        KnockbackForwardsRecover,
        Crouch,
        Hang,
        Climb,
        SlideRecover,
        GroundPoundPrepare,
        GroundPound,
        GroundPoundRecover,
        Strike,
        Stagger,
        TeleportOut,
        TeleportIn,
        EnterLevel,
        MegaSpring,
        DeathFront,
        DeathBack
    }

    public class JumpProfile
    {
        public bool CanDive = false;
        public bool CanKick = false;
        public bool CanControlHeight = false;
        public float JumpHeight;
        public float InitialForwardVelocity;
        public float Gravity = 35.0f;
        public float MaximumGravity = 30.0f;
        public string Animation;
        public string FallAnimation;
        public float CrossFadeTime;
    }

    void Start()
    {
        input = GetComponent<MarioInput>();
        status = GetComponent<MarioStatus>();
        sound = GetComponent<MarioSound>();
        controller = GetComponent<SuperCharacterController>();
        anim = AnimatedMesh.GetComponent<Animation>();
        transparencyShaderSwapper = AnimatedMesh.GetComponent<ShaderSwapper>();
        goldMaterialSwapper = AnimatedMesh.GetComponent<MaterialSwapper>();
        transparencyFade = AnimatedMesh.GetComponent<TransparencyFade>();

        RunSmokeEffect.enableEmission = false;

        anim["run_redux"].speed = 1.7f;

        lookDirection = Quaternion.AngleAxis(InitialRotation, controller.up) * Vector3.forward;

        artUpDirection = controller.up;

        currentState = MarioStates.EnterLevel;
    }


    protected override void EarlyGlobalSuperUpdate()
    {
    }

    protected override void LateGlobalSuperUpdate()
    {
        // Trigger any objects we are directly standing on
        if (controller.IsClamping())
        {
            TriggerableObject triggerable = controller.currentGround.Transform.GetComponent<TriggerableObject>();

            if (triggerable != null)
            {
                triggerable.StandingOn(transform.position);
            }
        }

        if (goldMario)
        {
            GoldBodySlam();
        }

        // Increment our position based on our current velocity
        transform.position += moveDirection * controller.deltaTime;

        if (DebugGui)
        {
            DebugDraw.DrawVector(transform.position, lookDirection, 2.0f, 1.0f, Color.red, 0, true);
            DebugDraw.DrawVector(transform.position, input.Current.MoveInput, 1.0f, 1.0f, Color.yellow, 0, true);
            DebugDraw.DrawVector(transform.position, moveDirection, 1.0f, 1.0f, Color.blue, 0, true);
        }

        // Rotate the chest forwards or side to side
        ChestBone.Rotation = Quaternion.Euler(new Vector3(0, chestTwistAngle, chestBendAngle));

        // Angle the controller's mesh based on current art direction
        Vector3 projected = Math3d.ProjectVectorOnPlane(artUpDirection, lookDirection);
        AnimatedMesh.rotation = Quaternion.LookRotation(projected, artUpDirection);

        // End fall damage if the animation has finished playing. If not, scale the
        // animated mesh based on the animation
        if (!ScaleAnimator.GetComponent<Animation>().isPlaying)
            isTakingFallDamage = false;

        if (isTakingFallDamage)
            AnimatedMesh.localScale = new Vector3(AnimatedMesh.localScale.x, ScaleAnimator.localScale.z, AnimatedMesh.localScale.z);
        else
            AnimatedMesh.localScale = new Vector3(1, 1, 1);

        // Check if we have fallen through the level, and place us on the first contacted ground if we have and return Mario to idle
        if (transform.position.y < -10)
        {
            RaycastHit hit;

            Vector3 verticalPostionZeroed = transform.position;
            verticalPostionZeroed.y = 0;

            if (Physics.Raycast(verticalPostionZeroed + Vector3.up * 5000, -Vector3.up, out hit, Mathf.Infinity, controller.Walkable))
            {
                transform.position = hit.point;

                verticalMoveSpeed = 0;
                moveSpeed = 0;
                moveDirection = Vector3.zero;

                currentState = MarioStates.Idle;
                return;
            }
        }
    }

    public bool Airborn()
    {
        if (StateCompare(MarioStates.Jump) ||
            StateCompare(MarioStates.Fall) ||
            StateCompare(MarioStates.AirKnockback) ||
            StateCompare(MarioStates.Dive) ||
            StateCompare(MarioStates.GroundPound) ||
            StateCompare(MarioStates.GroundPoundPrepare) ||
            StateCompare(MarioStates.SFlip) ||
            StateCompare(MarioStates.MegaSpring))
        {
            return true;
        }

        return false;
    }

    public Vector3 Velocity()
    {
        return moveDirection;
    }

    /// <summary>
    /// When hanging from a ledge, the target position Mario will move to after finishing the climb
    /// </summary>
    public Vector3 ClimbTarget()
    {
        return transform.position + controller.up * (controller.height + controller.radius) + lookDirection * controller.radius * 1.5f;
    }

    public bool StateCompare(Enum state)
    {
        return (MarioStates)state == (MarioStates)currentState;
    }

    public void Teleport(Vector3 target)
    {
        if (StateCompare(MarioStates.Idle) && Velocity().magnitude < 1.0f)
        {
            teleportTarget = target;
            currentState = MarioStates.TeleportOut;
        }
    }

    public void MegaSpring(Vector3 direction, float velocity, float lift)
    {
        lookDirection = Math3d.ProjectVectorOnPlane(controller.up, direction.normalized);

        moveSpeed = velocity;
        verticalMoveSpeed = lift;

        moveDirection = Math3d.SetVectorLength(lookDirection, moveSpeed) + controller.up * verticalMoveSpeed;

        currentState = MarioStates.MegaSpring;
        return;
    }

    public bool HeavyDamage(int damage, Vector3 origin)
    {
        if (status.Invincible())
        {
            return false;
        }

        if (StateCompare(MarioStates.Knockback) || StateCompare(MarioStates.KnockbackForwards) || StateCompare(MarioStates.TeleportIn) || StateCompare(MarioStates.TeleportOut))
        {
            return false;
        }

        Vector3 direction = Math3d.ProjectVectorOnPlane(controller.up, origin - transform.position).normalized;

        if (direction == Vector3.zero)
        {
            direction = lookDirection;
        }

        bool forward = Vector3.Angle(direction, lookDirection) < 90;

        if (Airborn())
        {
            if (forward)
            {
                moveSpeed = -3.0f;
                currentState = MarioStates.AirKnockback;
            }
            else
            {
                moveSpeed = 3.0f;
                currentState = MarioStates.AirKnockbackForwards;
            }
        }
        else
        {

            if (forward)
            {
                currentState = MarioStates.Knockback;
                moveSpeed = -3.0f;
            }
            else
            {
                currentState = MarioStates.KnockbackForwards;
                moveSpeed = 3.0f;
            }
        }

        lookDirection = forward ? direction : -direction;

        SmartCamera.Shake(1.6f, 25.0f, 0.5f);

        sound.PlayTakeDamage();

        Instantiate(TakeDamageEffect, transform.position + controller.up * controller.height * 0.6f, Quaternion.identity);

        status.TakeDamage(damage);

        return true;
    }

    public bool GroundDamageLight(int damage, Vector3 origin, bool canHurtInAir=true)
    {
        return GroundDamageLight(damage, origin, 4.0f, canHurtInAir);
    }

    public bool GroundDamageLight(int damage, Vector3 origin, float pushbackSpeed, bool canHurtInAir=true)
    {
        if (status.Invincible())
        {
            return false;
        }

        if (StateCompare(MarioStates.Stagger) || StateCompare(MarioStates.TeleportIn) || StateCompare(MarioStates.TeleportOut))
        {
            return false;
        }

        if (!canHurtInAir && Airborn())
        {
            return false;
        }

        if (StateCompare(MarioStates.Slide))
        {
            if (Mathf.Abs(moveSpeed) > 3.0f)
            {
                return false;
            }
        }

        status.TakeDamage(damage);

        Vector3 direction = Math3d.ProjectVectorOnPlane(controller.up, origin - transform.position).normalized;

        if (direction == Vector3.zero)
        {
            direction = lookDirection;
        }

        staggerForward = Vector3.Angle(direction, lookDirection) < 90;

        if (Airborn())
        {
            if (staggerForward)
            {
                moveSpeed = -3.0f;
                currentState = MarioStates.AirKnockback;
            }
            else
            {
                moveSpeed = 3.0f;
                currentState = MarioStates.AirKnockbackForwards;
            }
        }
        else
        {
            if (status.CurrentHealth == 0)
            {
                if (staggerForward)
                {
                    currentState = MarioStates.Knockback;
                    moveSpeed = -3.0f;
                }
                else
                {
                    currentState = MarioStates.KnockbackForwards;
                    moveSpeed = 3.0f;
                }
            }
            else
            {
                currentState = MarioStates.Stagger;

                moveSpeed = staggerForward ? -pushbackSpeed : pushbackSpeed;
            }
        }

        Instantiate(TakeDamageEffect, transform.position + controller.up * controller.height * 0.6f, Quaternion.identity);

        sound.PlayTakeDamage();

        lookDirection = staggerForward ? direction : -direction;

        SmartCamera.Shake(0.85f, 15.0f, 0.5f);

        return true;
    }

    // Bling bling
    public void GoldMarioUpgrade()
    {
        sound.PlayUpgrade();
        goldMaterialSwapper.SwapNew();

        var t = ((GameObject)Instantiate(UpgradeEffect, transform.position + controller.up * controller.height * 0.75f, Quaternion.identity)).transform;

        t.parent = transform;

        status.PermanentInvincibility = true;

        goldMario = true;
    }

    private void RotateLookDirection(Vector3 target, float speed)
    {
        lookDirection = Vector3.RotateTowards(lookDirection, target, speed * controller.deltaTime, 0);
    }

    private void RotateMoveDirection(Vector3 target, float speed)
    {
        moveDirection = Vector3.RotateTowards(moveDirection, target, speed * controller.deltaTime, 0);
    }

    private float GroundAngle()
    {
        return Vector3.Angle(controller.up, controller.currentGround.FarHit.normal);
    }

    private Vector3 GroundNormal()
    {
        return controller.currentGround.FarHit.normal;
    }

    private bool IsSliding()
    {
        return GroundAngle() > ((MarioCollisionType)controller.currentGround.CollisionType).SlideAngle;
    }

    private bool IsContinueSliding()
    {
        return GroundAngle() > ((MarioCollisionType)controller.currentGround.CollisionType).SlideContinueAngle;
    }

    private Vector3 SlopeDirection()
    {
        Vector3 n = controller.currentGround.Hit.normal;
        Vector3 r = Vector3.Cross(n, controller.down);
        return Vector3.Cross(r, n);
    }

    private bool HasWallCollided(out SuperCollision superCollision)
    {
        foreach (var col in controller.collisionData)
        {
            if (Vector3.Angle(col.normal, controller.up) > 80.0f)
            {
                superCollision = col;
                return true;
            }
        }

        superCollision = new SuperCollision();
        return false;
    }

    private bool HasHeadCollided()
    {
        SuperCollision c;
        return HasHeadCollided(out c);
    }

    private bool HasHeadCollided(out SuperCollision superCollision)
    {
        foreach (var col in controller.collisionData)
        {
            Vector3 direction = col.point - controller.OffsetPosition(controller.head.Offset);

            if (Vector3.Angle(direction, controller.up) < 88.0f)
            {
                superCollision = col;
                return true;
            }
        }

        superCollision = new SuperCollision();
        return false;
    }

    private bool HasFeetCollided()
    {
        SuperCollision c;
        return HasFeetCollided(out c);
    }

    private bool HasFeetCollided(out SuperCollision superCollision)
    {
        foreach (var col in controller.collisionData)
        {
            Vector3 direction = col.point - controller.OffsetPosition(controller.feet.Offset);

            if (Vector3.Angle(direction, controller.down) < 88.0f)
            {
                superCollision = col;
                return true;
            }
        }

        superCollision = new SuperCollision();
        return false;
    }

    private float WallCollisionAngle(Vector3 wallNormal, Vector3 direction)
    {
        Vector3 planarDirection = Math3d.ProjectVectorOnPlane(controller.up, direction);
        Vector3 planarWall = Math3d.ProjectVectorOnPlane(controller.up, wallNormal);

        return Vector3.Angle(planarWall, planarDirection);
    }

    /// <summary>
    /// Checks if there exists a wall in front of us as well as a flat surface to finish the climb on
    /// </summary>
    private bool CanGrabLedge(out Vector3 hitPosition, out GameObject grabbedObject)
    {
        hitPosition = Vector3.zero;

        grabbedObject = null;

        Vector3 o = controller.OffsetPosition(controller.head.Offset);

        Collider[] colliders = Physics.OverlapSphere(o, controller.radius + 0.2f, controller.Walkable);

        if (colliders.Length > 0)
        {
            foreach (var col in colliders)
            {
                SuperCollisionType type = col.GetComponent<SuperCollisionType>();

                Vector3 closestPoint = SuperCollider.ClosestPointOnSurface(col, o, controller.radius);

                RaycastHit hit;

                col.Raycast(new Ray(o, closestPoint - o), out hit, Mathf.Infinity);

                if (Vector3.Angle(hit.normal, controller.up) < type.StandAngle)
                {
                    continue;
                }

                if (Vector3.Angle(-hit.normal, lookDirection) < 60.0f)
                {
                    Vector3 topOfHead = o + controller.up * controller.radius;
                    Vector3 direction = Math3d.ProjectVectorOnPlane(controller.up, closestPoint - o);
                    Vector3 rayOrigin = topOfHead + Math3d.AddVectorLength(direction, 0.02f);

                    col.Raycast(new Ray(rayOrigin, controller.down), out hit, 0.5f);

                    if (Vector3.Angle(hit.normal, controller.up) < 20.0f)
                    {
                        hitPosition = Math3d.ProjectPointOnPlane(controller.up, hit.point, topOfHead + direction);
                        grabbedObject = col.gameObject;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if Mario is currently standing on level ground. Refer to the SuperCharacterController documentation for a more
    /// in depth summary of this method.
    /// </summary>
    /// <param name="distance">Maximum distance ground can be below before automatically returning false</param>
    /// <param name="currentlyGrounded">At the time the method is being called, is our controller currently grounded (or airborn and unclamped).
    /// Typically characters already grounded should have a larger ground distance before being counted as ungrounded to aid movement over uneven terrain</param>
    private bool IsGrounded(float distance, bool currentlyGrounded)
    {
        if (controller.currentGround.Hit.distance > distance)
        {
            return false;
        }

        Vector3 n = controller.currentGround.FarHit.normal;
        float angle = Vector3.Angle(n, Vector3.up);

        if (angle > controller.currentGround.CollisionType.StandAngle)
        {
            return false;
        }

        float upperBoundAngle = 60.0f;

        float maxDistance = 0.96f;
        float minDistance = 0.50f;

        float angleRatio = angle / upperBoundAngle;

        float distanceRatio = Mathf.Lerp(minDistance, maxDistance, angleRatio);

        Vector3 p = Math3d.ProjectPointOnPlane(controller.up, transform.position, controller.currentGround.Hit.point);

        bool steady = Vector3.Distance(p, transform.position) <= distanceRatio * controller.radius;

        if (!steady)
        {
            if (!currentlyGrounded)
            {
                return false;
            }

            if (controller.currentGround.NearHit.distance < distance)
            {
                if (Vector3.Angle(controller.currentGround.NearHit.normal, controller.up) > controller.currentGround.CollisionType.StandAngle)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private bool AcquiringGround()
    {
        return IsGrounded(acquireGroundDistance, false);
    }

    private bool MaintainingGround()
    {
        return IsGrounded(maintainGroundDistance, true);
    }

    private void GrabLedge(Vector3 ledgePosition)
    {
        Vector3 ledgeDirection = Math3d.ProjectVectorOnPlane(controller.up, transform.position - ledgePosition);

        lookDirection = -ledgeDirection.normalized;

        transform.position = ledgePosition + controller.radius * ledgeDirection.normalized;
        transform.position += controller.down * (controller.height + controller.radius + 0.05f);

        moveSpeed = 0;
        verticalMoveSpeed = 0;
    }

    private string ResolveStrike()
    {
        if (strikeCount == 1)
            return "punch_single";
        else if (strikeCount == 2)
            return "punch_double";
        else
            return "kick_triple";
    }

    private JumpProfile ResolveJump()
    {
        if (Time.time > lastLandTime + 0.2f)
        {
            return jumpStandard;
        }

        if (currentJumpProfile == jumpStandard && IsSliding())
        {
            return jumpStandard;
        }

        if (currentJumpProfile == jumpStandard || currentJumpProfile == jumpSideFlip || currentJumpProfile == jumpWall || currentJumpProfile == jumpKick)
        {
            return jumpDouble;
        }

        if (currentJumpProfile == jumpDouble)
        {
            Vector3 projected = Vector3.Project(moveDirection, lookDirection);

            if (Vector3.Angle(projected, lookDirection) < 90.0f && moveSpeed > runSpeed * 0.9f)
            {
                return jumpTriple;
            }
        }

        return jumpStandard;
    }

    private Vector3 GetPunchPosition()
    {
        return GetPunchOrigin() + GetPunchOffset();
    }

    private Vector3 GetPunchOrigin()
    {
        return transform.position + (controller.up * controller.height * 0.5f);
    }

    private Vector3 GetPunchOffset()
    {
        return lookDirection * controller.radius * 1.6f;
    }

    private Vector3 GetKickPosition()
    {
        return GetKickOrigin() + GetKickOffset();
    }

    private Vector3 GetKickOrigin()
    {
        return transform.position + (controller.up * controller.height * 0.2f);
    }

    private Vector3 GetKickOffset()
    {
        return lookDirection * controller.radius * 1.6f;
    }

    private bool Strike(out GameObject struckObject, Vector3 origin, Vector3 offset, float radius=0)
    {
        struckObject = null;

        if (radius == 0)
            radius = controller.radius;
        
        // Vector3 o = transform.position + (controller.Up * controller.Height * 0.5f);
        Vector3 center = origin + offset;

        Collider[] colliders = Physics.OverlapSphere(center, radius, controller.Walkable | EnemyLayerMask);

        foreach (var col in colliders)
        {
            SuperCollisionType type = col.GetComponent<SuperCollisionType>();

            Vector3 closestPoint = SuperCollider.ClosestPointOnSurface(col, center, radius);

            RaycastHit hit;

            col.Raycast(new Ray(origin, closestPoint - origin), out hit, Mathf.Infinity);

            if (type != null && Vector3.Angle(hit.normal, controller.up) < type.StandAngle)
            {
                continue;
            }

            if (Vector3.Angle(-hit.normal, lookDirection) < 60.0f)
            {
                struckObject = col.gameObject;
                return true;
            }
        }

        return false;
    }

    private bool FootStrike(out GameObject struckObject)
    {
        struckObject = null;

        float radius = controller.radius;

        Vector3 o = controller.OffsetPosition(controller.feet.Offset);

        Collider[] colliders = Physics.OverlapSphere(o, radius, EnemyLayerMask);

        foreach (var col in colliders)
        {
            var machine = col.gameObject.GetComponent<GoombaMachine>();

            if (machine != null)
            {
                if (!machine.Alive)
                {
                    continue;
                }
            }

            Vector3 closestPoint = SuperCollider.ClosestPointOnSurface(col, o, radius);

            if (SuperMath.PointAbovePlane(controller.down, o, closestPoint))
            {
                struckObject = col.gameObject;
                return true;
            }
        }

        return false;
    }

    private bool BodySlam()
    {
        float radius = controller.radius * 1.5f;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, EnemyLayerMask);

        foreach (var col in colliders)
        {
            EnemyMachine machine = col.GetComponent<EnemyMachine>();

            if (machine != null && machine is GoombaMachine)
            {
                if (machine.GetStruck(Math3d.ProjectVectorOnPlane(controller.up, machine.transform.position - transform.position).normalized, 7.0f, 15.0f, 0.3f))
                {
                    sound.PlayImpact();
                }
            }
        }

        return false;
    }

    private bool GoldBodySlam()
    {
        float radius = controller.radius * 1.5f;

        Collider[] colliders = Physics.OverlapSphere(transform.position + controller.up * controller.height * 0.5f, radius);

        foreach (var col in colliders)
        {
            EnemyMachine machine = col.GetComponent<EnemyMachine>();

            if (machine != null)
            {
                if (machine.GetStruck(Math3d.ProjectVectorOnPlane(controller.up, machine.transform.position - transform.position).normalized, 7.0f, 15.0f))
                {
                    sound.PlayImpact();

                    machine.MakeGold();
                }
            }

            RollingBallGoldDestroy ball = col.GetComponent<RollingBallGoldDestroy>();

            if (ball)
            {
                ball.BlowUp();
            }
        }

        return false;
    }

    private bool ShouldHaveFallDamage()
    {
        if (jumpPeak == Vector3.zero)
            return false;

        Vector3 linePosition = Math3d.ProjectPointOnLine(Vector3.zero, controller.up, transform.position);
        Vector3 peakPosition = Math3d.ProjectPointOnLine(Vector3.zero, controller.up, jumpPeak);

        return Vector3.Distance(linePosition, peakPosition) > jumpDamageHeight;
    }
    private bool FallDamage()
    {
        status.TakeDamage(2);

        sound.PlayTakeDamage();

        if (status.CurrentHealth == 0)
        {
            return true;
        }
        else
        {
            isTakingFallDamage = true;

            ScaleAnimator.GetComponent<Animation>().Play();

            status.EndInvincible();

            return false;
        }
    }

    private void PlayJumpSound()
    {
        if (currentJumpProfile == jumpStandard)
        {
            sound.PlaySingleJump();
        }
        else if (currentJumpProfile == jumpDouble)
        {
            sound.PlayDoubleJump();
        }
        else if (currentJumpProfile == jumpLong)
        {
            sound.PlayLongJump();
        }
        else if (currentJumpProfile == jumpTriple)
        {
            sound.PlayTripleJump();
        }
        else if (currentJumpProfile == jumpKick)
        {
            sound.PlayJumpKick();
        }
        else if (currentJumpProfile == jumpSideFlip)
        {
            sound.PlaySideFlip();
        }
        else if (currentJumpProfile == jumpBackFlip)
        {
            sound.PlayBackFlip();
        }
        else if (currentJumpProfile == jumpWall)
        {
            sound.PlayWallKick();
        }
    }

    private float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    private bool Timer(float startTime, float duration)
    {
        return Time.time > startTime + duration;
    }

    private float timeScale = 1.0f;

    // NOTE! If DebugGUI is active, you will NOT be able to properly pause the game, as the Time.timeScale is constantly
    // set in this method
    void OnGUI()
    {
        if (DebugGui)
        {
            GUI.Box(new Rect(10, 10, 200, 180), "Mario Cont");

            GUI.TextField(new Rect(20, 40, 180, 20), string.Format("State: {0}", currentState));
            timeScale = GUI.HorizontalSlider(new Rect(20, 70, 180, 20), timeScale, 0.0f, 1.0f);
            GUI.TextField(new Rect(20, 100, 180, 20), string.Format("Move Speed: {0}", moveSpeed));
            GUI.TextField(new Rect(20, 130, 180, 20), string.Format("Vertical Speed: {0}", verticalMoveSpeed));
            GUI.TextField(new Rect(20, 160, 180, 20), string.Format("Ground Angle: {0}", GroundAngle().ToString("F2")));

            Time.timeScale = timeScale;
        }
    }
}

