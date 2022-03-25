/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class GateSwitch : TriggerableObject {

    public Gate triggeredGate;

    public float TargetScale = 0.04f;
    public float ScaleSpeed = 6.0f;
    public float ActivateRadius = 1.2f;

    bool pushed = true;

    void FixedUpdate()
    {
        if (!pushed)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.MoveTowards(transform.localScale.z, TargetScale, ScaleSpeed * Time.fixedDeltaTime));
        }
    }

    public override bool StandingOn(Vector3 position)
    {
        if (!pushed)
            return false;

        if (Vector3.Distance(transform.position, Math3d.ProjectPointOnPlane(Vector3.up, transform.position, position)) > ActivateRadius)
            return false;

        triggeredGate.Open();

        GetComponent<AudioSource>().Play();

        pushed = false;

        return true;
    }
}
