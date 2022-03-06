using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor;
#endif
namespace MalbersAnimations
{
    /// <summary> Base on the New Unity Readme component </summary>
    public class Readme : ScriptableObject
    {
        public Texture2D icon;
        public string title;
        public Section[] sections;


        public List<string> packages = new List<string>() { "com.unity.cinemachine" , "com.unity.mathematics" };

        [Serializable]
        public class Section
        {
            public string heading, text, linkText, url;
            public UnityEngine.Object reference;
        }
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(Readme))]
    public class ReadmeEditor : Editor
    {
        static float kSpace = 16f;
        private ListRequest list;
        private AddRequest add;
        Readme M;

        private void OnEnable()
        {
            M = (Readme)target;
            list = Client.List();
        }

        bool HasPackage(string id) => id.Contains('@') ? list.Result.Any(x => x.packageId == id) : list.Result.Any(x => x.packageId.Split('@')[0] == id);

        protected override void OnHeaderGUI()
        {
            var readme = (Readme)target;
            Init();

            var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

            GUILayout.BeginHorizontal("In BigTitle");
            {
                GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
                GUILayout.Label(readme.title, TitleStyle);
            }
            GUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            var readme = (Readme)target;

            DrawPackageDependencies(readme);

            Init();

            foreach (var section in readme.sections)
            {
                float newspace = kSpace;
                if (string.IsNullOrEmpty(section.heading)) newspace = 6;

                GUILayout.Space(newspace);

                if (!string.IsNullOrEmpty(section.heading))
                {
                    GUILayout.Label(section.heading, HeadingStyle);
                }

             

                if (!string.IsNullOrEmpty(section.text))
                {
                    GUILayout.Label(section.text, BodyStyle);
                }

                if (section.reference != null)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    _ = EditorGUILayout.ObjectField(section.reference, typeof(UnityEngine.Object));
#pragma warning restore CS0618 // Type or member is obsolete
                }

                if (!string.IsNullOrEmpty(section.linkText))
                {
                    if (LinkLabel(new GUIContent(section.linkText)))
                    {
                        Application.OpenURL(section.url);
                    }
                }
            }
        }

        private void DrawPackageDependencies(Readme readme)
        {
            if (M.packages == null || M.packages.Count == 0) return;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                GUILayout.Label("Package Dependencies", EditorStyles.boldLabel);

                // We are adding a new package, wait for the operation to finish and then relist.
                if (add != null && add.IsCompleted)
                {
                    add = null;
                    list = Client.List();
                }

                if (add != null || !list.IsCompleted)
                    Repaint();// Keep refreshing while we are waiting for Packman to resolve our request.
                else
                {
                    if (!readme.packages.All(x => HasPackage(x)))
                        EditorGUILayout.HelpBox($"This Asset requires the following packages to be installed in your Project. \nPlease install all the required packages!", MessageType.Warning);
                }


                foreach (var req in readme.packages)
                {
                    Rect rect = EditorGUILayout.GetControlRect(true, 20);

                    GUI.Label(rect, new GUIContent(req), EditorStyles.label);
                    rect.width -= 160;
                    rect.x += 160;
                    if (add != null || !list.IsCompleted)
                    {
                        using (new EditorGUI.DisabledScope(true))
                        {
                            GUI.Label(rect, "checking…", EditorStyles.label);
                        }
                    }
                    else if (HasPackage(req))
                    {
                        GUI.Label(rect, $"OK \u2713", EditorStyles.boldLabel);
                    }
                    else
                    {
                        GUI.Label(rect, "Missing \u2717", EditorStyles.label);
                        rect.x += rect.width - 80;
                        rect.width = 80;
                        if (GUI.Button(rect, "Install")) add = Client.Add(req);
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        bool m_Initialized;

        GUIStyle LinkStyle { get { return m_LinkStyle; } }
        [SerializeField] GUIStyle m_LinkStyle;

        GUIStyle TitleStyle { get { return m_TitleStyle; } }
        [SerializeField] GUIStyle m_TitleStyle;

        GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
        [SerializeField] GUIStyle m_HeadingStyle;

        GUIStyle BodyStyle { get { return m_BodyStyle; } }
        [SerializeField] GUIStyle m_BodyStyle;

        void Init()
        {
            if (m_Initialized)
                return;
            m_BodyStyle = new GUIStyle(EditorStyles.label);
            m_BodyStyle.wordWrap = true;
            m_BodyStyle.fontSize = 14;

            m_TitleStyle = new GUIStyle(m_BodyStyle);
            m_TitleStyle.fontSize = 26;

            m_HeadingStyle = new GUIStyle(m_BodyStyle);
            m_HeadingStyle.fontSize = 18;

            m_LinkStyle = new GUIStyle(m_BodyStyle);
            m_LinkStyle.wordWrap = false;
            // Match selection color which works nicely for both light and dark skins
            m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            m_LinkStyle.stretchWidth = false;

            m_Initialized = true;
        }

        bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
        {
            var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

            Handles.BeginGUI();
            Handles.color = LinkStyle.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, LinkStyle);
        }
    }
#endif
}
