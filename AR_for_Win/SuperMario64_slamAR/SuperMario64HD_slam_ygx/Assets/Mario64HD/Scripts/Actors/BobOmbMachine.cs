/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class BobOmbMachine : EnemyMachine {

    public bool IdleBomb;

    public AdditiveTransform WindTransform;
    public Transform FuseTarget;
    public GameObject FuseSmoke;
    public GameObject BobOmbExplosion;
    public GameObject ExplosionEffect;

    public float FuseTimer = 4.0f;
    public float WanderSpeed = 3.0f;
    public float AttackSpeed = 7.0f;
    public float AttackTurnSpeed = 360.0f;
    public float FieldOfView = 70.0f;
    public float SightDistance = 3.0f;
    public float KnockbackGravity = 25.0f;

    private float windRotation;

    private bool fuseLit = false;
    private float fuseLitTime;

    private float struckTime;

    public enum BobOmbStates
    {
        Wander,
        Chase,
        Knockback,
        Explode,
        Idle,
        Fall
    }

    protected override void Start()
    {
        base.Start();

        if (IdleBomb)
            currentState = BobOmbStates.Idle;
        else
            currentState = BobOmbStates.Wander;
    }  

    protected override void LateGlobalSuperUpdate()
    {
        base.LateGlobalSuperUpdate();

        if (fuseLit)
        {
            windRotation = SuperMath.ClampAngle(windRotation + 1000.0f * Time.deltaTime);
        }
        else
        {
            windRotation = SuperMath.ClampAngle(windRotation + 360.0f * Time.deltaTime);
        }

        WindTransform.Rotation = Quaternion.Euler(new Vector3(0, 0, windRotation));

        if (fuseLit)
        {
            if (SuperMath.Timer(fuseLitTime, FuseTimer))
            {
                AnimatedMesh.localScale = Vector3.MoveTowards(AnimatedMesh.localScale, initialScale * 2.0f, 10.0f * Time.deltaTime);
            }

            if (SuperMath.Timer(fuseLitTime, FuseTimer + 0.1f))
            {
                currentState = BobOmbStates.Explode;
                return;
            }
        }
    }

    public override bool GetStruck(Vector3 direction, float force, float lift, float deathTimer = 0)
    {
        if ((BobOmbStates)currentState == BobOmbStates.Knockback)
        {
            return false;
        }

        moveDirection = direction.normalized * force + controller.up * lift;

        struckTime = Time.time;

        currentState = BobOmbStates.Knockback;

        return true;
    }

    public override void KillEnemy()
    {
        DestroyBobOmb();
    }

    public override bool Explosion()
    {
        DestroyBobOmb();

        return true;
    }

    private void DestroyBobOmb()
    {
        Alive = false;

        AnimatedMesh.gameObject.SetActive(false);

        if (!isGold)
        {
            Instantiate(BobOmbExplosion, transform.position, Quaternion.identity);
            Instantiate(ExplosionEffect, transform.position, Quaternion.identity);

            if (canDropObjectOnDeath)
                Instantiate(ObjectDroppedOnDeath, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(GoldParticleDeath, transform.position, Quaternion.identity);
            Instantiate(ObjectDroppedOnDeath, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);

        if (server != null)
            server.PatronDeath();
    }

    void Idle_EnterState()
    {
        anim.Play("idle");

        controller.EnableClamping();
        controller.EnableSlopeLimit();
    }

    void Idle_SuperUpdate()
    {
        if (!IsGrounded(0.5f, true))
        {
            currentState = BobOmbStates.Fall;
            return;
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
            if (fuseLit)
            {
                currentState = BobOmbStates.Chase;
                return;
            }
            else
            {
                currentState = BobOmbStates.Wander;
                return;
            }
        }
    }

    void Fall_ExitState()
    {
        moveDirection = Math3d.ProjectVectorOnPlane(controller.up, moveDirection);
    }

    void Wander_EnterState()
    {
        controller.EnableClamping();
        controller.EnableSlopeLimit();

        anim.CrossFade("walk", 0.25f);
    }

    void Wander_SuperUpdate()
    {
        if (!IsGrounded(0.5f, true))
        {
            currentState = BobOmbStates.Fall;
            return;
        }

        Vector3 direction = target.position - transform.position;

        direction = Math3d.ProjectVectorOnPlane(controller.up, direction);

        float distance = Vector3.Distance(target.position, transform.position);

        if (Vector3.Angle(direction, lookDirection) < FieldOfView && distance < SightDistance)
        {
            currentState = BobOmbStates.Chase;
            return;
        }

        moveSpeed = Mathf.MoveTowards(moveSpeed, WanderSpeed, 3.0f * Time.deltaTime);

        lookDirection = Quaternion.AngleAxis(30.0f * Time.deltaTime, controller.up) * lookDirection;

        moveDirection = moveSpeed * lookDirection;
    }

    Vector3 initialScale;

    void Chase_EnterState()
    {
        if (!fuseLit)
        {
            fuseLit = true;
            fuseLitTime = Time.time;

            var ob = (GameObject)Instantiate(FuseSmoke, FuseTarget.position, Quaternion.identity);

            ob.transform.parent = FuseTarget;

            initialScale = AnimatedMesh.localScale;

            GetComponent<AudioSource>().Play();

            anim["walk"].speed = 2.0f;
        }
    }

    void Chase_SuperUpdate()
    {
        if (!IsGrounded(0.5f, true))
        {
            currentState = BobOmbStates.Fall;
            return;
        }

        Vector3 direction = target.position - transform.position;

        direction = Math3d.ProjectVectorOnPlane(controller.up, direction);

        lookDirection = Vector3.RotateTowards(lookDirection, direction, AttackTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0);

        moveSpeed = Mathf.MoveTowards(moveSpeed, AttackSpeed, 12.0f * Time.deltaTime);

        moveDirection = lookDirection * moveSpeed;
    }

    void Explode_EnterState()
    {
        DestroyBobOmb();
    }

    void Explode_SuperUpdate()
    {

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

        if (SuperMath.Timer(struckTime, 0.2f) && controller.collisionData.Count > 0)
        {
            currentState = BobOmbStates.Explode;
            return;
        }
    }
}
