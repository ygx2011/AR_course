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

public class InputManager : MonoBehaviour {

    public bool SetGamepadDefaults;

    public custom_inputs CustomInput;

    public GameObject ControlsMenu;

    public float AnalogSensitivity = 10.0f;
    public float AnalogGravity = 20.0f;

    private ButtonKeyBinding currentlyPushed;

    private ButtonKeyBinding[] buttonKeyBindings;

    private Vector2 currentMoveWeight;
    private Vector2 currentCameraWeight;

    public float MoveDeadZone = 0.10f;
    public float CameraDeadZone = 0.20f;

    private string[] JoySticks = { "JoystickUp", "JoystickDown", "JoystickLeft", "JoystickRight", "Joystick_3a", "Joystick_3b", "Joystick_4a", "Joystick_4b", "Joystick_5a", "Joystick_5b", "Joystick_6b", "Joystick_6b", "Joystick_7a", "Joystick_7b", "Joystick_8a", "Joystick_8b" };

    private const int MoveUpIndex = 0;
    private const int MoveDownIndex = 1;
    private const int MoveLeftIndex = 2;
    private const int MoveRightIndex = 3;

    private const int JumpIndex = 4;
    private const int StrikeIndex = 5;
    private const int CrouchIndex = 6;

    private const int CameraUpIndex = 7;
    private const int CameraDownIndex = 8;
    private const int CameraLeftIndex = 9;
    private const int CameraRightIndex = 10;

    private const int PauseIndex = 11;

	void Start () {
        DontDestroyOnLoad(gameObject);

        if (SetGamepadDefaults)
        {
            SetGamePadDefaults();
        }
        else
        {
            SetKeyboardDefaults();
        }

        GetKeyBindings();
	}

    void SetKeyboardDefaults()
    {
        CustomInput.SetKey(KeyCode.W, MoveUpIndex);
        CustomInput.SetKey(KeyCode.S, MoveDownIndex);
        CustomInput.SetKey(KeyCode.A, MoveLeftIndex);
        CustomInput.SetKey(KeyCode.D, MoveRightIndex);

        CustomInput.SetKey(KeyCode.Space, JumpIndex);
        CustomInput.SetKey(KeyCode.Return, StrikeIndex);
        CustomInput.SetKey(KeyCode.LeftControl, CrouchIndex);

        CustomInput.SetKey(KeyCode.UpArrow, CameraUpIndex);
        CustomInput.SetKey(KeyCode.DownArrow, CameraDownIndex);
        CustomInput.SetKey(KeyCode.LeftArrow, CameraLeftIndex);
        CustomInput.SetKey(KeyCode.RightArrow, CameraRightIndex);

        CustomInput.SetKey(KeyCode.Escape, PauseIndex);

        MoveDeadZone = 0.10f;
        CameraDeadZone = 0.25f;

    }

    void SetGamePadDefaults()
    {
        CustomInput.SetAxis("JoystickUp", MoveUpIndex);
        CustomInput.SetAxis("JoystickDown", MoveDownIndex);
        CustomInput.SetAxis("JoystickLeft", MoveLeftIndex);
        CustomInput.SetAxis("JoystickRight", MoveRightIndex);

        CustomInput.SetKey(KeyCode.Joystick1Button0, JumpIndex);
        CustomInput.SetKey(KeyCode.Joystick1Button2, StrikeIndex);
        CustomInput.SetKey(KeyCode.Joystick1Button5, CrouchIndex);

        CustomInput.SetAxis("Joystick_5a", CameraUpIndex);
        CustomInput.SetAxis("Joystick_5b", CameraDownIndex);
        CustomInput.SetAxis("Joystick_4a", CameraLeftIndex);
        CustomInput.SetAxis("Joystick_4b", CameraRightIndex);

        CustomInput.SetKey(KeyCode.Joystick1Button7, PauseIndex);

        MoveDeadZone = 0.10f;
        CameraDeadZone = 0.25f;
    }

    void GetKeyBindings()
    {
        buttonKeyBindings = GameObject.FindObjectsOfType<ButtonKeyBinding>();
    }

    public void SetPreset(int preset)
    {
        switch (preset)
        {
            case 0:
                SetKeyboardDefaults();
                break;
            case 1:
                SetGamePadDefaults();
                break;
            default: 
                break;
        }

        GetKeyBindings();
        UpdateButtonStrings();     
    }

    public void UpdateKeyBindings()
    {
        GetKeyBindings();
        UpdateButtonStrings();
    }

    public void ButtonPushed(ButtonKeyBinding button)
    {
        GetKeyBindings();

        foreach (var b in buttonKeyBindings)
        {
            b.EnableButton();
        }

        currentlyPushed = button;
        currentlyPushed.DisableButton();
    }

    public void UpdateMoveDeadZone(float value)
    {
        MoveDeadZone = Mathf.Clamp(value, 0, 0.95f);
    }

    public void UpdateCameraDeadZone(float value)
    {
        CameraDeadZone = Mathf.Clamp(value, 0, 0.95f);
    }

    public bool Jump()
    {
        return CustomInput.isInput[JumpIndex];
    }

    public bool JumpDown()
    {
        return CustomInput.isInputDown[JumpIndex];
    }

    public bool Strike()
    {
        return CustomInput.isInput[StrikeIndex];
    }

    public bool StrikeDown()
    {
        return CustomInput.isInputDown[StrikeIndex];
    }

    public bool Crouch()
    {
        return CustomInput.isInput[CrouchIndex];
    }

    public bool CrouchDown()
    {
        return CustomInput.isInputDown[CrouchIndex];
    }

    public bool PauseDown()
    {
        return CustomInput.isInputDown[PauseIndex];
    }

    public float MovementMagnitudeDeadZoned()
    {
        float deadZoneMagnitude = Mathf.Clamp(MovementInputDeadZoned().magnitude, 0, 1);

        if (deadZoneMagnitude > 0.97f)
            return 1f;

        float range = 1f - MoveDeadZone;

        return (deadZoneMagnitude - MoveDeadZone) / range;
    }

    public Vector2 MovementInputDeadZoned()
    {
        Vector2 input = MovementInputRaw();

        if (CustomInput.joystickActive[MoveUpIndex])
        {
            if (input.magnitude < MoveDeadZone)
                input = Vector2.zero;
        }

        return input;
    }

    public Vector2 MovementInputRaw()
    {
        Vector2 input = Vector2.zero;

        if (CustomInput.joystickActive[MoveUpIndex] && Input.GetAxis(CustomInput.joystickString[MoveUpIndex]) > 0)
        {
            input.y = Input.GetAxis(CustomInput.joystickString[MoveUpIndex]);
        }
        else if (currentMoveWeight.y > 0)
        {
            input.y = currentMoveWeight.y;
        }

        if (CustomInput.joystickActive[MoveDownIndex] && Input.GetAxis(CustomInput.joystickString[MoveDownIndex]) > 0)
        {
            input.y = -Input.GetAxis(CustomInput.joystickString[MoveDownIndex]);
        }
        else if (currentMoveWeight.y < 0)
        {
            input.y = currentMoveWeight.y;
        }

        if (CustomInput.joystickActive[MoveLeftIndex] && Input.GetAxis(CustomInput.joystickString[MoveLeftIndex]) > 0)
        {
            input.x = -Input.GetAxis(CustomInput.joystickString[MoveLeftIndex]);
        }
        else if (currentMoveWeight.x < 0)
        {
            input.x = currentMoveWeight.x;
        }

        if (CustomInput.joystickActive[MoveRightIndex] && Input.GetAxis(CustomInput.joystickString[MoveRightIndex]) > 0)
        {
            input.x = Input.GetAxis(CustomInput.joystickString[MoveRightIndex]);
        }
        else if (currentMoveWeight.x > 0)
        {
            input.x = currentMoveWeight.x;
        }

        return Vector2.ClampMagnitude(input, 1);
    }

    public Vector2 CameraInputDeadZoned()
    {
        Vector2 input = CameraInputRaw();

        if (CustomInput.joystickActive[CameraUpIndex])
        {
            if (input.magnitude < CameraDeadZone)
                input = Vector2.zero;
        }
        return input;
    }

    public Vector2 CameraInputRaw()
    {
        Vector2 input = Vector2.zero;

        if (CustomInput.joystickActive[CameraUpIndex] && Input.GetAxis(CustomInput.joystickString[CameraUpIndex]) > 0)
        {
            input.y = Input.GetAxis(CustomInput.joystickString[CameraUpIndex]);
        }
        else if (currentCameraWeight.y > 0)
        {
            input.y = currentCameraWeight.y;
        }

        if (CustomInput.joystickActive[CameraDownIndex] && Input.GetAxis(CustomInput.joystickString[CameraDownIndex]) > 0)
        {
            input.y = -Input.GetAxis(CustomInput.joystickString[CameraDownIndex]);
        }
        else if (currentCameraWeight.y < 0)
        {
            input.y = currentCameraWeight.y;
        }

        if (CustomInput.joystickActive[CameraLeftIndex] && Input.GetAxis(CustomInput.joystickString[CameraLeftIndex]) > 0)
        {
            input.x = -Input.GetAxis(CustomInput.joystickString[CameraLeftIndex]);
        }
        else if (currentCameraWeight.x < 0)
        {
            input.x = currentCameraWeight.x;
        }

        if (CustomInput.joystickActive[CameraRightIndex] && Input.GetAxis(CustomInput.joystickString[CameraRightIndex]) > 0)
        {
            input.x = Input.GetAxis(CustomInput.joystickString[CameraRightIndex]);
        }
        else if (currentCameraWeight.x > 0)
        {
            input.x = currentCameraWeight.x;
        }

        return input;
    }

    void Update()
    {
        if (!CustomInput.joystickActive[MoveUpIndex])
        {
            if (CustomInput.isInput[MoveUpIndex])
            {
                currentMoveWeight.y = Mathf.MoveTowards(currentMoveWeight.y, 1.0f, AnalogSensitivity * Time.deltaTime);
            }
            else if (CustomInput.isInput[MoveDownIndex])
            {
                currentMoveWeight.y = Mathf.MoveTowards(currentMoveWeight.y, -1.0f, AnalogSensitivity * Time.deltaTime);
            }
            else
            {
                currentMoveWeight.y = Mathf.MoveTowards(currentMoveWeight.y, 0, AnalogGravity * Time.deltaTime);
            }
        }

        if (!CustomInput.joystickActive[MoveLeftIndex])
        {
            if (CustomInput.isInput[MoveLeftIndex])
            {
                currentMoveWeight.x = Mathf.MoveTowards(currentMoveWeight.x, -1.0f, AnalogSensitivity * Time.deltaTime);
            }
            else if (CustomInput.isInput[MoveRightIndex])
            {
                currentMoveWeight.x = Mathf.MoveTowards(currentMoveWeight.x, 1.0f, AnalogSensitivity * Time.deltaTime);
            }
            else
            {
                currentMoveWeight.x = Mathf.MoveTowards(currentMoveWeight.x, 0, AnalogGravity * Time.deltaTime);
            }
        }

        if (!CustomInput.joystickActive[CameraUpIndex])
        {
            if (CustomInput.isInput[CameraUpIndex])
            {
                currentCameraWeight.y = Mathf.MoveTowards(currentCameraWeight.y, 1.0f, AnalogSensitivity * Time.deltaTime);
            }
            else if (CustomInput.isInput[CameraDownIndex])
            {
                currentCameraWeight.y = Mathf.MoveTowards(currentCameraWeight.y, -1.0f, AnalogSensitivity * Time.deltaTime);
            }
            else
            {
                currentCameraWeight.y = Mathf.MoveTowards(currentCameraWeight.y, 0, AnalogGravity * Time.deltaTime);
            }
        }

        if (!CustomInput.joystickActive[CameraLeftIndex])
        {
            if (CustomInput.isInput[CameraLeftIndex])
            {
                currentCameraWeight.x = Mathf.MoveTowards(currentCameraWeight.x, -1.0f, AnalogSensitivity * Time.deltaTime);
            }
            else if (CustomInput.isInput[CameraRightIndex])
            {
                currentCameraWeight.x = Mathf.MoveTowards(currentCameraWeight.x, 1.0f, AnalogSensitivity * Time.deltaTime);
            }
            else
            {
                currentCameraWeight.x = Mathf.MoveTowards(currentCameraWeight.x, 0, AnalogGravity * Time.deltaTime);
            }
        }
    }

	void OnGUI () {
        if (currentlyPushed)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.Escape)
            {
                CustomInput.SetKey(Event.current.keyCode, currentlyPushed.Index);
                UpdateButtonStrings();
                currentlyPushed = null;
            }

            for (int joyK = 350; joyK < 409; joyK++)
            {
                // check for all joystick buttons
                if (Input.GetKey((KeyCode)joyK) && Event.current.keyCode != KeyCode.Escape)
                {
                    CustomInput.SetKey((KeyCode)joyK, currentlyPushed.Index);
                    UpdateButtonStrings();
                    currentlyPushed = null;
                }
            }

            foreach (string joyStick in JoySticks)
            {
                if (Input.GetAxis(joyStick) > 0.8f && Event.current.keyCode != KeyCode.Escape)
                {
                    CustomInput.SetAxis(joyStick, currentlyPushed.Index);
                    UpdateButtonStrings();
                    currentlyPushed = null;
                }
            }
        }        
	}

    void UpdateButtonStrings()
    {
        string[] inputStrings = CustomInput.GetInputStrings();

        foreach (var b in buttonKeyBindings)
        {
            b.EnableButton(inputStrings[b.Index]);
        }
    }
}
