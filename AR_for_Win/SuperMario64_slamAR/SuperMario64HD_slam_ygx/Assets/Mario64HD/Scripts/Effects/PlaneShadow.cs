/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class PlaneShadow : MonoBehaviour {

    public LayerMask Ground;
    public float Tolerance = 0.01f;

    public bool Static = true;

    private Transform target;

    private Vector3 initialOffset;
    private Quaternion initialRotation;

	// Use this for initialization
	void Start () {

        initialRotation = transform.rotation;

        if (!Static)
        {
            target = transform.parent;

            transform.parent = null;

            initialOffset = transform.position - target.position;
        }

        ClampToGround();
	}
	
	// Update is called once per frame
	void Update () {
        if (!Static)
        {
            if (target == null)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = target.position + initialOffset;

                ClampToGround();
            }
        }
	}

    void ClampToGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, Ground))
        {
            transform.position = hit.point += Vector3.up * Tolerance;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * initialRotation;
        }
    }
}
