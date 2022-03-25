/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    public int Value = 1;
    public bool UniformHeight = true;
    public float PlacementHeight = 0.6f;
    public float SpinSpeed = 360.0f;
    public GameObject DeathEffect;
    public AudioClip CoinSound;
    public Transform Art;

    void Awake()
    {
        RaycastHit hit;

        if (UniformHeight && Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity))
        {
            transform.position = hit.point + Vector3.up * PlacementHeight;
        }
    }

    void Update()
    {
        Art.rotation *= Quaternion.AngleAxis(SpinSpeed * Time.deltaTime, transform.up);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Death();
        }
    }

    void Death()
    {
        GameObject.FindObjectOfType<GameMaster>().AddCoin(Value);
        GameObject.FindObjectOfType<MarioStatus>().AddHealth(Value);

        if (DeathEffect)
            Instantiate(DeathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
