/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SuperCharacterController))]
public class GoombaMachine : EnemyMachine {

    public GameObject ParticleDeathEffect;

    public AudioClip Footstep;
    public AudioClip FlattenedSound;
    public AudioClip Alerted;

    public float AttackSpeed = 3.0f;
    public float WanderSpeed = 1.0f;
    public float WanderTime = 5.0f;
    public float FieldOfView = 70.0f;
    public float SightDistance = 7.0f;
    public float MaintainSightDistance = 7.0f;
    public float ResumeWanderTime = 2.0f;
    public float KnockbackGravity = 25.0f;
    public float WanderDistance = 8.0f;

    private Vector3 initialPosition;

    public enum GoombaStates
    {
        Wander,
        Alert,
        Attack,
        Damage,
        Knockback,
        Dying,
        Fall
    }

    protected override void Start()
    {
        base.Start();

        initialPosition = transform.position;

        currentState = GoombaStates.Wander;
    }

    private float lastDirectionChangeTime;
    private Vector3 targetDirection;

    public override bool GetStruck(Vector3 direction, float force, float lift, float deathTimer = 0)
    {
        if ((GoombaStates)currentState == GoombaStates.Knockback)
        {
            return false;
        }

        moveDirection = direction.normalized * force + controller.up * lift;

        if (deathTimer != 0)
            Invoke("DestroyGoomba", deathTimer);

        currentState = GoombaStates.Knockback;

        return true;
    }

    public override void KillEnemy()
    {
        if ((GoombaStates)currentState != GoombaStates.Dying)
        {
            currentState = GoombaStates.Dying;
        }
    }

    private void DestroyGoomba()
    {
        if (server != null)
            server.PatronDeath();

        if (!isGold)
        {
            Instantiate(ParticleDeathEffect, transform.position, Quaternion.identity);

            if (canDropObjectOnDeath)
                Instantiate(ObjectDroppedOnDeath, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(GoldParticleDeath, transform.position, Quaternion.identity);
            Instantiate(ObjectDroppedOnDeath, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private bool Attack(float pushbackSpeed)
    {
        bool tookDamage = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position + controller.up * controller.radius, controller.radius, playerLayer);

        foreach (var col in colliders)
        {
            tookDamage = col.GetComponent<MarioMachine>().GroundDamageLight(1, transform.position, pushbackSpeed, false);
        }

        return tookDamage;
    }

    IEnumerator Footsteps()
    {
        float speed = 0.15f;

        float lastStepTime = Time.time - speed;

        while (true)
        {
            if (SuperMath.Timer(lastStepTime, speed))
            {
                GetComponent<AudioSource>().PlayOneShot(Footstep, 0.5f);
                lastStepTime = Time.time;
            }

            yield return 0;
        }
    }

    void Fall_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();
    }

    void Fall_SuperUpdate()
    {
        moveDirection -= controller.up * KnockbackGravity * Time.deltaTime;

        if (IsGrounded(0.15f, false))
        {
            currentState = GoombaStates.Wander;
        }
    }

    void Wander_EnterState()
    {
        controller.EnableClamping();
        controller.EnableSlopeLimit();

        targetDirection = lookDirection;

        anim.CrossFade("walk", 0.25f);
    }

	void Wander_SuperUpdate() 
    {
        if (!IsGrounded(0.5f, true))
        {
            currentState = GoombaStates.Fall;
            return;
        }

        if (Attack(4.0f))
        {
            currentState = GoombaStates.Alert;
            return;
        }

        Vector3 direction = target.position - transform.position;

        direction = Math3d.ProjectVectorOnPlane(controller.up, direction);

        float distance = Vector3.Distance(target.position, transform.position);

        if (Vector3.Angle(direction, lookDirection) < FieldOfView && distance < SightDistance)
        {
            currentState = GoombaStates.Alert;
            return;
        }

        if (Vector3.Distance(transform.position, initialPosition) > WanderDistance)
        {
            targetDirection = Math3d.ProjectVectorOnPlane(controller.up, (initialPosition - transform.position).normalized);
        }

        if (Time.time > lastDirectionChangeTime + WanderTime + Random.Range(0f, 2f))
        {
            targetDirection = Quaternion.AngleAxis(Random.Range(30, 70) * Mathf.Sign(Random.Range(-1, 1)), controller.up) * targetDirection;
            lastDirectionChangeTime = Time.time;
        }

        float accleration = moveSpeed > WanderSpeed ? 10.0f : 1.0f;

        moveSpeed = Mathf.MoveTowards(moveSpeed, WanderSpeed, accleration * Time.deltaTime);

        lookDirection = Vector3.RotateTowards(lookDirection, targetDirection, 90 * Mathf.Deg2Rad * Time.deltaTime, 0);

        moveDirection = lookDirection * moveSpeed;
    }

    void Alert_EnterState()
    {
        moveDirection = Vector3.zero;

        GetComponent<AudioSource>().PlayOneShot(Alerted);

        anim.Play("alert");
    }

    void Alert_SuperUpdate()
    {
        if (!IsGrounded(0.5f, true))
        {
            currentState = GoombaStates.Fall;
            return;
        }

        Attack(4.0f);

        Vector3 direction = target.position - transform.position;

        direction = Math3d.ProjectVectorOnPlane(controller.up, direction);

        lookDirection = Vector3.RotateTowards(lookDirection, targetDirection, 180 * Mathf.Deg2Rad * Time.deltaTime, 0);

        if (Time.time > anim["alert"].length + timeEnteredState)
        {
            currentState = GoombaStates.Attack;
        }
    }

    float lastSeenTime;

    void Attack_EnterState()
    {
        StartCoroutine(Footsteps());

        lastSeenTime = Time.time;

        anim.CrossFade("run", 0.1f);
    }

    void Attack_SuperUpdate()
    {
        if (!IsGrounded(0.5f, true))
        {
            currentState = GoombaStates.Fall;
            return;
        }

        if (Attack(8.0f))
        {
            currentState = GoombaStates.Alert;
            return;
        }

        Vector3 direction = target.position - transform.position;

        direction = Math3d.ProjectVectorOnPlane(controller.up, direction);

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance > MaintainSightDistance)
        {
            currentState = GoombaStates.Wander;
            return;
        }

        if (Vector3.Angle(direction, lookDirection) < FieldOfView)
        {
            lastSeenTime = Time.time;
        }

        if (SuperMath.Timer(lastSeenTime, ResumeWanderTime))
        {
            currentState = GoombaStates.Wander;
            return;
        }

        lookDirection = Vector3.RotateTowards(lookDirection, direction, 70 * Mathf.Deg2Rad * Time.deltaTime, 0);

        moveSpeed = Mathf.MoveTowards(moveSpeed, AttackSpeed, 5.0f * Time.deltaTime);

        moveDirection = lookDirection * moveSpeed;
    }

    void Attack_ExitState()
    {
        StopAllCoroutines();
    }

    float currentScale;

    void Dying_EnterState()
    {
        Alive = false;
        moveDirection = Vector3.zero;

        currentScale = AnimatedMesh.localScale.y;

        foreach (AnimationState a in anim)
        {
            if (a.enabled)
            {
                a.speed = 0;
                break;
            }
        }

        GetComponent<AudioSource>().PlayOneShot(FlattenedSound);
    }

    void Dying_SuperUpdate()
    {
        currentScale = Mathf.MoveTowards(currentScale, 0.2f, 6.0f * Time.deltaTime);

        AnimatedMesh.localScale = new Vector3(AnimatedMesh.localScale.x, currentScale, AnimatedMesh.localScale.z);

        if (Time.time > timeEnteredState + 0.6f)
        {
            DestroyGoomba();
        }
    }

    void Knockback_EnterState()
    {
        controller.DisableClamping();
        controller.DisableSlopeLimit();

        foreach (AnimationState a in anim)
        {
            if (a.enabled)
            {
                a.speed = 0;
                break;
            }
        }
    }

    void Knockback_SuperUpdate()
    {
        moveDirection -= controller.up * KnockbackGravity * Time.deltaTime;

        if (Vector3.Angle(moveDirection, controller.up) > 90 && controller.collisionData.Count > 0)
        {
            DestroyGoomba();
        }
    }
}
