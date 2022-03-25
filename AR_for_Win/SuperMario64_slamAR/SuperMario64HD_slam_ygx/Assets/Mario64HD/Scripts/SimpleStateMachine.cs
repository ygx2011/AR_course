/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// God damn this thing is useful srsly live. laugh. love. it.
/// </summary>
public class SimpleStateMachine : MonoBehaviour
{
    public bool DebugGui;
    public Vector2 DebugGuiPosition;

    public string DebugGuiTitle = "Simple Machine";
    
    protected Enum queueCommand;

    void OnGUI()
    {
        if (DebugGui)
        {
            GUI.Box(new Rect(DebugGuiPosition.x, DebugGuiPosition.y, 200, 50), DebugGuiTitle);

            GUI.TextField(new Rect(DebugGuiPosition.x + 10, DebugGuiPosition.y + 20, 180, 20), string.Format("State: {0}", currentState));
        }
    }

    protected float timeEnteredState;

    public class State
    {
        public Action DoUpdate = DoNothing;
        public Action DoFixedUpdate = DoNothing;
        public Action DoLateUpdate = DoNothing;
        public Action DoManualUpdate = DoNothing;
        public Action enterState = DoNothing;
        public Action exitState = DoNothing;

        public Enum currentState;
    }

    public State state = new State();

    public Enum currentState
    {
        get
        {
            return state.currentState;
        }
        set
        {
            if (state.currentState == value)
                return;

            ChangingState();
            state.currentState = value;
            ConfigureCurrentState();
        }
    }

    [HideInInspector]
    public Enum lastState;

    void ChangingState()
    {
        lastState = state.currentState;
        timeEnteredState = Time.time;
    }

    void ConfigureCurrentState()
    {
        if (state.exitState != null)
        {
            state.exitState();
        }

        //Now we need to configure all of the methods
        state.DoUpdate = ConfigureDelegate<Action>("Update", DoNothing);
        state.DoFixedUpdate = ConfigureDelegate<Action>("FixedUpdate", DoNothing);
        state.DoLateUpdate = ConfigureDelegate<Action>("LateUpdate", DoNothing);
        state.DoManualUpdate = ConfigureDelegate<Action>("ManualUpdate", DoNothing);
        state.enterState = ConfigureDelegate<Action>("EnterState", DoNothing);
        state.exitState = ConfigureDelegate<Action>("ExitState", DoNothing);

        if (state.enterState != null)
        {
            state.enterState();
        }
    }

    Dictionary<Enum, Dictionary<string, Delegate>> _cache = new Dictionary<Enum, Dictionary<string, Delegate>>();

    T ConfigureDelegate<T>(string methodRoot, T Default) where T : class
    {

        Dictionary<string, Delegate> lookup;
        if (!_cache.TryGetValue(state.currentState, out lookup))
        {
            _cache[state.currentState] = lookup = new Dictionary<string, Delegate>();
        }
        Delegate returnValue;
        if (!lookup.TryGetValue(methodRoot, out returnValue))
        {
            var mtd = GetType().GetMethod(state.currentState.ToString() + "_" + methodRoot, System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod);

            if (mtd != null)
            {
                returnValue = Delegate.CreateDelegate(typeof(T), this, mtd);
            }
            else
            {
                returnValue = Default as Delegate;
            }
            lookup[methodRoot] = returnValue;
        }
        return returnValue as T;

    }

    void Update()
    {
        EarlyGlobalSuperUpdate();

        state.DoUpdate();

        LateGlobalSuperUpdate();
    }

    void FixedUpdate()
    {
        state.DoFixedUpdate();
    }

    void LateUpdate()
    {
        state.DoLateUpdate();
    }

    protected virtual void EarlyGlobalSuperUpdate() { }

    protected virtual void LateGlobalSuperUpdate() { }

    static void DoNothing() { }
}
