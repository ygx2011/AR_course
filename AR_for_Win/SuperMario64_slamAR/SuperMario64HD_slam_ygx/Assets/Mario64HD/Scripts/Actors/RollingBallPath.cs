/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class RollingBallPath : SimpleStateMachine {

    enum RollingBallStates { Spawned, Pathing, Bouncing, WallBouncing }

    public string PathName;

    public bool WallBouncer = false;

    public Transform Ball;
    public LayerMask Ground;
    public GameObject DeathParticle;
    public GameObject MoveSmokeEffect;
    public GameObject BounceSmokeEffect;

    public float BounceShakeDistance = 15.0f;

    public float PathSpeed = 8.0f;
    public float BounceSpeed = 12.0f;
    public float BounceHeight = 2.0f;
    public float Gravity = 20.0f;
    public float RotationSpeed = 180.0f;
    public float AngleAdjustment = 30.0f;
    public float Elasticity = 0.7f;

    private Vector3 lastPosition;
    private Vector3 moveDirection;
    private float radius;
    private GameObject smokeEffect;
    private MarioVerySmartCamera smartCamera;
    private RaycastHit currentGround;

	// Use this for initialization
	void Start () {
        radius = GetComponent<SphereCollider>().radius * transform.localScale.x;

        smartCamera = Camera.main.GetComponent<MarioVerySmartCamera>();

        currentState = RollingBallStates.Spawned;
	}

    bool TouchingGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, radius, Ground);
    }

    bool TouchingGround(out RaycastHit hit)
    {
        return Physics.Raycast(transform.position, -Vector3.up, out hit, radius, Ground);
    }

    bool TouchingWall()
    {
        return Physics.Raycast(transform.position, transform.forward, GetComponent<SphereCollider>().radius * transform.localScale.x, Ground);

        //return Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius * transform.localScale.x * 0.7f, Ground).Length > 0;
    }

    void ClampToGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, Ground))
        {
            transform.position = hit.point + Vector3.up * GetComponent<SphereCollider>().radius * transform.localScale.x;
        }
    }

    Vector3 spawnTarget;

    void Spawned_EnterState()
    {
        ClampToGround();

        spawnTarget = iTweenPath.GetPath(PathName)[0];

        Vector3 direction = Math3d.ProjectVectorOnPlane(Vector3.up, spawnTarget - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        moveDirection = direction * 4.0f;
    }

    void Spawned_Update()
    {
        Ball.Rotate(new Vector3(RotationSpeed * Time.deltaTime, 0, 0), Space.Self);

        transform.position += moveDirection * Time.deltaTime;

        RaycastHit hit;

        if (TouchingGround(out hit))
        {
            moveDirection.y = 0;

            if (hit.distance < radius)
            {
                ClampToGround();
            }
        }
        else
        {
            moveDirection.y -= Gravity * Time.deltaTime;
        }

        if (!SuperMath.PointAbovePlane(transform.forward, transform.position, spawnTarget))
        {
            currentState = RollingBallStates.Pathing;
        }
    }

    void Pathing_EnterState()
    {
        GetComponent<ProximityCameraShaker>().enabled = true;

        GetComponent<AudioSource>().Play();

        smokeEffect = Instantiate(MoveSmokeEffect, transform.position, Quaternion.identity) as GameObject;

        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(PathName), "speed", PathSpeed, "easetype", iTween.EaseType.linear, "looktime", 0.2f, "oncomplete", "PathComplete", "movetopath", false));
    }

    void Pathing_Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, Ground))
        {
            smokeEffect.transform.position = hit.point;
        }

        Ball.Rotate(new Vector3(RotationSpeed * Time.deltaTime, 0, 0), Space.Self);
    }

    void Pathing_LateUpdate()
    {
        ClampToGround();

        Vector3 direction = (transform.position - lastPosition).normalized;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);
        }

        lastPosition = transform.position;   
    }

    void Pathing_ExitState()
    {
        GetComponent<AudioSource>().Stop();

        GetComponent<ProximityCameraShaker>().enabled = false;
    }

    Quaternion targetRotation;

    int hookBounce = 0;
    int currentBounce = 0;

    void Bouncing_Update()
    {
        Ball.Rotate(new Vector3(RotationSpeed * Time.deltaTime, 0, 0), Space.Self);

        transform.position += moveDirection * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * 0.2f * Time.deltaTime);

        RaycastHit hit;

        if (TouchingGround(out hit) && moveDirection.y < 0)
        {
            if (Mathf.Abs(moveDirection.y) > 2.0f)
            {
                if (Vector3.Angle(Vector3.up, hit.normal) < 10)
                {
                    moveDirection.y = Mathf.Abs(moveDirection.y) * Elasticity;
                }
                else
                {
                    moveDirection.y = 0;
                    Bounce(BounceHeight);
                }

                BounceShake();

                Instantiate(BounceSmokeEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                transform.position = hit.point + Vector3.up * GetComponent<SphereCollider>().radius * transform.localScale.x;

                moveDirection.y = 0;
            }

            currentBounce++;

            if (hookBounce != 0 && currentBounce == hookBounce)
            {
                targetRotation = Quaternion.AngleAxis(30.0f, Vector3.up) * transform.rotation;

                float vertical = moveDirection.y;

                moveDirection = Quaternion.AngleAxis(30.0f, Vector3.up) * transform.forward * BounceSpeed;

                moveDirection.y = vertical;
            }

        }
        else if (TouchingWall())
        {
            Instantiate(DeathParticle, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
        else
        {
            moveDirection -= Vector3.up * Gravity * Time.deltaTime;
        }
    }

    // I admit, this is far and away the worst possible way to program this
    // Shhh...no one has to ever know

    void WallBouncing_EnterState()
    {
        ClampToGround();

        moveDirection = transform.forward * 12.0f;

        Invoke("FirstBounce", 0.39f);
    }

    void WallBouncing_Update()
    {
        moveDirection.y -= Gravity * Time.deltaTime;

        transform.position += moveDirection * Time.deltaTime;
    }

    void FirstBounce()
    {
        float vertical = moveDirection.y;

        moveDirection = transform.forward * -12.0f;
        moveDirection.y = vertical;

        Instantiate(BounceSmokeEffect, transform.position + transform.forward * GetComponent<SphereCollider>().radius * transform.localScale.x, Quaternion.LookRotation(-transform.forward));

        BounceShake();

        Invoke("SecondBounce", 0.15f);
    }

    void SecondBounce()
    {
        targetRotation = Quaternion.AngleAxis(-45.0f, Vector3.up) * transform.rotation;

        float vertical = moveDirection.y;

        moveDirection = Quaternion.AngleAxis(-45.0f, Vector3.up) * transform.forward * BounceSpeed;

        moveDirection.y = vertical;

        hookBounce = 2;

        Instantiate(BounceSmokeEffect, transform.position - transform.forward * GetComponent<SphereCollider>().radius * transform.localScale.x, Quaternion.LookRotation(transform.forward));

        BounceShake();

        currentState = RollingBallStates.Bouncing;
    }

    void PathComplete()
    {
        smokeEffect.GetComponent<ParticleMaster>().Emit(false);

        if (!WallBouncer)
        {
            ClampToGround();

            Bounce(BounceHeight * 0.5f);

            targetRotation = Quaternion.AngleAxis(AngleAdjustment, Vector3.up) * transform.rotation;

            moveDirection = Quaternion.AngleAxis(AngleAdjustment, Vector3.up) * transform.forward * BounceSpeed;

            currentState = RollingBallStates.Bouncing;
        }
        else
        {
            currentState = RollingBallStates.WallBouncing;
        }
    }

    void DestroySmokeEffect()
    {
        if (smokeEffect != null)
            smokeEffect.GetComponent<ParticleMaster>().Emit(false);
    }

    void Bounce(float height)
    {
        moveDirection += SuperMath.CalculateJumpSpeed(height, Gravity) * Vector3.up;
    }

    void BounceShake()
    {
        float distance = Vector3.Distance(smartCamera.transform.position, transform.position);

        if (distance < BounceShakeDistance)
        {
            float magnitude = Mathf.Lerp(0.2f, 0, Mathf.InverseLerp(5.0f, BounceShakeDistance, distance));

            smartCamera.Shake(magnitude, 10.0f, 0.2f);
        }
    }
}
