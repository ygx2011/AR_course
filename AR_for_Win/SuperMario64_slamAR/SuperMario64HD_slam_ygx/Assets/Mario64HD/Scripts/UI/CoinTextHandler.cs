/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CoinTextHandler : MonoBehaviour {

    public ImageToNumber CoinFirst;
    public ImageToNumber CoinSecond;
    public ImageToNumber CoinThird;

    // Dear god this is an ugly way to do this c'mon Erik you gotta be less lazy
    public void UpdateValue(int value)
    {
        value = Mathf.Clamp(value, 0, 999);

        var stringValue = value.ToString();

        if (stringValue.Length == 1)
        {
            CoinFirst.GetComponent<Image>().enabled = true;

            CoinFirst.SetValue((int)Char.GetNumericValue(stringValue[0]));

            CoinSecond.GetComponent<Image>().enabled = false;
            CoinThird.GetComponent<Image>().enabled = false;
        }
        else if (stringValue.Length == 2)
        {
            CoinFirst.GetComponent<Image>().enabled = true;
            CoinSecond.GetComponent<Image>().enabled = true;

            CoinFirst.SetValue((int)Char.GetNumericValue(stringValue[0]));
            CoinSecond.SetValue((int)Char.GetNumericValue(stringValue[1]));

            CoinThird.GetComponent<Image>().enabled = false;
        }
        else
        {
            CoinFirst.GetComponent<Image>().enabled = true;
            CoinSecond.GetComponent<Image>().enabled = true;
            CoinThird.GetComponent<Image>().enabled = true;

            CoinFirst.SetValue((int)Char.GetNumericValue(stringValue[0]));
            CoinSecond.SetValue((int)Char.GetNumericValue(stringValue[1]));
            CoinThird.SetValue((int)Char.GetNumericValue(stringValue[2]));
        }
    }
}
