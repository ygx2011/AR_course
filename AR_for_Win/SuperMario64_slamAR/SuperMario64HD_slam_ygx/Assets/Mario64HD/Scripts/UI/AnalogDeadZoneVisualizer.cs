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

public class AnalogDeadZoneVisualizer : MonoBehaviour {

    public enum VisualizerType { Movement, Camera }

    public VisualizerType visualizerType = VisualizerType.Movement;

    public InputManager Inputs;
    public RectTransform Dot;
    public float MaxDotDistance = 50.0f;
    public Slider slider;

    private RectTransform rect;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();

        if (!Inputs)
            Inputs = GameObject.FindObjectOfType<InputManager>();

        if (visualizerType == VisualizerType.Movement)
        {
            slider.value = Inputs.MoveDeadZone;
        }
        else if (visualizerType == VisualizerType.Camera)
        {
            slider.value = Inputs.CameraDeadZone;
        }
	}
	
	// Update is called once per frame
	void Update () {
        rect.localScale = new Vector3(slider.value, slider.value, 1);

        Vector2 input = Vector2.zero;

        if (visualizerType == VisualizerType.Movement)
        {
            input = Inputs.MovementInputDeadZoned();
        }
        else if (visualizerType == VisualizerType.Camera)
        {
            input = Inputs.CameraInputDeadZoned();
        }

        Vector2 moveInput = Vector2.ClampMagnitude(input, 1.0f);

        Dot.localPosition = new Vector3(moveInput.x * MaxDotDistance, moveInput.y * MaxDotDistance, 0);
	}
}
