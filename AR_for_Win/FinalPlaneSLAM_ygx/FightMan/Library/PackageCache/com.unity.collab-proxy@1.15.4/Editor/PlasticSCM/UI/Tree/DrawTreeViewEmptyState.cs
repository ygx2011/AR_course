using System;
using UnityEngine;
using UnityEditor;

namespace Unity.PlasticSCM.Editor.UI.Tree
{
    internal static class DrawTreeViewEmptyState
    {
        internal static void For(
            Rect rect,
            string text)
        {
            GUIContent content = new GUIContent(text);
            Vector2 contentSize = GetContentSize(content);

            GUI.BeginGroup(rect);

            DrawLabel(
                content,
                contentSize,
                (rect.width - contentSize.x) / 2,
                rect.height / 2);

            GUI.EndGroup();
        }

        internal static void For(
            Rect rect,
            string text,
            Images.Name iconName)
        {
            GUIContent content = new GUIContent(text);
            Vector2 contentSize = GetContentSize(content);

            GUI.BeginGroup(rect);

            DrawLabelWithIcon(
                content,
                contentSize,
                (rect.width - contentSize.x) / 2,
                rect.height / 2,
                iconName);

            GUI.EndGroup();
        }

        internal static void For(
            Rect rect,
            string topText,
            string bottomText,
            Images.Name iconName)
        {
            GUIContent topContent = new GUIContent(topText);
            Vector2 topSize = GetContentSize(topContent);

            GUIContent bottomContent = new GUIContent(bottomText);
            Vector2 bottomSize = GetContentSize(bottomContent);

            GUI.BeginGroup(rect);

            DrawLabel(
                topContent,
                topSize,
                (rect.width - topSize.x) / 2,
                (rect.height - topSize.y) / 2);

            DrawLabelWithIcon(
                bottomContent,
                bottomSize,
                (rect.width - bottomSize.x) / 2,
                (rect.height + bottomSize.y) / 2,
                iconName);

            GUI.EndGroup();
        }

        internal static void For(
            Rect rect,
            string topText,
            string bottomText,
            string buttonText,
            Images.Name iconName,
            Action buttonClickedAction)
        {
            GUIContent topContent = new GUIContent(topText);
            Vector2 topSize = GetContentSize(topContent);

            GUIContent bottomContent = new GUIContent(bottomText);
            Vector2 bottomSize = GetContentSize(bottomContent);

            GUI.BeginGroup(rect);

            DrawLabel(
                topContent,
                topSize,
                (rect.width - topSize.x) / 2,
                (rect.height - topSize.y) / 2);

            DrawLabelWithIconAndButton(
                bottomContent,
                bottomSize,
                (rect.width - bottomSize.x) / 2,
                (rect.height + bottomSize.y) / 2,
                iconName,
                buttonText,
                buttonClickedAction);

            GUI.EndGroup();
        }

        static void DrawLabel(
            GUIContent content,
            Vector2 contentSize,
            float offsetX,
            float offsetY)
        {
            GUI.Label(
                new Rect(offsetX, offsetY, contentSize.x, contentSize.y),
                content,
                UnityStyles.Tree.StatusLabel);
        }

        static void DrawLabelWithIcon(
            GUIContent content,
            Vector2 contentSize,
            float offsetX,
            float offsetY,
            Images.Name iconName)
        {
            int iconSize = UnityConstants.TREEVIEW_STATUS_ICON_SIZE;
            int padding = UnityConstants.TREEVIEW_STATUS_CONTENT_PADDING;

            float iconOffsetX = offsetX - iconSize + padding;
            float contentOffsetX = offsetX + iconSize - padding;

            GUI.DrawTexture(
                new Rect(iconOffsetX, offsetY + padding, iconSize, iconSize),
                Images.GetImage(iconName),
                ScaleMode.ScaleToFit);

            GUI.Label(
                new Rect(contentOffsetX, offsetY, contentSize.x, contentSize.y),
                content,
                UnityStyles.Tree.StatusLabel
            );
        }

        static void DrawLabelWithIconAndButton(
            GUIContent content,
            Vector2 contentSize,
            float offsetX,
            float offsetY,
            Images.Name iconName,
            string buttonText,
            Action buttonClickedAction)
        {
            int iconSize = UnityConstants.TREEVIEW_STATUS_ICON_SIZE;
            int padding = UnityConstants.TREEVIEW_STATUS_CONTENT_PADDING;

            GUIContent button = new GUIContent(buttonText);
            Vector2 buttonSize = EditorStyles.miniButton.CalcSize(button);

            float iconOffsetX = offsetX - iconSize + padding - buttonSize.x / 2;
            float contentOffsetX = offsetX + iconSize - padding - buttonSize.x / 2;
            float buttonOffsetX = contentOffsetX + contentSize.x + 2 * padding;

            GUI.DrawTexture(
                new Rect(iconOffsetX, offsetY + padding, iconSize, iconSize),
                Images.GetImage(iconName),
                ScaleMode.ScaleToFit);

            GUI.Label(
                new Rect(contentOffsetX, offsetY, contentSize.x, contentSize.y),
                content,
                UnityStyles.Tree.StatusLabel
            );

            if (GUI.Button(
                new Rect(buttonOffsetX, offsetY + padding, buttonSize.x, buttonSize.y),
                button,
                EditorStyles.miniButton))
            {
                if (buttonClickedAction != null)
                    buttonClickedAction();
            }
        }

        static Vector2 GetContentSize(GUIContent content)
        {
            return ((GUIStyle)UnityStyles.Tree.StatusLabel).CalcSize(content);
        }
    }
}