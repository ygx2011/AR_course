/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisc : MonoBehaviour {

    public Image Disc;
    public Image DiscGloss;
    public Image DiscOverlay;
    public Color[] DiscColors;
    public float OffScreenMoveTime = 0.5f;
    public float DiscDisplayTime = 1.0f;

    private float initialPosition;
    private float offScreenTarget;

    private RectTransform rect;

	void Start () {
        Disc.color = DiscColors[0];

        if (DiscColors.Length != 4)
        {
            Debug.LogError("Disc Colors array is not of length 4, and is required to be");
        }

        UpdateDisc(8);

        rect = GetComponent<RectTransform>();

        initialPosition = rect.anchoredPosition.y;

        offScreenTarget = -initialPosition * 2;
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, offScreenTarget);

        gameObject.SetActive(false);
	}
	
	public void UpdateDisc(int currentHealth)
    {
        DiscOverlay.fillAmount = (8.0f - currentHealth) / 8.0f;

        int colorIndex = (int)Mathf.Clamp(Mathf.Floor((8 - currentHealth) / 2), 0, DiscColors.Length - 1);

        Disc.color = DiscGloss.color = DiscColors[colorIndex];
	}

    public void Minimize()
    {
        StartCoroutine(MoveOffScreen());
    }

    public void Maximize()
    {
        StopAllCoroutines();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, initialPosition);

        gameObject.SetActive(true);
    }

    IEnumerator MoveOffScreen()
    {
        yield return new WaitForSeconds(DiscDisplayTime);

        float i = 0;

        while (i < 1.0f)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(initialPosition, offScreenTarget, i));

            i += Time.deltaTime / OffScreenMoveTime;

            yield return 0;
        }

        gameObject.SetActive(false);
    }
}
