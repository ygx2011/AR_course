/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class MarioCamera : MonoBehaviour {

    [SerializeField]
    Transform target;

    [SerializeField]
    float scrollSpeed = 20.0f;

    [SerializeField]
    float xSensitivity = 20.0f;

    [SerializeField]
    float ySensitivity = 20.0f;

    [SerializeField]
    float maxDistance = 10.0f;

    [SerializeField]
    float minDistance = 1.0f;

    [SerializeField]
    float height = 1.0f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private float currentDistance;

	// Use this for initialization
	void Start () {
        currentDistance = (maxDistance + minDistance) / 2;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        currentDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        rotationX += Input.GetAxis("Mouse X") * xSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * ySensitivity;

        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        transform.rotation = xQuaternion * yQuaternion;

        transform.position = target.position - (transform.forward * currentDistance) + (target.up * height);
	}

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
