/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class ShaderSwapper : MonoBehaviour
{
    public Shader NewShader;

    private Shader oldShader;
    private Renderer[] renderers;

    void Start()
    {
        renderers = gameObject.GetComponentsInChildren<Renderer>();

        oldShader = renderers[0].material.shader;
    }

    public void SwapNew()
    {
        foreach (var r in renderers)
        {
            r.material.shader = NewShader;
        }
    }

    public void SwapOriginal()
    {
        foreach (var r in renderers)
        {
            r.material.shader = oldShader;
        }
    }

    public void UpdateShaders(Shader originalShader, Shader secondaryShader)
    {
        oldShader = originalShader;
        NewShader = secondaryShader;
    }
}
