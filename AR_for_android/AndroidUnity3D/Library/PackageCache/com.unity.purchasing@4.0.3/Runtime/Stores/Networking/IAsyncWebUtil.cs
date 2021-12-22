using System;

namespace UnityEngine.Purchasing
{
    /// <summary>
    /// Asynchronous Coroutine based functionality encapsulated in an interface
    /// for testability outside of Unity in plain old C#.
    /// </summary>
    internal interface IAsyncWebUtil
    {
        /// <summary>
        /// Retrieves network response using HTTP Method Get with a max timeout.
        /// Triggers responseHandler if successful; otherwise, triggers errorHandler
        /// Note: If a network response cannot be retrieved before the maxTimeoutInSeconds has been reached,
        ///  the errorHandler will be triggered.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="responseHandler">Response handler.</param>
        /// <param name="errorHandler">Error handler.</param>
        /// <param name="maxTimeoutInSeconds">Max timeout in seconds.</param>
        void Get(string url, Action<string> responseHandler, Action<string> errorHandler, int maxTimeoutInSeconds = 30);

        // POST to a URL with callbacks for successful fetching and failure.
        void Post(string url, string body, Action<string> responseHandler, Action<string> errorHandler, int maxTimeoutInSeconds = 30);

        // Schedule an Action to be invoked on the scripting thread with a delay.
        void Schedule(Action a, int delayInSeconds);
    }
}
