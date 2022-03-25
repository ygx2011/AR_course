/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class BobOmbExplosion : MonoBehaviour {

    public float Duration = 0.5f;
    public float Radius = 3.0f;
    public float Delay = 0.05f;
    public float CameraShakeDistance = 10.0f;

    private float startTime;

	void Start () {
        startTime = Time.time;
        
        Invoke("Death", Duration);

        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (distance < CameraShakeDistance)
        {
            float magnitude = Mathf.Lerp(0.4f, 0, Mathf.InverseLerp(5.0f, CameraShakeDistance, distance));

            Camera.main.GetComponent<MarioVerySmartCamera>().Shake(magnitude, 20.0f, 0.5f);
        }
	}
	
	void Update () {

        if (SuperMath.Timer(startTime, Delay))
        {
            // Haha I still can't get over "PlayerMask"
            Collider[] cols = Physics.OverlapSphere(transform.position, Radius);

            foreach (var col in cols)
            {
                if (col.gameObject.tag == "Player")
                {
                    col.gameObject.GetComponent<MarioMachine>().HeavyDamage(2, transform.position);
                }

                var trigger = col.gameObject.GetComponent<TriggerableObject>();

                if (trigger)
                {
                    trigger.Explosion();
                }

                var machine = col.gameObject.GetComponent<EnemyMachine>();

                if (machine)
                {
                    machine.Explosion();
                }
            }
        }
	}

    void Death()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
