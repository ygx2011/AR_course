/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class TransparencyFade : MonoBehaviour {

    private Renderer[] renderers;

	void Start () {
        renderers = gameObject.GetComponentsInChildren<Renderer>();
	}


    public void FadeIn(float time)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(time));
    }

    public void FadeOut(float time)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(time));
    }

	IEnumerator FadeInCoroutine(float time)
    {
        float i = 0;

        float alpha = 0;

        while (i < 1)
        {
            foreach (var r in renderers)
            {
                alpha = Mathf.Lerp(0, 1.0f, i);

                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, alpha);

                i += Time.deltaTime / time;

                yield return 0;
            }
        }

        foreach (var r in renderers)
        {
            r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, 1);
        }
	}

    IEnumerator FadeOutCoroutine(float time)
    {
        float i = 0;

        float alpha = 1;

        while (i < 1)
        {
            foreach (var r in renderers)
            {
                alpha = Mathf.Lerp(0, 1.0f, 1 - i);

                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, alpha);

                i += Time.deltaTime / time;

                yield return 0;
            }
        }

        foreach (var r in renderers)
        {
            r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, 0);
        }
    }
}
