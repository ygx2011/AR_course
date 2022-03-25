/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class DestructibleBox : TriggerableObject {

    public GameObject DeathParticle;
    public GameObject Debris;

    public GameObject ObjectSpawnedOnDeath;

    public override bool GroundPound()
    {
        Explode();

        return true;
    }

    public override bool Strike()
    {
        Explode();

        return true;
    }

    private void Explode()
    {
        Instantiate(DeathParticle, transform.position, Quaternion.identity);

        if (Debris)
            Instantiate(Debris, transform.position, Quaternion.AngleAxis(Random.Range(-360, 360), Vector3.up));

        if (ObjectSpawnedOnDeath)
            Instantiate(ObjectSpawnedOnDeath, transform.position, Quaternion.identity);

        DestroyImmediate(gameObject);
    }
}
