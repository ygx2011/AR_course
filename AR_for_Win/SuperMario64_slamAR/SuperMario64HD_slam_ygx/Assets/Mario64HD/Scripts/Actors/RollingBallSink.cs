/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class RollingBallSink : MonoBehaviour {

    public LayerMask Ground;
    public Transform Ball;

    public float RotationSpeed = 60.0f;
    public float Acceleration = 8.0f;
    public float Decceleration = 8.5f;

    public bool Forwards = true;

    private float currentSpeed;
    private float radius;

    void Start()
    {
        currentSpeed = Acceleration;

        radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }

	void Update () {

        Vector3 right = Forwards ? transform.right : -transform.right;

        RaycastHit hit;

        Physics.SphereCast(transform.position + 1.0f * Vector3.up, radius, -Vector3.up, out hit, Mathf.Infinity, Ground);

        transform.position -= (hit.distance - 1.0f) * Vector3.up;

        Vector3 direction = Vector3.Cross(right, hit.normal);

        transform.position += currentSpeed * direction * Time.deltaTime;

        float angle = Vector3.Angle(direction, Vector3.up);

        if (Vector3.Angle(hit.normal, Vector3.up) < 5)
        {
        } 
        else if (angle < 90)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Decceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, Acceleration, Acceleration * Time.deltaTime);
        }

        if (currentSpeed == 0)
        {
            Forwards = !Forwards;
        }

        int rotationDirection = Forwards ? 1 : -1;

        Ball.Rotate(new Vector3(currentSpeed / Acceleration * RotationSpeed * rotationDirection * Time.deltaTime, 0, 0), Space.Self);

	}
}
