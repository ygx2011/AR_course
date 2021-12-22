namespace UnityEngine.Purchasing.Default
{
    /// <summary>
    /// A factory for creating WinRT Store objects.
    /// </summary>
    public class Factory
    {
        /// <summary>
        /// Creates a <c>WinRTStore</c> objects.
        /// </summary>
        /// <param name="mocked"> Whether or not to use a mock store. </param>
        /// <returns> The instance of the <c>WinRTStore</c> created</returns>
        public static IWindowsIAP Create(bool mocked)
        {
            ICurrentApp app;
            if (mocked)
            {
                app = new UnibillCurrentAppSimulator();
            }
            else
            {
                app = new CurrentApp();
            }

            return new WinRTStore(app);
        }
    }
}
