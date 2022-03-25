/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public static class SuperMath {

    public static Vector3 ClampAngleOnPlane(Vector3 origin, Vector3 direction, float angle, Vector3 planeNormal)
    {
        float a = Vector3.Angle(origin, direction);

        if (a < angle)
            return direction;

        Vector3 r = Vector3.Cross(planeNormal, origin);

        float s = Vector3.Angle(r, direction);
        float rotationAngle = (s < 90 ? 1 : -1) * angle;
        Quaternion rotation = Quaternion.AngleAxis(rotationAngle, planeNormal);

        return rotation * origin;
    }

    /// <summary>
    /// Returns a value contained within a series of bounds approximating a curve
    /// </summary>
    /// <param name="bounds">Series of bounds, implicity enclosed by -Infinity and +Infinity</param>
    /// <param name="values">Series of values one length longer than the bounds, with each value belonging below each bound</param>
    /// <param name="t">Signifies where along the approximated curve the value should fall</param>
    public static float BoundedInterpolation(float[] bounds, float[] values, float t)
    {
        for (int i = 0; i < bounds.Length; i++)
        {
            if (t < bounds[i])
            {
                return values[i];
            }
        }

        return values[values.Length - 1];
    }

    public static bool PointAbovePlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
    {
        Vector3 direction = point - planePoint;
        return Vector3.Angle(direction, planeNormal) < 90;
    }

    /// <summary>
    /// Checks if the duration since start time has elapsed
    /// </summary>
    public static bool Timer(float startTime, float duration)
    {
        return Time.time > startTime + duration;
    }

    public static float ClampAngle(float angle)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return angle;
    }

    public static float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}
