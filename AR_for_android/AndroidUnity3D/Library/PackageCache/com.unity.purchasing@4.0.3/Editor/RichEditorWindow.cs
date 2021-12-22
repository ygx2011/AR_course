using UnityEngine;

namespace UnityEditor.Purchasing
{
    internal class RichEditorWindow : EditorWindow
    {
        private const string kLightLinkIconPath = "Packages/com.unity.purchasing/Icons/LinkWhite.png";
        private const string kDarkLinkIconPath = "Packages/com.unity.purchasing/Icons/LinkBlack.png";

        private GUIStyle m_LinkStyle;
        private Texture m_LightLinkIcon;
        private Texture m_DarkLinkIcon;

        internal RichEditorWindow()
        {
        }

        internal void GUILink(string linkText, string url)
        {
            m_LightLinkIcon = m_LightLinkIcon ?? AssetDatabase.LoadAssetAtPath<Texture>(kLightLinkIconPath);
            m_DarkLinkIcon = m_DarkLinkIcon ?? AssetDatabase.LoadAssetAtPath<Texture>(kDarkLinkIconPath);

            m_LinkStyle = m_LinkStyle ?? new GUIStyle();
            m_LinkStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.cyan : Color.blue;
            m_LinkStyle.contentOffset = new Vector2(6, 0); // Indent like other labels

            var linkIcon = EditorGUIUtility.isProSkin ? m_LightLinkIcon : m_DarkLinkIcon;

            var linkSize = m_LinkStyle.CalcSize(new GUIContent(linkText));
            GUILayout.Label(linkText, m_LinkStyle);
            var linkRect = GUILayoutUtility.GetLastRect();

            if (linkIcon != null)
                GUI.Label(new Rect(linkSize.x, linkRect.y, linkRect.height, linkRect.height), linkIcon);
            else
            {
                Debug.LogWarning("Cannot get icon: " + kLightLinkIconPath);
            }

            if (Event.current.type == EventType.MouseUp && linkRect.Contains(Event.current.mousePosition))
                Application.OpenURL(url);
        }

    }
}
