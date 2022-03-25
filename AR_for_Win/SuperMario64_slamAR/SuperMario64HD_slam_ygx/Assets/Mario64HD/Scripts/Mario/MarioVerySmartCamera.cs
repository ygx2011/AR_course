/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class MarioVerySmartCamera : MonoBehaviour {

    public bool AutoTrack = false;

    public float InitialPosition = 0.3f;

    public float YSensitivity = 45.0f;
    public float XSensitivity = 45.0f;

    public float MinDistance = 5.0f;
    public float MaxDistance = 15.0f;

    public float MinHeight = 2.0f;
    public float MaxHeight = 8.0f;

    public float MinAngle = 10.0f;
    public float MaxAngle = 30.0f;

    public float MinDampWeight = 0.75f;
    public float MaxDampWeight = 0.3f;

    public float MinDropDistance = 10.0f;
    public float MaxDropDistance = 25.0f;

    public float MinDropRotation = 0.0f;
    public float MaxDropRotation = 60.0f;

    public float MinMaximumJumpHeight = 5.0f;
    public float MaxMaximumJumpHeight = 8.0f;

    public float MinMaxHeightAdjustment = 5.0f;
    public float MaxMaxHeightAdjustment = 12.0f;

    public Transform target { get; private set; }
    private MarioMachine mario;
    private MarioInput input;
    private SuperCharacterController controller;

    // Lerp value for Distance, Height and Angle between 0 and 1
    private float currentCameraPosition;
    private float currentRotationHorizontal;
    private float currentRotationVertical;

    private float currentRotationVelocity;

    private Vector3 liftoffPoint;
    private Vector3 verticalPosition;
    private Vector3 currentDampVelocity;

    private Vector3 planarPosition;
    private Vector3 planarDampVelocity;
    private bool planarDamping = false;

    private Vector3 cameraShakePosition;
    private Vector3 constantShakePosition;

    private IEnumerator shakeCoroutine;

	void Start () {
        currentCameraPosition = InitialPosition;

        target = GameObject.FindWithTag("Player").transform;

        input = target.GetComponent<MarioInput>();
        mario = target.GetComponent<MarioMachine>();
        controller = target.GetComponent<SuperCharacterController>();

        currentRotationHorizontal = mario.InitialRotation;

        var height = Mathf.Lerp(MinHeight, MaxHeight, currentCameraPosition);

        verticalPosition = Math3d.ProjectPointOnLine(Vector3.zero, controller.up, target.position + height * controller.up);

        StartCoroutine(ConstantShake());
	}

    float currentShakeMagnitude;

    public void ConstantShake(float magnitude)
    {
        currentShakeMagnitude = Mathf.Max(currentShakeMagnitude, magnitude);
    }

    public void Shake(float magnitude, float speed, float duration)
    {
        StopShake();
        shakeCoroutine = CameraShake(magnitude, speed, duration, true);
        StartCoroutine(shakeCoroutine);
    }

    public void StopShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
    }

    IEnumerator CameraShake(float magnitude, float speed, float duration, bool damp)
    {
        cameraShakePosition = Vector3.zero;

        Vector3 targetPosition = cameraShakePosition;

        float shakeStartTime = Time.time;

        while (!SuperMath.Timer(shakeStartTime, duration))
        {
            cameraShakePosition = Vector3.MoveTowards(cameraShakePosition, targetPosition, speed * Time.deltaTime);

            float magModifier = 1.0f - Mathf.InverseLerp(shakeStartTime, shakeStartTime + duration, Time.time);

            if (cameraShakePosition == targetPosition)
            {
                targetPosition = Vector3.zero + Random.insideUnitSphere * magnitude * magModifier;
            }

            yield return 0;
        }

        while (cameraShakePosition != Vector3.zero)
        {
            cameraShakePosition = Vector3.MoveTowards(cameraShakePosition, Vector3.zero, speed * Time.deltaTime);

            yield return 0;
        }
    }

    IEnumerator ConstantShake()
    {
        constantShakePosition = Vector3.zero;

        Vector3 targetPosition = constantShakePosition;

        while (true)
        {
            if (currentShakeMagnitude != 0)
            {
                constantShakePosition = Vector3.MoveTowards(constantShakePosition, targetPosition, 10.0f * Time.deltaTime);

                if (constantShakePosition == targetPosition)
                {
                    targetPosition = Vector3.zero + Random.insideUnitSphere * currentShakeMagnitude;
                }
            }
            else
            {
                constantShakePosition = Vector3.MoveTowards(constantShakePosition, Vector3.zero, 10.0f * Time.deltaTime);
            }

            yield return 0;
        }
    }

	void LateUpdate () {

        var height = Mathf.Lerp(MinHeight, MaxHeight, currentCameraPosition);
        var maxHeight = Mathf.Lerp(MinMaximumJumpHeight, MaxMaximumJumpHeight, currentCameraPosition);
        var maxHeightAdjustment = Mathf.Lerp(MinMaxHeightAdjustment, MaxMaxHeightAdjustment, currentCameraPosition);
        var distance = Mathf.Lerp(MinDistance, MaxDistance, currentCameraPosition);
        var angle = Mathf.Lerp(MinAngle, MaxAngle, currentCameraPosition);
        var weight = Mathf.Lerp(MinDampWeight, MaxDampWeight, currentCameraPosition);

        Vector3 targetPoint = target.position;

        if (mario.StateCompare(MarioMachine.MarioStates.Hang) || mario.StateCompare(MarioMachine.MarioStates.Climb))
        {
            targetPoint = mario.ClimbTarget();

            if (!planarDamping)
            {
                SetPlanarDamping(true);
            }
        }
        else
        {
            if (planarDamping)
            {
                SetPlanarDamping(false);
            }
        }

        if (!mario.Airborn())
        {
            liftoffPoint = targetPoint;

            verticalPosition = Vector3.SmoothDamp(verticalPosition, Math3d.ProjectPointOnLine(Vector3.zero, controller.up, targetPoint + height * controller.up), ref currentDampVelocity, 0.2f);

            currentRotationVertical = Mathf.SmoothDamp(currentRotationVertical, 0, ref currentRotationVelocity, 0.2f);
        }
        else
        { 
            Vector3 groundPosition = Math3d.ProjectPointOnLine(Vector3.zero, controller.up, liftoffPoint);
            Vector3 airPosition = Math3d.ProjectPointOnLine(Vector3.zero, controller.up, targetPoint);

            float jumpHeight = Vector3.Distance(groundPosition, airPosition);

            var dropRotation = Mathf.Lerp(MinDropRotation, MaxDropRotation, Mathf.InverseLerp(MinDropDistance, MaxDropDistance, jumpHeight));

            if (SuperMath.PointAbovePlane(controller.up, liftoffPoint, targetPoint))
            {
                float extraJumpHeight = 0;

                if (jumpHeight > maxHeight)
                {
                    extraJumpHeight = Mathf.Clamp(jumpHeight - maxHeight, 0, maxHeightAdjustment);
                }

                verticalPosition = Vector3.SmoothDamp(verticalPosition, groundPosition + controller.up * ((jumpHeight * weight) + height + extraJumpHeight), ref currentDampVelocity, 0.1f);
            }
            else if (SuperMath.PointAbovePlane(controller.up, liftoffPoint - controller.up * MinDropDistance, targetPoint))
            {
                verticalPosition = Vector3.SmoothDamp(verticalPosition, Math3d.ProjectPointOnLine(Vector3.zero, controller.up, targetPoint + height * controller.up), ref currentDampVelocity, 0.1f);

            }
            else
            {
                currentRotationVertical = Mathf.SmoothDamp(currentRotationVertical, dropRotation, ref currentRotationVelocity, 0.5f);
            }
        }

        Vector3 direction = Math3d.ProjectVectorOnPlane(controller.up, (targetPoint - transform.position).normalized);

        float angleAdjustment = Vector3.Angle(direction, Math3d.ProjectVectorOnPlane(controller.up, transform.forward));

        if (!AutoTrack)
            angleAdjustment = 0;

        angleAdjustment = SuperMath.PointAbovePlane(transform.right, transform.position, targetPoint) ? angleAdjustment : -angleAdjustment;

        currentRotationHorizontal = SuperMath.ClampAngle(currentRotationHorizontal - input.Current.CameraInput.x * XSensitivity + angleAdjustment);

        transform.rotation = Quaternion.AngleAxis(currentRotationHorizontal, controller.up);

        currentCameraPosition = Mathf.Clamp(currentCameraPosition - input.Current.CameraInput.y * YSensitivity, 0, 1);

        if (planarDamping)
        {
            planarPosition = Vector3.SmoothDamp(planarPosition, Math3d.ProjectPointOnPlane(controller.up, Vector3.zero, targetPoint), ref planarDampVelocity, 0.2f);
        }
        else
        {
            planarPosition = Math3d.ProjectPointOnPlane(controller.up, Vector3.zero, targetPoint);
        }

        transform.position = planarPosition + verticalPosition - transform.forward * distance + cameraShakePosition + constantShakePosition;

        transform.rotation = Quaternion.AngleAxis(angle + currentRotationVertical, transform.right) * transform.rotation;

        currentShakeMagnitude = 0;
	}

    void SetPlanarDamping(bool enabled)
    {
        planarDamping = enabled;
        planarDampVelocity = Vector3.zero;
    }
}
