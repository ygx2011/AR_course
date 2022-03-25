/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class MarioSmartCamera : MonoBehaviour {

    public bool DebugGui;

    [SerializeField]
    Transform target;

    [SerializeField]
    float trackerHeight = 2.0f;

    [SerializeField]
    float cameraHeight = 1.0f;

    [SerializeField]
    float maxDistance = 12.0f;

    [SerializeField]
    float minDistance = 3.0f;

    [SerializeField]
    float xSensitivity = 45.0f;

    [SerializeField]
    float ySensitivity = 45.0f;

    private MarioMachine mario;
    private SuperCharacterController controller;
    private MarioInput input;

    private Vector3 lastGroundedPosition;

    private float currentDistance;
    private float currentRotation;

    private Vector3 trackerPlanar;
    private Vector3 trackerVertical;
    private Vector3 verticalTrackerVelocity;

	// Use this for initialization
	void Start () {
        currentDistance = (maxDistance + minDistance) * 0.5f;

        mario = target.GetComponent<MarioMachine>();
        controller = target.GetComponent<SuperCharacterController>();
        input = target.GetComponent<MarioInput>();
	}

	// Update is called once per frame
	void LateUpdate () {

        currentDistance = Mathf.Clamp(currentDistance + input.Current.CameraInput.y * ySensitivity, minDistance, maxDistance);
        currentRotation -= input.Current.CameraInput.x * xSensitivity;

        float t = Mathf.InverseLerp(minDistance, maxDistance, currentDistance);
        float maxCamDistance = Mathf.Lerp(1.5f, 7, t);

        bool airborn = MarioAirborn();

        Vector3 verticalTrackerTarget;

        if (airborn)
        {
            Vector3 lastGroundAltitude = Math3d.ProjectPointOnLine(target.position, Vector3.up, lastGroundedPosition);
            Vector3 currentGroundAltitude = Math3d.ProjectPointOnLine(target.position, Vector3.up, controller.currentGround.Hit.point);

            bool above = PointAbovePlane(Vector3.up, lastGroundAltitude, currentGroundAltitude);

            if (above)
            {
                lastGroundAltitude = currentGroundAltitude;
            }

            float distance = Vector3.Distance(lastGroundAltitude, target.position);

            if (distance > maxCamDistance)
            {
                verticalTrackerTarget = (distance - maxCamDistance) * Vector3.up;
            }
            else
            {
                verticalTrackerTarget = Vector3.zero;
            }

            verticalTrackerTarget += lastGroundAltitude;
        }
        else
        {
            verticalTrackerTarget = Math3d.ProjectPointOnLine(target.position, Vector3.up, lastGroundedPosition);
            lastGroundedPosition = target.position;
        }

        verticalTrackerTarget = Math3d.ProjectPointOnLine(Vector3.zero, Vector3.up, verticalTrackerTarget);

        trackerVertical = Math3d.ProjectPointOnLine(verticalTrackerTarget, Vector3.up, trackerVertical);
        trackerVertical = Vector3.SmoothDamp(trackerVertical, verticalTrackerTarget, ref verticalTrackerVelocity, 0.1f);

        trackerPlanar = Math3d.ProjectPointOnPlane(Vector3.up, Vector3.zero, target.position);

        Vector3 tracker = trackerVertical + trackerPlanar;

        tracker += Vector3.up * trackerHeight;

        transform.rotation = Quaternion.AngleAxis(currentRotation, Vector3.up);

        Vector3 targetPosition = tracker - transform.forward * currentDistance + Vector3.up * cameraHeight;

        transform.position = targetPosition;
        transform.rotation = Quaternion.LookRotation(tracker - transform.position);

        if (DebugGui)
        {
            DebugDraw.DrawMarker(tracker, 1.0f, Color.cyan, 0, false);
        }
	}

    private bool PointAbovePlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
    {
        Vector3 direction = point - planePoint;
        return Vector3.Angle(direction, planeNormal) < 90;
    }

    private bool MarioAirborn()
    {
        if ((MarioMachine.MarioStates)mario.currentState == MarioMachine.MarioStates.Jump ||
            (MarioMachine.MarioStates)mario.currentState == MarioMachine.MarioStates.Fall ||
            (MarioMachine.MarioStates)mario.currentState == MarioMachine.MarioStates.AirKnockback ||
            (MarioMachine.MarioStates)mario.currentState == MarioMachine.MarioStates.Dive ||
            (MarioMachine.MarioStates)mario.currentState == MarioMachine.MarioStates.GroundPound ||
            (MarioMachine.MarioStates)mario.currentState == MarioMachine.MarioStates.GroundPoundPrepare ||
            (MarioMachine.MarioStates)mario.currentState == MarioMachine.MarioStates.SFlip)
        {
            return true;
        }

        return false;
    }
}
