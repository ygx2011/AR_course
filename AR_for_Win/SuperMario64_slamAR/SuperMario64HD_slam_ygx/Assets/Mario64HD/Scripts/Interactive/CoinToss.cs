/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System;
using System.Collections;

public class CoinToss : MonoBehaviour {

    public enum CoinTossDirection { Up, Down }

    public CoinTossDirection Direction;

    public Transform Art;
    public GameObject DeathParticle;

    public float InitialVelocity = 4.0f;
    public float DeccelerationTime = 0.5f;
    public float SpinSpeed = 90.0f;

    private float currentVelocity;

	// Use this for initialization
	void Start () {
        currentVelocity = InitialVelocity;

        if (Direction == CoinTossDirection.Down)
        {
            Invoke("Death", DeccelerationTime);

            currentVelocity = -InitialVelocity;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Direction == CoinTossDirection.Up)
        {
            currentVelocity = Mathf.MoveTowards(currentVelocity, 0, InitialVelocity / DeccelerationTime * Time.deltaTime);

            if (currentVelocity == 0)
            {
                Death();
            }
        }

        transform.position += Vector3.up * currentVelocity * Time.deltaTime;

        Art.rotation *= Quaternion.AngleAxis(SpinSpeed * Time.deltaTime, transform.up);
	}

    void Death()
    {
        Instantiate(DeathParticle, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
