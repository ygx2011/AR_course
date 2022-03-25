/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class StarGraphic : MonoBehaviour {

    public ParticleSystem HoverParticles;

    public float ScaleAmount = 1.2f;
    public float ScaleTime = 0.5f;
    public float RotationSpeed = 360.0f;

    public bool Hovered { get; set; }

    private Vector3 initialScale;
    private float currentRotation;

    private bool animating;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (Hovered || animating)
        {
            HoverParticles.enableEmission = true;

            transform.localScale = Vector3.MoveTowards(transform.localScale, initialScale * ScaleAmount, ScaleAmount / ScaleTime * Time.deltaTime);
            currentRotation = SuperMath.ClampAngle(currentRotation + RotationSpeed * Time.deltaTime);
        }
        else
        {
            HoverParticles.enableEmission = false;

            transform.localScale = Vector3.MoveTowards(transform.localScale, initialScale, ScaleAmount / ScaleTime * Time.deltaTime);
            
            if (currentRotation != 0)
                currentRotation = Mathf.MoveTowards(currentRotation, 360.0f, RotationSpeed * Time.deltaTime);
        }

        transform.rotation = Quaternion.AngleAxis(currentRotation, Vector3.down);
    }

    public void StartLevelAnimation()
    {
        StartCoroutine(SpinAnimation());
    }

    IEnumerator SpinAnimation()
    {
        animating = true;

        float acceleration = 0;

        while (true)
        {
            acceleration += 700.0f * Time.deltaTime;
            currentRotation = SuperMath.ClampAngle(currentRotation + acceleration * Time.deltaTime);
            transform.rotation = Quaternion.AngleAxis(currentRotation, Vector3.down);

            yield return 0;
        }
    }
}
