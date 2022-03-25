/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class RollingBallPathSpawner : MonoBehaviour {

    public Color SpawnColor = Color.grey;
    public string SpawnPath = "RollingBallPath";
    public bool WallBouncer = false;
    public GameObject SpawnedObject;
    public float SpawnInterval = 6.0f;

    private bool canSpawn = true;

	// Use this for initialization
	void Start () {
        Spawn();
	}

    void Spawn()
    {
        if (canSpawn)
        {
            RollingBallPath path = (Instantiate(SpawnedObject, transform.position, Quaternion.identity) as GameObject).GetComponent<RollingBallPath>();

            path.PathName = SpawnPath;
            path.WallBouncer = WallBouncer;
        }

        Invoke("Spawn", SpawnInterval);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = SpawnColor;
        Gizmos.DrawSphere(transform.position, 1.75f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            canSpawn = false;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            canSpawn = true;
        }
    }
}
