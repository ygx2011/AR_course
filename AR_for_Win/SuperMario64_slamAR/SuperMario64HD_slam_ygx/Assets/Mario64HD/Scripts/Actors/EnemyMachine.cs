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

[RequireComponent(typeof(SuperCharacterController))]
public abstract class EnemyMachine : SuperStateMachine {

    [HideInInspector]
    public Transform target;

    [HideInInspector]
    public EnemySpawner server;

    [HideInInspector]
    public bool canDropObjectOnDeath = true;

    public GameObject ObjectDroppedOnDeath;

    public bool DebugGui;
    public Transform AnimatedMesh;

    public bool RandomDirection = true;

    public MaterialSwapper goldMaterialSwapper;
    public GameObject GoldParticleDeath;
    public GameObject GoldStruckParticle;

    protected bool isGold;

    public bool Alive { get; protected set; }

    protected SuperCharacterController controller;
    protected Animation anim;

    protected float moveSpeed;

    protected Vector3 lookDirection;
    protected Vector3 moveDirection;

    protected LayerMask playerLayer;

    protected virtual void Start()
    {
        Alive = true;

        controller = gameObject.GetComponent<SuperCharacterController>();

        lookDirection = transform.forward;

        if (RandomDirection)
            lookDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), transform.up) * lookDirection;

        anim = AnimatedMesh.GetComponent<Animation>();

        playerLayer = 1 << LayerMask.NameToLayer("Player");

        if (!target)
            target = GameObject.FindWithTag("Player").transform;
	}

    protected override void LateGlobalSuperUpdate()
    {
        transform.position += moveDirection * Time.deltaTime;

        if (DebugGui)
        {
            DebugDraw.DrawVector(transform.position, lookDirection, 2.0f, 1.0f, Color.red, 0, true);
            DebugDraw.DrawVector(transform.position, moveDirection, 1.0f, 1.0f, Color.blue, 0, true);
        }

        AnimatedMesh.rotation = Quaternion.LookRotation(lookDirection, controller.up);
    }

    protected bool IsGrounded(float distance, bool currentlyGrounded)
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

    public void MakeGold()
    {
        isGold = true;

        goldMaterialSwapper.SwapNew();

        Transform goldEffect = ((GameObject)Instantiate(GoldStruckParticle, transform.position + controller.up * 0.4f, Quaternion.identity)).transform;
        goldEffect.parent = transform;
    }

    public virtual bool GetStruck(Vector3 direction, float force, float lift, float deathTimer = 0) { return false; }

    public virtual bool Explosion() { return false; }

    public virtual void KillEnemy() { }
}
