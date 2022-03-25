/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class MarioInput : MonoBehaviour
{
    public bool MouseCamera;

    [SerializeField]
    Transform marioCamera;

    public bool DebugGui;

    public MarioInputSet Current { get; private set; }

    public InputManager input;

    void Start()
    {
        if (marioCamera == null)
        {
            Debug.LogError("[MarioInput]: Main camera not found");
        }
    }

    void Update()
    {
        Vector3 analogMoveInput = input.MovementInputDeadZoned();

        Vector3 moveInput = Vector3.zero;

        if (analogMoveInput.x != 0)
        {
            moveInput += marioCamera.right * analogMoveInput.x;
        }

        if (analogMoveInput.y != 0)
        {
            moveInput += marioCamera.forward * analogMoveInput.y;
        }

        moveInput = Math3d.ProjectVectorOnPlane(transform.up, moveInput).normalized;

        float moveMagnitude = input.MovementMagnitudeDeadZoned();
        Vector2 cameraInput = input.CameraInputDeadZoned();


        bool jump = input.Jump();
        bool jumpDown = input.JumpDown();
        bool strike = input.Strike();
        bool strikeDown = input.StrikeDown();
        bool trigger = input.Crouch();
        bool triggerDown = input.CrouchDown();

        Current = new MarioInputSet()
        {
            MoveInput = moveInput,
            MoveMagnitude = moveMagnitude,
            CameraInput = cameraInput,
            Jump = jump,
            JumpDown = jumpDown,
            Strike = strike,
            StrikeDown = strikeDown,
            Trigger = trigger,
            TriggerDown = triggerDown
        };
    }

    void OnGUI()
    {
        if (DebugGui)
        {
            GUI.BeginGroup(new Rect(220, 10, 160, 180));

            GUI.Box(new Rect(0, 0, 150, 180), "Mario Input");
            GUI.TextField(new Rect(10, 30, 130, 20), string.Format("Move Input X: {0}", Input.GetAxis("Horizontal").ToString("F3")));
            GUI.TextField(new Rect(10, 60, 130, 20), string.Format("Move Input Y: {0}", Input.GetAxis("Vertical").ToString("F3")));
            GUI.TextField(new Rect(10, 90, 130, 20), string.Format("Move Magn: {0}", Current.MoveMagnitude.ToString("F3")));
            GUI.TextField(new Rect(10, 120, 130, 20), string.Format("Cam Input X: {0}", Input.GetAxis("AxisTwoHorizontal").ToString("F3")));
            GUI.TextField(new Rect(10, 150, 130, 20), string.Format("Cam Input Y: {0}", Input.GetAxis("AxisTwoVertical").ToString("F3")));
            GUI.EndGroup();
        }
    }
}

public struct MarioInputSet
{
    public Vector3 MoveInput;
    public Vector2 CameraInput;
    public float MoveMagnitude;
    public bool Jump;
    public bool JumpDown;
    public bool Strike;
    public bool StrikeDown;
    public bool Trigger;
    public bool TriggerDown;
}

