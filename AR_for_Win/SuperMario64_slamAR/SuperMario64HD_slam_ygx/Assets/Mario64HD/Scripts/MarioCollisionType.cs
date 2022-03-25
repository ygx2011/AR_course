/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class MarioCollisionType : SuperCollisionType {

    public float SlideAngle = 30.0f;
    public float SlideContinueAngle = 30.0f;

    public bool Destructable;

    public void Trigger()
    {
        gameObject.SendMessage("OnCollisionTrigger", SendMessageOptions.DontRequireReceiver);
    }

    void Start()
    {
        SlideAngle = Mathf.Clamp(SlideAngle, SlideContinueAngle, Mathf.Infinity);
    }
}
