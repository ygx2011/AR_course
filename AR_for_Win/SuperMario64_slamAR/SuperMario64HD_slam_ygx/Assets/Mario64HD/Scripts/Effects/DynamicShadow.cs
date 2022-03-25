/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Projector))]
public class DynamicShadow : MonoBehaviour {

    public LayerMask Ground;

    public float MinSize = 0.4f;
    public float MinSizeDistance = 3.0f;
    public float MaxSizeDistance = 20.0f;

    private Projector projector;

    private float initialSize;

    [HideInInspector]
    public float Scale = 1.0f;

    void Start()
    {
        projector = GetComponent<Projector>();

        initialSize = projector.orthographicSize;
    }

	void LateUpdate () {

        RaycastHit hit;

        Physics.SphereCast(transform.position + Vector3.up * 0.02f, initialSize, -Vector3.up, out hit, Mathf.Infinity, Ground);

        var lerpValue = Mathf.InverseLerp(MinSizeDistance, MaxSizeDistance, hit.distance);
        var size = Mathf.Lerp(initialSize * MinSize, initialSize, 1 - lerpValue);

        projector.orthographicSize = size * Scale;
	}
}
