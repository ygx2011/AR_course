/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class custom_inputs : MonoBehaviour
{
    // custom inputmanager values
    // ---------------------------   
    // is menu on or off
    // bool menuOn = false;

    //logo to show on top of the inputmanager
    public Texture2D inputManagerLogo;

    // Description string array
    public string[] DescriptionString;

    // Keycode arrays
    KeyCode[] inputKey;
    KeyCode[] inputKey2;
    public KeyCode[] default_inputKeys; // if u set this in the inspector ,make sure its the same length as the Description string array !!!
    public KeyCode[] alt_default_inputKeys; // same deal as above ! leave empty if not used !

    // do we use alternative inputbuttons ?
    // bool altInputson = false;
    
    //should we accept mouse axis ?
    public bool mouseAxisOn = false;

    //should we accept mouse buttons ?
    public bool mouseButtonsOn = true;

    //should we accept duplicate inputs ? ?
    public bool allowDuplicates = false;

    // Input bool arrays
    bool[] inputBool ;
    bool[] inputBool2;

    // input string array
    string[] inputString;
    string[] inputString2;

    // Joystick bool arrays
    [HideInInspector]
    public bool[] joystickActive;
    [HideInInspector]
    public bool[] joystickActive2;
    [HideInInspector]
    public string[] joystickString;
    [HideInInspector]
    public string[] joystickString2;
    bool[] tempjoy1;
    bool[] tempjoy2;

    // inputbool arrays which states if we get input on a controll or not
    [HideInInspector]
    public bool[] isInput;
    [HideInInspector]
    public bool[] isInputDown;
    [HideInInspector]
    public bool[] isInputUp;

    // temp values
    string tempkeyPressed;
    // bool tempbool = false;

    // GUI skin
    // public GUISkin OurSkin;

    //when can we click again
//    bool mouseClick0=false;

    // internal value
    float lastInterval;
    
    // non inputmanager values
    // -----------------------

    // analog feel values
    // use these values if you want to simulate a virtual analog axis
    [HideInInspector]
    public float analogFeel_up = 0;
    [HideInInspector]
    public float analogFeel_down = 0;
    [HideInInspector]
    public float analogFeel_left = 0;
    [HideInInspector]
    public float analogFeel_right = 0;
    [HideInInspector]
    public float analogFeel_jump = 0;

    // public float analogFeel_gravity = 0.2f; // how fast do we slow down after we release the button 
    // public float analogFeel_sensitivity = 0.8f; // how fast do we speed up when we start pressing a button

    //to check the length
    int tempLength;

    /*float DescriptionBox_X = 0;
    float InputBox1_X = 0;
    float InputBox2_X = 0;
    float resetbuttonX = 0;*/

    void Awake () 
    {    	
        // inputmanager ---------------
        // if we set default inputkeys then we set the bool true
        //if (alt_default_inputKeys.Length == default_inputKeys.Length) { altInputson = true; }

        // set up the arrays size same as the Description arrays size (set in inspector)
        inputBool = new bool[DescriptionString.Length];
        inputString = new string[DescriptionString.Length];
        inputKey = new KeyCode[DescriptionString.Length];
        joystickActive = new bool[DescriptionString.Length];
        joystickString = new string[DescriptionString.Length];
        inputBool2 = new bool[DescriptionString.Length];
        inputString2 = new string[DescriptionString.Length];
        inputKey2 = new KeyCode[DescriptionString.Length];
        joystickActive2 = new bool[DescriptionString.Length];
        joystickString2 = new string[DescriptionString.Length];
        isInput = new bool[DescriptionString.Length];
        isInputDown = new bool[DescriptionString.Length];
        isInputUp = new bool[DescriptionString.Length];
        tempLength = PlayerPrefs.GetInt("KeyLength");
        tempjoy1 = new bool[DescriptionString.Length];
        tempjoy2 = new bool[DescriptionString.Length];

        SetDefaults();

        //if (!PlayerPrefs.HasKey("KeyCodes") || !PlayerPrefs.HasKey("KeyCodes2"))
        //{
        //    reset2defaults();
        //}
        tempLength = PlayerPrefs.GetInt("KeyLength");
        if (PlayerPrefs.HasKey("KeyCodes") && tempLength == DescriptionString.Length)
        {
            // loadConfig();
        }

        // make the isInput bools
        for (int gg = 0; gg < DescriptionString.Length; gg++)
        {
            isInput[gg] = false;
            isInputDown[gg] = false;
            isInputUp[gg] = false;
            tempjoy1[gg] = true;
            tempjoy2[gg] = false;
        }
        // ********************************
 
	}

	void Update ()
    {
	    /*DescriptionBox_X = ((Screen.width/2) + DescBox_X);
	    InputBox1_X = ((Screen.width/2) + InputBox_X);
	    InputBox2_X = ((Screen.width/2) + AltInputBox_X);
	    resetbuttonX = ((Screen.width/2) + resetbuttonLocX);*/
	    
        // if the menu is off
        //if (!menuOn)
        //{
            // handle the controlls 
           inputSetBools();  
        //   inputhandling();
        //}



        // call the menu with ESCAPE 
        /*if (Input.GetKeyDown("escape")) 
        {
            if (menuOn) 
            {
                // remove all references to timescale if u don't want the game to pause
                Time.timeScale = 1;
                tempbool = false; 
                menuOn = false;
                // save our configuration
                saveInputs();
            }
            else 
            {
                // remove all references to timescale if u don't want the game to pause
                Time.timeScale = 0; 
                menuOn = true; 
            }
        }*/
     
	}

    /*void OnGUI()
    {

         // we take 3 seconds interval 
         if (Time.realtimeSinceStartup>lastInterval+3)
         {
             // we don't wait for key input anymore
             tempbool = false;
         }


        // only do this when menu is up
        if (menuOn == true)
        {
            drawButtons1();
            // if we have alternative inputbuttons on
            if (altInputson) { drawButtons2(); }
        }
    }*/



   void inputSetBools()
    {
        // sets the isInput[] bool to true or false
        for (int inP = 0; inP < DescriptionString.Length; inP++)
        {
            if ((Input.GetKey(inputKey[inP]) || (joystickActive[inP] && (Input.GetAxis(joystickString[inP]) > 0.95f))) || (Input.GetKey(inputKey2[inP]) || (joystickActive2[inP] && (Input.GetAxis(joystickString2[inP]) > 0.95f))))
            {
               isInput[inP] = true;
            }
            else { isInput[inP] = false; }

            // sets the isInputDown[] bool to true or false
            if ((Input.GetKeyDown(inputKey[inP])) || (Input.GetKeyDown(inputKey2[inP]))) 
            {
                isInputDown[inP] = true;
            }
            else { isInputDown[inP] = false;  }

            if ((joystickActive[inP] && (Input.GetAxis(joystickString[inP]) > 0.95f)) || (joystickActive2[inP] && (Input.GetAxis(joystickString2[inP]) > 0.95f)))
            {
                if (!tempjoy1[inP])
                {
                    isInputDown[inP] = false;
                }
                if (tempjoy1[inP])
                {
                    isInputDown[inP] = true;
                    tempjoy1[inP] = false;
                }
            }
            if (!tempjoy1[inP] && (joystickActive[inP] && (Input.GetAxis(joystickString[inP]) < 0.1f)) && (joystickActive2[inP] && (Input.GetAxis(joystickString2[inP]) < 0.1f)))
            {
                isInputDown[inP] = false;
                tempjoy1[inP] = true;
            }
            if (!tempjoy1[inP] && (!joystickActive[inP]) && (joystickActive2[inP] && (Input.GetAxis(joystickString2[inP]) < 0.1f)))
            {
                isInputDown[inP] = false;
                tempjoy1[inP] = true;
            }
            if (!tempjoy1[inP] && (!joystickActive2[inP]) && (joystickActive[inP] && (Input.GetAxis(joystickString[inP]) < 0.1f)))
            {
                isInputDown[inP] = false;
                tempjoy1[inP] = true;
            }
            // sets the isInputUp[] bool to true or false
            if ((Input.GetKeyUp(inputKey[inP])) || (Input.GetKeyUp(inputKey2[inP])))
            {
                isInputUp[inP] = true;
            }
            else { isInputUp[inP] = false; }
            if ((joystickActive[inP] && (Input.GetAxis(joystickString[inP]) > 0.95f)) || (joystickActive2[inP] && (Input.GetAxis(joystickString2[inP]) > 0.95f)))
            {
                if (tempjoy2[inP])
                {
                    isInputUp[inP] = false;
                }
                if (!tempjoy2[inP])
                {
                    isInputUp[inP] = false;
                    tempjoy2[inP] = true;
                }
            }
            if (tempjoy2[inP] && (joystickActive[inP] && (Input.GetAxis(joystickString[inP]) < 0.1f)) && (joystickActive2[inP] && (Input.GetAxis(joystickString2[inP]) < 0.1f)))
            {
                isInputUp[inP] = true;
                tempjoy2[inP] = false;
            }
            if (tempjoy2[inP] && (!joystickActive[inP]) && (joystickActive2[inP] && (Input.GetAxis(joystickString2[inP]) < 0.1f)))
            {
                isInputUp[inP] = true;
                tempjoy2[inP] = false;
            }
            if (tempjoy2[inP] && (!joystickActive2[inP]) && (joystickActive[inP] && (Input.GetAxis(joystickString[inP]) < 0.1f)))
            {
                isInputUp[inP] = true;
                tempjoy2[inP] = false;
            }
        }
    }
    void saveInputs()
    {
        // *** save input configuration ***
        // ********************************
        // temporary string to hold the KeyCodes
        string KeyCodes_TempString = "";
        string Joystick_TempString = "";
        string Names_TempString = "";
        string KeyCodes_TempString2 = "";
        string Joystick_TempString2 = "";
        string Names_TempString2 = "";
        // go thru all keycodes
        for (int sn = DescriptionString.Length - 1; sn > -1; sn--)
        {
            // add every key to our temp. keycode string,also add "*" seperators
            KeyCodes_TempString = (int)inputKey[sn] + "*" + KeyCodes_TempString;
            // add joystick data to our temp. Joystick string,also add "*" seperators
            Joystick_TempString = joystickString[sn] + "*" + Joystick_TempString;
            // add the names data to our temp. name string,also add "*" seperators
            Names_TempString = inputString[sn] + "*" + Names_TempString;
            // add every key to our temp. keycode string,also add "*" seperators
            KeyCodes_TempString2 = (int)inputKey2[sn] + "*" + KeyCodes_TempString2;
            // add joystick data to our temp. Joystick string,also add "*" seperators
            Joystick_TempString2 = joystickString2[sn] + "*" + Joystick_TempString2;
            // add the names data to our temp. name string,also add "*" seperators
            Names_TempString2 = inputString2[sn] + "*" + Names_TempString2;
        }
        // save the 6 strings to the PlayerPrefs
        PlayerPrefs.SetString("KeyCodes", KeyCodes_TempString);
        PlayerPrefs.SetString("Joystick_input", Joystick_TempString);
        PlayerPrefs.SetString("Names_input", Names_TempString);

        PlayerPrefs.SetString("KeyCodes2", KeyCodes_TempString2);
        PlayerPrefs.SetString("Joystick_input2", Joystick_TempString2);
        PlayerPrefs.SetString("Names_input2", Names_TempString2);
        // save the length to the PlayerPrefs
        PlayerPrefs.SetInt("KeyLength", DescriptionString.Length);
        // ********************************
    
    }
    void reset2defaults()
    {
        // if you didn't set the default inputkeys correct we make it the right length (and clear it)
        if (default_inputKeys.Length != DescriptionString.Length) { default_inputKeys = new KeyCode[DescriptionString.Length]; }
        if (alt_default_inputKeys.Length != default_inputKeys.Length) { alt_default_inputKeys = new KeyCode[default_inputKeys.Length]; }

        // *** save input configuration ***
        // ********************************
        // temporary string to hold the KeyCodes
        string KeyCodes_TempString = "";
        string Joystick_TempString = "";
        string Names_TempString = "";

        string KeyCodes_TempString2 = "";
        string Joystick_TempString2 = "";
        string Names_TempString2 = "";

        // go thru all keycodes
        for (int sn = DescriptionString.Length - 1; sn > -1; sn--)
        {
            // add every key to our temp. keycode string,also add "*" seperators
            KeyCodes_TempString = (int)default_inputKeys[sn] + "*" + KeyCodes_TempString;
            // add joystick data to our temp. Joystick string,also add "*" seperators
            Joystick_TempString = Joystick_TempString + "#*";
            // add the names data to our temp. name string,also add "*" seperators
            Names_TempString = default_inputKeys[sn].ToString() + "*" + Names_TempString;

            // save the 3 strings to the PlayerPrefs
            PlayerPrefs.SetString("KeyCodes", KeyCodes_TempString);
            PlayerPrefs.SetString("Joystick_input", Joystick_TempString);
            PlayerPrefs.SetString("Names_input", Names_TempString);


                // add every key to our temp. keycode string,also add "*" seperators
                KeyCodes_TempString2 = (int)alt_default_inputKeys[sn] + "*" + KeyCodes_TempString2;
                // add joystick data to our temp. Joystick string,also add "*" seperators
                Joystick_TempString2 = Joystick_TempString2 + "#*";
                // add the names data to our temp. name string,also add "*" seperators
                Names_TempString2 = alt_default_inputKeys[sn].ToString() + "*" + Names_TempString2;

                // save the 3 strings to the PlayerPrefs
                PlayerPrefs.SetString("KeyCodes2", KeyCodes_TempString2);
                PlayerPrefs.SetString("Joystick_input2", Joystick_TempString2);
                PlayerPrefs.SetString("Names_input2", Names_TempString2);
                // save the length to the PlayerPrefs
                PlayerPrefs.SetInt("KeyLength", DescriptionString.Length);
        }



        // ********************************
    }
    void loadConfig()
    {
        // *** load input configuration ***
        // ********************************
        // load the input from the playerprefs
        string KeyCodes_loadstring = PlayerPrefs.GetString("KeyCodes");
        string Joystick_loadstring = PlayerPrefs.GetString("Joystick_input");
        string Names_loadstring = PlayerPrefs.GetString("Names_input");
        string KeyCodes_loadstring2 = PlayerPrefs.GetString("KeyCodes2");
        string Joystick_loadstring2 = PlayerPrefs.GetString("Joystick_input2");
        string Names_loadstring2 = PlayerPrefs.GetString("Names_input2");

        // split them up and put them in an array
        string[] KeyCode_prefs = KeyCodes_loadstring.Split('*');
        joystickString = Joystick_loadstring.Split('*');
        inputString = Names_loadstring.Split('*');

        string[] KeyCode_prefs2 = KeyCodes_loadstring2.Split('*');
        joystickString2 = Joystick_loadstring2.Split('*');
        inputString2 = Names_loadstring2.Split('*');

        for (int sn = 0; sn < DescriptionString.Length; sn++)
        {
            // convert the strings -> ints -> KeyCodes array
            int KeyCode_prefs_temp;
            int.TryParse(KeyCode_prefs[sn], out KeyCode_prefs_temp);
            inputKey[sn] = (KeyCode)KeyCode_prefs_temp;

            int KeyCode_prefs_temp2;
            int.TryParse(KeyCode_prefs2[sn], out KeyCode_prefs_temp2);
            inputKey2[sn] = (KeyCode)KeyCode_prefs_temp2;

            // fill up the joystickActive array with the joystickString array data
            if (joystickString[sn] == "#") { joystickActive[sn] = false; }
            else { joystickActive[sn] = true; }
            // fill up the joystickActive array with the joystickString array data
            if (joystickString2[sn] == "#") { joystickActive2[sn] = false; }
            else { joystickActive2[sn] = true; }
        }
  
    }
    
    /*void drawButtons1()
    {
        // mouse and menu stuff
        float inputy = Boxes_Y;
        float ix = Input.mousePosition.x;
        float iy = Input.mousePosition.y;
        Vector3 transMouse = GUI.matrix.inverse.MultiplyPoint3x4(new Vector3(ix, Screen.height - iy, 1));
        GUI.skin = OurSkin;
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        GUI.Box(new Rect(90, 90, Screen.width-180, Screen.height-180), "","window");
        GUI.Box(new Rect(100, 100, Screen.width-200, Screen.height-200), "","window");

        // draw the logo
        GUI.DrawTexture(new Rect(Screen.width / 2 - (inputManagerLogo.width / 2), 150, inputManagerLogo.width, inputManagerLogo.height), inputManagerLogo);
        // loop thru all elements of our arrays

        GUI.Label(new Rect(DescriptionBox_X, inputy-10, DescriptionSize, 25), "name","textfield");
        GUI.Label(new Rect(InputBox1_X, inputy - 10, DescriptionSize, 25), "input", "textfield");
        GUI.Label(new Rect(InputBox2_X, inputy - 10, DescriptionSize, 25), "alt input", "textfield");
        for (int n = 0; n < DescriptionString.Length; n++)
        {
            // add the margin between buttons
            inputy += BoxesMargin_Y;
            // Description (name) of the buttons
            GUI.Label(new Rect(DescriptionBox_X, inputy, DescriptionSize, 25), DescriptionString[n],"box");
            Rect buttonRec = new Rect(InputBox1_X, inputy, buttonSize, 25);
            // the button with the inputkey 
            GUI.Button(buttonRec, inputString[n]);
            // add # to empty joystrickstrings 
            if (joystickActive[n] == false && inputKey[n] == KeyCode.None) { joystickString[n] = "#"; }
            // marks the selected input button
            if (inputBool[n] == true)
            {
                GUI.Toggle(buttonRec, true, "", OurSkin.button);
            }
            // if the button gets pressed
            if (buttonRec.Contains(transMouse) && Input.GetMouseButtonUp(0) && tempbool == false)
            {
                // were ready to receive input
                tempbool = true;

                // on this element
                inputBool[n] = true;

                // reset our interval
                lastInterval = Time.realtimeSinceStartup;
            }
            // reset button code
            if (GUI.Button(new Rect(resetbuttonX, resetbuttonLocY, buttonSize, 25), resetbuttonText) && Input.GetMouseButtonUp(0))
            {
                PlayerPrefs.DeleteAll();
                reset2defaults();
                loadConfig();
                saveInputs();
            }
            // KEYBOARD
            // if we received key-input thats not ESCAPE 
            if (Event.current.type == EventType.KeyDown && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // store the keycode in our key variable
                inputKey[n] = Event.current.keyCode;
                // we don't want more input on this element
                inputBool[n] = false;
                // Update the buttons inputstring to the new key
                inputString[n] = inputKey[n].ToString();
                // we don't wait for key input anymore
                tempbool = false;
                // state we are not using the joystick axis on this one
                joystickActive[n] = false;
                // empty the joysticString
                joystickString[n] = "#";
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubles(inputKey[n],n,1);
            }

            if (mouseButtonsOn)
            {
                // MOUSE
                // this is the enum id of the first mousebutton
                int key = 323;
                for (int m = 0; m < 6; m++)
                {
                    // we check for 6 mousebuttons
                    if (Input.GetMouseButton(m) && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
                    {
                        key += m;
                        // add the key 
                        inputKey[n] = (KeyCode)key;
                        // we don't want more input on this element
                        inputBool[n] = false;
                        // Update the buttons inputstring to the new key
                        inputString[n] = inputKey[n].ToString();
                        // state we are not using the joystick axis on this one
                        joystickActive[n] = false;
                        // empty the joysticString
                        joystickString[n] = "#";
                        // save our configuration
                        saveInputs();
                        // check for doubles
                        checDoubles(inputKey[n], n,1);
                    }
                }
            }
            //JOYSTICK
            for (int joyK = 350; joyK < 409; joyK++)
                // check for all joystick buttons
                if (Input.GetKey((KeyCode)joyK) && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // add the key 
                    inputKey[n] = (KeyCode)joyK;
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // Update the buttons inputstring to the new key
                    inputString[n] = inputKey[n].ToString();
                    // we don't wait for key input anymore
                    tempbool = false;
                    // state we are not using the joystick axis on this one
                    joystickActive[n] = false;
                    // empty the joysticString
                    joystickString[n] = "#";
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubles(inputKey[n], n,1);
                }
            //MOUSE AXS
            //----------------------------------------------------------------
            // joystick axis is kind hacky but i don't see a way around it
            // we set the axis in the unity inputmanager and then use them here
            // so we can set them to anything we want
            //----------------------------------------------------------------
            // only use mouseaxis when we turned it on!
            if (mouseAxisOn)
            {
                if (Input.GetAxis("MouseUp") == 1 && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive[n] = true;
                    joystickString[n] = "MouseUp";
                    // Update the buttons inputstring to the new key
                    inputString[n] = "Mouse Up";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubleAxis(joystickString[n], n,1);
                }
                if (Input.GetAxis("MouseDown") == 1 && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive[n] = true;
                    joystickString[n] = "MouseDown";
                    // Update the buttons inputstring to the new key
                    inputString[n] = "Mouse Down";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubleAxis(joystickString[n], n,1);
                }
                if (Input.GetAxis("MouseLeft") == 1 && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive[n] = true;
                    joystickString[n] = "MouseLeft";
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // Update the buttons inputstring to the new key
                    inputString[n] = "Mouse Left";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubleAxis(joystickString[n], n,1);
                }
                if (Input.GetAxis("MouseRight") == 1 && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive[n] = true;
                    joystickString[n] = "MouseRight";
                    // Update the buttons inputstring to the new key
                    inputString[n] = "Mouse Right";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubleAxis(joystickString[n], n,1);
                }
            }
            if (mouseButtonsOn)
            {
                if (Input.GetAxis("MouseScrollUp") > 0 && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive[n] = true;
                    joystickString[n] = "MouseScrollUp";
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // Update the buttons inputstring to the new key
                    inputString[n] = "Mouse scroll Up";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubleAxis(joystickString[n], n,1);
                }
                if (Input.GetAxis("MouseScrollDown") > 0 && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive[n] = true;
                    joystickString[n] = "MouseScrollDown";
                    // we don't want more input on this element
                    inputBool[n] = false;
                    // Update the buttons inputstring to the new key
                    inputString[n] = "Mouse scroll Down";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubleAxis(joystickString[n], n,1);
                }
            }
            //JOYSTICK AXS
            //----------------------------------------------------------------
            // joystick axis is kind hacky but i don't see a way around it
            // we set the axis in the unity inputmanager and then use them here
            // so we can set them to anything we want
            //----------------------------------------------------------------
            if (Input.GetAxis("JoystickUp") > 0.5f && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "JoystickUp";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Up";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("JoystickDown") > 0.5f && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "JoystickDown";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Down";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("JoystickLeft") > 0.5f && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "JoystickLeft";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Left";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("JoystickRight") > 0.5f && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "JoystickRight";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Right";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("Joystick_3a") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_3a";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 3 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("Joystick_3b") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_3b";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 3 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("Joystick_4a") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_4a";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 4 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("Joystick_4b") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_4b";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 4 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            // ** this is commented out to support the xbox360 controller on the MAC
            if (Input.GetAxis("Joystick_5a") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_5a";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 5 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
             //
            if (Input.GetAxis("Joystick_5b") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_5b";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 5 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            // ** this is commented out to support the xbox360 controller on the MAC
            if (Input.GetAxis("Joystick_6a") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_6a";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 6 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
             //
            if (Input.GetAxis("Joystick_6b") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_6b";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 6 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("Joystick_7a") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_7a";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 7 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("Joystick_7b") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_7b";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 7 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("Joystick_8a") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_8a";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 8 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
            if (Input.GetAxis("Joystick_8b") > JoyStickSettingDeadZone && inputBool[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool[n] = false;
                // state we are using the joystick axis on this
                joystickActive[n] = true;
                joystickString[n] = "Joystick_8b";
                // Update the buttons inputstring to the new key
                inputString[n] = "Joystick Axis 8 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubleAxis(joystickString[n], n,1);
            }
        }
   
    }
    void drawButtons2()
    {
        // mouse and menu stuff
        float inputy = Boxes_Y;
        float ix = Input.mousePosition.x;
        float iy = Input.mousePosition.y;
        Vector3 transMouse = GUI.matrix.inverse.MultiplyPoint3x4(new Vector3(ix, Screen.height - iy, 1));
        GUI.skin = OurSkin;

        // loop thru all elements of our arrays

        for (int n = 0; n < DescriptionString.Length; n++)
        {
            // add the margin between buttons
            inputy += BoxesMargin_Y;
            Rect buttonRec2 = new Rect(InputBox2_X, inputy, buttonSize, 25);
            // the button with the inputkey 
            GUI.Button(buttonRec2, inputString2[n]);
            // add # to empty joystrickstrings 
            if (joystickActive2[n] == false && inputKey2[n] == KeyCode.None) { joystickString2[n] = "#"; }
            // marks the selected input button
            if (inputBool2[n] == true)
            {
                GUI.Toggle(buttonRec2, true, "", OurSkin.button);
            }
            // if the button gets pressed
            if (buttonRec2.Contains(transMouse) && Input.GetMouseButtonUp(0) && tempbool == false)
            {
                // were ready to receive input
                tempbool = true;

                // on this element
                inputBool2[n] = true;

                // reset our interval
                lastInterval = Time.realtimeSinceStartup;
            }
           

            // KEYBOARD
            // if we received key-input thats not ESCAPE 
            if (Event.current.type == EventType.KeyDown && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // store the keycode in our key variable
                inputKey2[n] = Event.current.keyCode;
                // we don't want more input on this element
                inputBool2[n] = false;
                // Update the buttons inputstring to the new key
                inputString2[n] = inputKey2[n].ToString();
                // we don't wait for key input anymore
                tempbool = false;
                // state we are not using the joystick axis on this one
                joystickActive2[n] = false;
                // empty the joysticString
                joystickString2[n] = "#";
                // save our configuration
                saveInputs();
                // check for doubles
                checDoubles(inputKey2[n], n,2);
            }

            if (mouseButtonsOn)
            {
                // MOUSE
                // this is the enum id of the first mousebutton
                int key = 323;
                for (int m = 0; m < 6; m++)
                {
                    // we check for 6 mousebuttons
                    if (Input.GetMouseButton(m) && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
                    {
                        key += m;
                        // add the key 
                        inputKey2[n] = (KeyCode)key;
                        // we don't want more input on this element
                        inputBool2[n] = false;
                        // Update the buttons inputstring to the new key
                        inputString2[n] = inputKey2[n].ToString();
                        // state we are not using the joystick axis on this one
                        joystickActive2[n] = false;
                        // empty the joysticString
                        joystickString2[n] = "#";
                        // save our configuration
                        saveInputs();
                        // check for doubles
                        checDoubles(inputKey2[n], n,2);
                    }
                }
            }

            //JOYSTICK
            for (int joyK = 350; joyK < 409; joyK++)
                // check for all joystick buttons
                if (Input.GetKey((KeyCode)joyK) && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // add the key 
                    inputKey2[n] = (KeyCode)joyK;
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // Update the buttons inputstring to the new key
                    inputString2[n] = inputKey2[n].ToString();
                    // we don't wait for key input anymore
                    tempbool = false;
                    // state we are not using the joystick axis on this one
                    joystickActive2[n] = false;
                    // empty the joysticString
                    joystickString2[n] = "#";
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubles(inputKey2[n], n,2);
                }
            //MOUSE AXS
            //----------------------------------------------------------------
            // joystick axis is kind hacky but i don't see a way around it
            // we set the axis in the unity inputmanager and then use them here
            // so we can set them to anything we want
            //----------------------------------------------------------------
            // only use mouseaxis when we turned it on!
            if (mouseAxisOn)
            {
                if (Input.GetAxis("MouseUp") == 1 && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey2[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive2[n] = true;
                    joystickString2[n] = "MouseUp";
                    // Update the buttons inputstring to the new key
                    inputString2[n] = "Mouse Up";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    // check for doubles
                    checDoubleAxis(joystickString2[n], n,2);
                }
                if (Input.GetAxis("MouseDown") == 1 && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey2[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive2[n] = true;
                    joystickString2[n] = "MouseDown";
                    // Update the buttons inputstring to the new key
                    inputString2[n] = "Mouse Down";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    checDoubleAxis(joystickString2[n], n,2);
                }
                if (Input.GetAxis("MouseLeft") == 1 && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey2[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive2[n] = true;
                    joystickString2[n] = "MouseLeft";
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // Update the buttons inputstring to the new key
                    inputString2[n] = "Mouse Left";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    checDoubleAxis(joystickString2[n], n,2);
                }
                if (Input.GetAxis("MouseRight") == 1 && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey2[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive2[n] = true;
                    joystickString2[n] = "MouseRight";
                    // Update the buttons inputstring to the new key
                    inputString2[n] = "Mouse Right";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    checDoubleAxis(joystickString2[n], n,2);
                }
            }
            if (mouseButtonsOn)
            {
                if (Input.GetAxis("MouseScrollUp") > 0 && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey2[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive2[n] = true;
                    joystickString2[n] = "MouseScrollUp";
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // Update the buttons inputstring to the new key
                    inputString2[n] = "Mouse scroll Up";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    checDoubleAxis(joystickString2[n], n,2);
                }
                if (Input.GetAxis("MouseScrollDown") > 0 && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
                {
                    // delete the key 
                    inputKey2[n] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // state we are using the joystick axis on this
                    joystickActive2[n] = true;
                    joystickString2[n] = "MouseScrollDown";
                    // we don't want more input on this element
                    inputBool2[n] = false;
                    // Update the buttons inputstring to the new key
                    inputString2[n] = "Mouse scroll Down";
                    // we don't wait for key input anymore
                    tempbool = false;
                    // save our configuration
                    saveInputs();
                    checDoubleAxis(joystickString2[n], n,2);
                }
            }

            //JOYSTICK AXS
            //----------------------------------------------------------------
            // joystick axis is kind hacky but i don't see a way around it
            // we set the axis in the unity inputmanager and then use them here
            // so we can set them to anything we want
            //----------------------------------------------------------------
            if (Input.GetAxis("JoystickUp") > 0.5f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "JoystickUp";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Up";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("JoystickDown") > 0.5f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "JoystickDown";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Down";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("JoystickLeft") > 0.5f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "JoystickLeft";
                // we don't want more input on this element
                inputBool2[n] = false;
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Left";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("JoystickRight") > 0.5f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "JoystickRight";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Right";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("Joystick_3a") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_3a";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 3 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("Joystick_3b") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_3b";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 3 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("Joystick_4a") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_4a";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 4 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("Joystick_4b") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_4b";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 4 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            // ** this is commented out to support the xbox360 controller on the MAC
            if (Input.GetAxis("Joystick_5a") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_5a";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 5 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            //
            if (Input.GetAxis("Joystick_5b") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_5b";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 5 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            // ** this is commented out to support the xbox360 controller on the MAC
            if (Input.GetAxis("Joystick_6a") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_6a";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 6 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            //
            if (Input.GetAxis("Joystick_6b") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_6b";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 6 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("Joystick_7a") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_7a";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 7 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("Joystick_7b") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_7b";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 7 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("Joystick_8a") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_8a";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 8 +";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
            if (Input.GetAxis("Joystick_8b") > 0.8f && inputBool2[n] == true && Event.current.keyCode != KeyCode.Escape)
            {
                // delete the key 
                inputKey2[n] = KeyCode.None;
                // we don't want more input on this element
                inputBool2[n] = false;
                // state we are using the joystick axis on this
                joystickActive2[n] = true;
                joystickString2[n] = "Joystick_8b";
                // Update the buttons inputstring to the new key
                inputString2[n] = "Joystick Axis 8 -";
                // we don't wait for key input anymore
                tempbool = false;
                // save our configuration
                saveInputs();
                checDoubleAxis(joystickString2[n], n,2);
            }
        }

    }*/

    void checDoubles(KeyCode testkey,int o,int p)
    {
        if (!allowDuplicates)
        {
            for (int m = 0; m < DescriptionString.Length; m++)
            {
                // check if we allready have testkey in our list and make sure we dont compare with itself

                // input buttons 
                if (testkey == inputKey[m] && (m != o || p == 2))
                {
                    // reset the double key
                    inputKey[m] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool[m] = false;
                    // Update the buttons inputstring to the new key
                    inputString[m] = inputKey[m].ToString();
                    // state we are not using the joystick axis on this one
                    joystickActive[m] = false;
                    // empty the joysticString
                    joystickString[m] = "#";
                    // save our configuration
                    saveInputs();
                }

                // alt input buttons 
                if (testkey == inputKey2[m] && (m != o || p == 1))
                {
                    // reset the double key
                    inputKey2[m] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool2[m] = false;
                    // Update the buttons inputstring to the new key
                    inputString2[m] = inputKey2[m].ToString();
                    // state we are not using the joystick axis on this one
                    joystickActive2[m] = false;
                    // empty the joysticString
                    joystickString2[m] = "#";
                    // save our configuration
                    saveInputs();
                }
            }
        }
    }
    void checDoubleAxis(string testAxisString, int o,int p)
    {
        if (!allowDuplicates)
        {
            for (int m = 0; m < DescriptionString.Length; m++)
            {
                // check if we allready have testkey in our list and make sure we dont compare with itself

                // input buttons 
                if (testAxisString == joystickString[m] && (m != o || p == 2))
                {
                    // reset the double key
                    inputKey[m] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool[m] = false;
                    // Update the buttons inputstring to the new key
                    inputString[m] = inputKey[m].ToString();
                    // state we are not using the joystick axis on this one
                    joystickActive[m] = false;
                    // empty the joysticString
                    joystickString[m] = "#";
                    // save our configuration
                    saveInputs();
                }
                // alt input buttons 
                if (testAxisString == joystickString2[m] && (m != o || p == 1))
                {
                    // reset the double key
                    inputKey2[m] = KeyCode.None;
                    // we don't want more input on this element
                    inputBool2[m] = false;
                    // Update the buttons inputstring to the new key
                    inputString2[m] = inputKey2[m].ToString();
                    // state we are not using the joystick axis on this one
                    joystickActive2[m] = false;
                    // empty the joysticString
                    joystickString2[m] = "#";
                    // save our configuration
                    saveInputs();
                }

            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------
    // Modified by Erik Ross for use with the Super Character Controller demo Mario 64 project
    //--------------------------------------------------------------------------------------------------------------

    private const float JoyStickSettingDeadZone = 0.6f;

    public void SetKey(KeyCode key, int inputNumber)
    {
        // store the keycode in our key variable
        inputKey[inputNumber] = key;
        // we don't want more input on this element
        inputBool[inputNumber] = false;
        // Update the buttons inputstring to the new key
        inputString[inputNumber] = inputKey[inputNumber].ToString();
        // we don't wait for key input anymore
        //tempbool = false;
        // state we are not using the joystick axis on this one
        joystickActive[inputNumber] = false;
        // empty the joysticString
        joystickString[inputNumber] = "#";
        // save our configuration
        saveInputs();
        // check for doubles
        checDoubles(inputKey[inputNumber], inputNumber, 1);
    }

    public void SetAxis(string JoyStickAxis, int inputNumber)
    {
        // delete the key 
        inputKey[inputNumber] = KeyCode.None;
        // we don't want more input on this element
        inputBool[inputNumber] = false;
        // state we are using the joystick axis on this
        joystickActive[inputNumber] = true;
        joystickString[inputNumber] = JoyStickAxis;
        // Update the buttons inputstring to the new key
        inputString[inputNumber] = JoyStickAxis + " +";
        // we don't wait for key input anymore
        //tempbool = false;
        // save our configuration
        saveInputs();
        // check for doubles
        checDoubleAxis(joystickString[inputNumber], inputNumber, 1);
    }

    public string[] GetInputStrings()
    {
        return inputString;
    }

    private void SetDefaults()
    {
        for (int i = 0; i < default_inputKeys.Length; i++)
        {
            SetKey(default_inputKeys[i], i);
        }
    }
}

