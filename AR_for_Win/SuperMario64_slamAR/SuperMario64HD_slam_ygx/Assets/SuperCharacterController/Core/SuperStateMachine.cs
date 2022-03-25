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
/// StateMachine for use with the SuperCharacterController.
/// </summary>
public class SuperStateMachine : MonoBehaviour {

    protected float timeEnteredState;

    public class State
    {
        public Action DoSuperUpdate = DoNothing;
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
        state.DoSuperUpdate = ConfigureDelegate<Action>("SuperUpdate", DoNothing);
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

    void SuperUpdate()
    {
        EarlyGlobalSuperUpdate();

        state.DoSuperUpdate();

        LateGlobalSuperUpdate();
    }

    protected void DelegateMethod(string method, object[] parameters)
    {
        var mtd = GetType().GetMethod(state.currentState.ToString() + "_" + method, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        if (mtd == null)
        {
            mtd = GetType().GetMethod("Global" + "_" + method, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        mtd.Invoke(this, parameters);
    }

    protected virtual void EarlyGlobalSuperUpdate() { }

    protected virtual void LateGlobalSuperUpdate() { }

    static void DoNothing() { }
}
