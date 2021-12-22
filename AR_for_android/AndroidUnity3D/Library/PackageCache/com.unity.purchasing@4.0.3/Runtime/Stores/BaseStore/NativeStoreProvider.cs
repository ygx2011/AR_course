using System;
using UnityEngine.Purchasing.Extension;
using Uniject;

namespace UnityEngine.Purchasing
{
    internal class NativeStoreProvider : INativeStoreProvider
    {
        public INativeStore GetAndroidStore (IUnityCallback callback, AppStore store, IPurchasingBinder binder, IUtil util)
        {
            INativeStore nativeStore;
            try
            {
                nativeStore = GetAndroidStoreHelper(callback, store, binder, util);
            }
            catch (Exception e)
            {
                throw new NotSupportedException("Failed to bind to native store: " + e.ToString());
            }

            if (nativeStore != null)
            {
                return nativeStore;
            }

            throw new NotImplementedException();
        }

        private INativeStore GetAndroidStoreHelper(IUnityCallback callback, AppStore store, IPurchasingBinder binder,
            IUtil util)
        {
            switch (store) {
                case AppStore.AmazonAppStore:
                    using (var pluginClass = new AndroidJavaClass("com.unity.purchasing.amazon.AmazonPurchasing"))
                    {
                        // Switch Android callbacks to the scripting thread, via ScriptingUnityCallback.
                        var proxy = new JavaBridge (new ScriptingUnityCallback(callback, util));
                        var instance = pluginClass.CallStatic<AndroidJavaObject> ("instance", proxy);
                        // Hook up our amazon specific functionality.
                        var extensions = new AmazonAppStoreStoreExtensions (instance);
                        binder.RegisterExtension<IAmazonExtensions> (extensions);
                        binder.RegisterConfiguration<IAmazonConfiguration> (extensions);
                        return new AndroidJavaStore (instance);
                    }

                case AppStore.UDP:
                    {
                        Type udpIapBridge = UdpIapBridgeInterface.GetClassType();
                        if (udpIapBridge != null)
                        {
                            UDPImpl udpImpl = new UDPImpl();
                            UDPBindings udpBindings = new UDPBindings();
                            udpImpl.SetNativeStore(udpBindings);
                            binder.RegisterExtension<IUDPExtensions>(udpImpl);
                            return udpBindings;
                        }
                        else
                        {
                            Debug.LogError("Cannot set Android target to UDP. Make sure you have installed UDP in your project");
                            throw new NotImplementedException();
                        }
                    }
            }

            throw new NotImplementedException();
        }

        public INativeAppleStore GetStorekit(IUnityCallback callback)
        {
            // Both tvOS and iOS use the same Objective-C linked to the XCode project.
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS) {
                return new iOSStoreBindings ();
            }
            return new OSXStoreBindings ();
        }
    }
}
