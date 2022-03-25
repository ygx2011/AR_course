/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

    public Transform LeftDoor;
    public Transform RightDoor;

    public float OpenSpeed = 55.0f;
    public float OpenAngle = 95.0f;

    private bool opened = false;
    private float rotated;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (opened)
        {
            if (rotated < OpenAngle)
            {
                rotated += OpenSpeed * Time.fixedDeltaTime;

                LeftDoor.rotation *= Quaternion.AngleAxis(OpenSpeed * Time.fixedDeltaTime, Vector3.forward);
                RightDoor.rotation *= Quaternion.AngleAxis(-OpenSpeed * Time.fixedDeltaTime, Vector3.forward);
            }
        }
    }

    void Update()
    {
    }

    public void Open()
    {
        if (opened)
            return;

        GetComponent<AudioSource>().Play();

        opened = true;
    }
}
