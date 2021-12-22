using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace UnityEditor.Purchasing
{
    internal class BuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.WSAPlayer)
            {
                WinRTPatcher.PatchWinRTBuild();
            }
        }
    }
}
