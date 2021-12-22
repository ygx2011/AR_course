#if SERVICES_SDK_CORE_ENABLED
using System;
using UnityEditor;

namespace UnityEngine.Advertisements.Editor
{
    class EditorTimer
    {
        public event Action Elapsed;

        double m_IntervalInSeconds;

        public double IntervalInSeconds
        {
            get => m_IntervalInSeconds;
            set
            {
                m_IntervalInSeconds = Math.Max(0, value);

                if (IsRunning())
                {
                    Tick();
                }
            }
        }

        double m_StartTime;

        public bool IsRunning()
        {
            return m_StartTime > 0;
        }

        public void Restart()
        {
            m_StartTime = EditorApplication.timeSinceStartup;

            EditorApplication.update += Tick;
        }

        public void Stop()
        {
            EditorApplication.update -= Tick;

            m_StartTime = 0;
        }

        void Tick()
        {
            if (!IsElapsed())
            {
                return;
            }

            Stop();

            Elapsed?.Invoke();
        }

        bool IsElapsed()
        {
            return EditorApplication.timeSinceStartup - m_StartTime >= IntervalInSeconds;
        }
    }
}
#endif
