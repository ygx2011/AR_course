#if UNITY_IOS
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngine.Advertisements.Platform.iOS
{
    internal abstract class IosNativeObject : IDisposable
    {
        protected static ConcurrentDictionary<IntPtr, IosNativeObject> s_Objects = new ConcurrentDictionary<IntPtr, IosNativeObject>();

        private IntPtr m_NativePtr;

        public IntPtr NativePtr
        {
            get => m_NativePtr;
            protected set
            {
                if (m_NativePtr == value) return;
                if (m_NativePtr != IntPtr.Zero)
                {
                    s_Objects.TryRemove(m_NativePtr, out _);
                }
                m_NativePtr = value;
                if (m_NativePtr != IntPtr.Zero)
                {
                    s_Objects.TryAdd(m_NativePtr, this);
                }
            }
        }

        protected static T Get<T>(IntPtr ptr) where T : IosNativeObject
        {
            return s_Objects.TryGetValue(ptr, out var obj) ? (T)obj : null;
        }

        public virtual void Dispose()
        {
            if (NativePtr == IntPtr.Zero) return;
            BridgeTransfer(NativePtr);
            NativePtr = IntPtr.Zero;
        }

        public bool CheckDisposedAndLogError(string message)
        {
            if (NativePtr != IntPtr.Zero) return false;
            Debug.LogErrorFormat("UnityAds SDK: {0}: Instance of type {1} is disposed. Please create a new instance in order to call any method.", message, GetType().FullName);
            return true;
        }

        [DllImport("__Internal", EntryPoint = "UnityAdsBridgeTransfer")]
        private static extern void BridgeTransfer(IntPtr x);
    }
}
#endif
