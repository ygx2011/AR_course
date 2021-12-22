#if UNITY_TVOS || UNITY_IOS
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace UnityEditor.Purchasing
{
    class AppleCapabilities : IPostprocessBuildWithReport
    {
        const string k_StorekitFramework = "StoreKit.framework";
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            OnPostprocessBuild(report.summary.platform, report.summary.outputPath);
        }

        static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            OnPostprocessBuildForApple(path);
        }

        static void OnPostprocessBuildForApple(string path)
        {
            var projPath = PBXProject.GetPBXProjectPath(path);
            var proj = new PBXProject();
            proj.ReadFromFile(projPath);

            AddStoreKitFramework(proj, projPath);
            AddInAppPurchasingCapability(projPath, proj);
        }

        static void AddInAppPurchasingCapability(string projPath, PBXProject proj)
        {
            var manager = new ProjectCapabilityManager(
                projPath,
                null,
                targetGuid: proj.GetUnityMainTargetGuid()
            );
            manager.AddInAppPurchase();
            manager.WriteToFile();
        }

        static void AddStoreKitFramework(PBXProject proj, string projPath)
        {
            foreach (var targetGuid in new [] {proj.GetUnityMainTargetGuid(), proj.GetUnityFrameworkTargetGuid()})
            {
                proj.AddFrameworkToProject(targetGuid, k_StorekitFramework, false);
                System.IO.File.WriteAllText(projPath, proj.WriteToString());
            }
        }
    }
}
#endif
