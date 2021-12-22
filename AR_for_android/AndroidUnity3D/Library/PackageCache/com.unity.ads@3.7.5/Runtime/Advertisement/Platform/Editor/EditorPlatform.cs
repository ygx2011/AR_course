#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace UnityEngine.Advertisements.Platform.Editor
{
    internal class EditorPlatform : INativePlatform
    {
        private const string k_BaseUrl = "http://editor-support.unityads.unity3d.com/games";
        private const string k_Version = "3.7.5";

        private IPlatform m_Platform;
        private Configuration m_Configuration;
        private Placeholder m_Placeholder;

        private bool m_StartedInitialization;
        private bool m_Initialized;
        private bool m_DebugMode;
        private bool m_LoadEnabled;
        private string m_GameId;

        private Dictionary<string, bool> m_PlacementMap = new Dictionary<string, bool>();
        private Queue<string> m_QueuedLoads = new Queue<string>();

        public void SetupPlatform(IPlatform platform)
        {
            m_Platform = platform;
        }

        public void Initialize(string gameId, bool testMode, bool enablePerPlacementLoad, IUnityAdsInitializationListener initializationListener)
        {
            if (m_StartedInitialization) return;
            m_StartedInitialization = true;
            m_LoadEnabled = enablePerPlacementLoad;
            m_GameId = gameId;
            if (m_Platform.DebugMode)
            {
                Debug.Log("UnityAdsEditor: Initialize(" + gameId + ", " + testMode + ", " + enablePerPlacementLoad + ");");
            }

#if ASSET_STORE
            Debug.LogWarning("Please consider upgrading to the Packman Distribution of the Unity Ads SDK.  The Asset Store distribution will not longer be supported after Unity 2018.3");
#endif

            var placeHolderGameObject = new GameObject("UnityAdsEditorPlaceHolderObject")
            {
                hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector
            };

            GameObject.DontDestroyOnLoad(placeHolderGameObject);
            m_Placeholder = placeHolderGameObject.AddComponent<Placeholder>();
            m_Placeholder.OnFinish += (sender, e) =>
            {
                m_Platform.UnityAdsDidFinish(e.placementId, e.showResult);

                if (!m_LoadEnabled)
                {
                    m_Platform.UnityLifecycleManager.Post(() => {
                        var placementIds = new List<string>(m_PlacementMap.Keys);
                        foreach (var placementId in placementIds)
                        {
                            foreach (var listener in GetClonedHashSet(m_Platform.Listeners))
                            {
                                listener?.OnUnityAdsReady(placementId);
                            }
                        }
                    });
                }
            };

            var configurationUrl = string.Join("/", new string[]
            {
                k_BaseUrl,
                gameId,
                string.Join("&", new string[]
                {
                    "configuration?platform=editor",
                    "unityVersion=" + Uri.EscapeDataString(Application.unityVersion),
                    "sdkVersionName=" + Uri.EscapeDataString(m_Platform.Version)
                })
            });
            var request = WebRequest.Create(configurationUrl);
            request.BeginGetResponse(result =>
            {
                try
                {
                    var response = request.EndGetResponse(result);
                    var reader = new StreamReader(response.GetResponseStream() ?? throw new Exception("Null response stream fetching configuration"));
                    var responseBody = reader.ReadToEnd();
                    try
                    {
                        m_Configuration = new Configuration(responseBody);
                        if (!m_Configuration.enabled)
                        {
                            Debug.LogWarning("gameId " + gameId + " is not enabled");
                        }

                        //Add placements to load list
                        foreach (var placement in m_Configuration.placements)
                        {
                            m_PlacementMap.Add(placement.Key, !m_LoadEnabled);
                        }

                        m_Initialized = true;
                        initializationListener?.OnInitializationComplete();

                        foreach (var placement in m_Configuration.placements)
                        {
                            if (m_LoadEnabled)
                            {
                                foreach (var queuedPlacementId in m_QueuedLoads)
                                {
                                    Load(queuedPlacementId, null);
                                }
                            }
                            else
                            {
                                m_Platform.UnityAdsReady(placement.Key);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError($"Failed to parse configuration for gameId: {gameId}");
                        Debug.Log(responseBody);
                        Debug.LogError(exception.Message);
                        m_Platform.UnityAdsDidError($"Failed to parse configuration for gameId: {gameId}");
                        initializationListener?.OnInitializationFailed(UnityAdsInitializationError.INTERNAL_ERROR, $"Failed to parse configuration for gameId: {gameId}");
                    }
                    reader.Close();
                    response.Close();
                }
                catch  (Exception exception)
                {
                    Debug.LogError($"Invalid configuration request for gameId: {gameId}");
                    Debug.LogError(exception.Message);
                    m_Platform.UnityAdsDidError($"Invalid configuration request for gameId: {gameId}");
                    initializationListener?.OnInitializationFailed(UnityAdsInitializationError.INTERNAL_ERROR, $"Invalid configuration request for gameId: {gameId}");
                }
            }, new object());
        }

        public void Load(string placementId, IUnityAdsLoadListener loadListener)
        {
            // If placementId is null, use explicit defaultPlacement to match native behaviour
            if (m_Initialized && placementId == null)
            {
                placementId = m_Configuration.defaultPlacement;
            }

            if (!m_Initialized)
            {
                m_QueuedLoads?.Enqueue(placementId);
                return;
            }

            m_Platform.UnityLifecycleManager.Post(() => {
                if (m_PlacementMap.ContainsKey(placementId))
                {
                    m_PlacementMap[placementId] = true;
                    m_Platform.UnityAdsReady(placementId);
                    loadListener?.OnUnityAdsAdLoaded(placementId);
                }
                else
                {
                    string errorMessage = "Placement " + placementId + " does not exist for gameId: " + m_GameId;
                    m_Platform.UnityAdsDidError(errorMessage);
                    loadListener?.OnUnityAdsFailedToLoad(placementId, UnityAdsLoadError.INVALID_ARGUMENT, errorMessage);
                }
            });
        }

        public void Show(string placementId, IUnityAdsShowListener showListener)
        {
            // If placementId is null, use explicit defaultPlacement to match native behaviour
            if (m_Initialized && placementId == null)
            {
                placementId = m_Configuration.defaultPlacement;
            }

            m_Platform.UnityLifecycleManager.Post(() => {
                if (IsReady(placementId))
                {
                    m_Platform.UnityAdsDidStart(placementId);
                    showListener?.OnUnityAdsShowStart(placementId);
                    m_Placeholder.Show(placementId, m_Configuration.placements[placementId]);
                    m_PlacementMap[placementId] = false;
                }
                else
                {
                    m_Platform.UnityAdsDidFinish(placementId, ShowResult.Failed);
                    showListener?.OnUnityAdsShowFailure(placementId, UnityAdsShowError.NOT_READY, $"Placement {placementId} is not ready");
                }
            });
        }

        public void SetMetaData(MetaData metaData)
        {
        }

        public bool GetDebugMode()
        {
            return m_DebugMode;
        }

        public void SetDebugMode(bool debugMode)
        {
            m_DebugMode = debugMode;
        }

        public string GetVersion()
        {
            return k_Version;
        }

        public bool IsInitialized()
        {
            return m_Initialized;
        }

        public bool IsReady(string placementId)
        {
            if (placementId == null)
            {
                return m_Initialized;
            }

            if (!m_LoadEnabled)
            {
                return m_Initialized && m_Configuration.placements.ContainsKey(placementId);
            }

            if (m_PlacementMap.ContainsKey(placementId))
            {
                return m_PlacementMap[placementId];
            }

            return false;
        }

        public PlacementState GetPlacementState(string placementId)
        {
            return IsReady(placementId) ? PlacementState.Ready : PlacementState.NotAvailable;
        }

        public string GetDefaultPlacement()
        {
            return "video";
        }

        private static HashSet<IUnityAdsListener> GetClonedHashSet(HashSet<IUnityAdsListener> hashSet)
        {
            return new HashSet<IUnityAdsListener>(hashSet);
        }
    }
}
#endif
