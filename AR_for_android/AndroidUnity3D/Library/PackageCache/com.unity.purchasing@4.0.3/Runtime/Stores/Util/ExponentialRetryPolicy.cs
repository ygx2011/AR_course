using System;
using System.Threading.Tasks;
using UnityEngine.Purchasing.Stores.Util;

namespace UnityEngine.Purchasing
{
    class ExponentialRetryPolicy : IRetryPolicy
    {
        int m_BaseRetryDelay;

        int m_MaxRetryDelay;

        int m_ExponentialFactor;

        public ExponentialRetryPolicy(int baseRetryDelay = 1000, int maxRetryDelay = 30 * 1000, int exponentialFactor = 2)
        {
            m_BaseRetryDelay = baseRetryDelay;
            m_MaxRetryDelay = maxRetryDelay;
            m_ExponentialFactor = exponentialFactor;
        }

        public void Invoke(Action<Action> actionToTry)
        {
            var currentRetryDelay = m_BaseRetryDelay;
            actionToTry(Retry);

            void Retry()
            {
                WaitAndRetry();
            }

            async Task WaitAndRetry()
            {
                await Task.Delay(currentRetryDelay);
                currentRetryDelay = AdjustDelay(currentRetryDelay);
                actionToTry(Retry);
            }
        }

        int AdjustDelay(int delay)
        {
            return Math.Min(m_MaxRetryDelay, delay * m_ExponentialFactor);
        }
    }
}
