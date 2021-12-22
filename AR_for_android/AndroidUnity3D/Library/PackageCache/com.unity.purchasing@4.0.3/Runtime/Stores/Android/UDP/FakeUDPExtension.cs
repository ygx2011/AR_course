using System;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Mock implementation of the interface for UDP purchasing extensions.
    /// </summary>
    public class FakeUDPExtension : IUDPExtensions
    {
        /// <summary>
        /// Some stores return user information after initialization.
        /// </summary>
        /// <returns>UserInfo, which may be null</returns>
        public object GetUserInfo()
        {
            Type udpUserInfo = UserInfoInterface.GetClassType();
            if (udpUserInfo == null)
            {
                return null;
            }

            object userInfo = Activator.CreateInstance(udpUserInfo);

            var channelProp = UserInfoInterface.GetChannelProp();
            channelProp.SetValue(userInfo, "Fake_Channel", null);
            var userIdProp = UserInfoInterface.GetIdProp();
            userIdProp.SetValue(userInfo, "Fake_User_Id_123456", null);
            var loginTokenProp = UserInfoInterface.GetIdProp();
            loginTokenProp.SetValue(userInfo, "Fake_Login_Token", null);
            return userInfo;
        }

        /// <summary>
        /// Return the UDP initialization error.
        /// </summary>
        /// <returns> The error as a string. </returns>
        public string GetLastInitializationError()
        {
            return "Fake Initialization Error";
        }

        /// <summary>
        /// Gets the last purchase error.
        /// </summary>
        /// <returns> The error as a string. </returns>
        public string GetLastPurchaseError()
        {
            return "Fake Purchase Error";
        }

        /// <summary>
        /// Enable debug log for UDP.
        /// </summary>
        /// <param name="enable"> Whether or not the logging is to be enabled. </param>
        public void EnableDebugLog(bool enable)
        {
            return;
        }

        /// <summary>
        /// Called when a processing a purchase from UDP that is in the "OnPurchasePending" state.
        /// </summary>
        /// <param name="action">Action will be called with the product that is in the "OnPurchasePending" state.</param>
        public void RegisterPurchaseDeferredListener(Action<Product> action)
        {

        }
    }
}
