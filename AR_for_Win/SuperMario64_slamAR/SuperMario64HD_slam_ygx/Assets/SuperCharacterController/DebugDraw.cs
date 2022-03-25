/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public static class DebugDraw {

    public static void DrawMarker(Vector3 position, float size, Color color, float duration, bool depthTest = true)
    {
        Vector3 line1PosA = position + Vector3.up * size * 0.5f;
        Vector3 line1PosB = position - Vector3.up * size * 0.5f;

        Vector3 line2PosA = position + Vector3.right * size * 0.5f;
        Vector3 line2PosB = position - Vector3.right * size * 0.5f;

        Vector3 line3PosA = position + Vector3.forward * size * 0.5f;
        Vector3 line3PosB = position - Vector3.forward * size * 0.5f;

        Debug.DrawLine(line1PosA, line1PosB, color, duration, depthTest);
        Debug.DrawLine(line2PosA, line2PosB, color, duration, depthTest);
        Debug.DrawLine(line3PosA, line3PosB, color, duration, depthTest);
    }

    public static void DrawPlane(Vector3 position, Vector3 normal, float size, Color color, float duration, bool depthTest = true) 
    {
        Vector3 v3;
 
        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;;
 
        Vector3 corner0 = position + v3 * size;
        Vector3 corner2 = position - v3 * size;
 
        Quaternion q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        Vector3 corner1 = position + v3 * size;
        Vector3 corner3 = position - v3 * size;

        Debug.DrawLine(corner0, corner2, color, duration, depthTest);
        Debug.DrawLine(corner1, corner3, color, duration, depthTest);
        Debug.DrawLine(corner0, corner1, color, duration, depthTest);
        Debug.DrawLine(corner1, corner2, color, duration, depthTest);
        Debug.DrawLine(corner2, corner3, color, duration, depthTest);
        Debug.DrawLine(corner3, corner0, color, duration, depthTest);
        Debug.DrawRay(position, normal * size, color, duration, depthTest);
    }

    public static void DrawVector(Vector3 position, Vector3 direction, float raySize, float markerSize, Color color, float duration, bool depthTest = true)
    {
        Debug.DrawRay(position, direction * raySize, color, 0, false);
        DebugDraw.DrawMarker(position + direction * raySize, markerSize, color, 0, false);
    }
}
