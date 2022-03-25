/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class MaterialColorChanger : MonoBehaviour {

    public Renderer Render;
    public Color NewColor;

    private Color initialColor;

	// Use this for initialization
	void Start () {
        initialColor = Render.material.color;
	}

    public void FlickerColor(float flickerSpeed)
    {

        StopAllCoroutines();
        StartCoroutine(Flicker(flickerSpeed, NewColor));
    }

	public void FlickerColor(float flickerSpeed, Color newColor) {

        StopAllCoroutines();
        StartCoroutine(Flicker(flickerSpeed, newColor));
	}

    IEnumerator Flicker(float flickerSpeed, Color newColor)
    {
        while (true)
        {
            float i = 0;

            while (i < 1)
            {
                Render.material.color = Color.Lerp(initialColor, newColor, i);

                i += 1 / flickerSpeed * Time.deltaTime;

                yield return 0;
            }

            i = 0;

            while (i < 1)
            {
                Render.material.color = Color.Lerp(newColor, initialColor, i);

                i += 1 / flickerSpeed * Time.deltaTime;

                yield return 0;
            }
        }
    }
}
