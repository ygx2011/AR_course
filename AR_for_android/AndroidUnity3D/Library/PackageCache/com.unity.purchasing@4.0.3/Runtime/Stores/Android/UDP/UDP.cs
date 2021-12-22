using System;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Class containing store information for Unity Distribution Portal builds.
    /// </summary>
    public class UDP
    {
        // Unity Distribution Portal (UDP) may target to various store,
        // e.g. Xiaomi, MooStore, etc. So the <code>Name</code> should
        // be more specific in this case.
        /// <summary>
        /// The name of the specific store service under UDP. Defaults to "UDP" if not determined from the UDP package.
        /// </summary>
        public static string Name
        {
            get
            {
                try
                {
                    return StoreServiceInterface.GetName() ?? "UDP";
                }
                catch (Exception)
                {
                    return "UDP";
                }
            }
        }
    }
}
