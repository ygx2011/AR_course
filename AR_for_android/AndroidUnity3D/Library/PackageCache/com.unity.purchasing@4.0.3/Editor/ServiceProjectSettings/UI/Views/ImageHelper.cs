using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Purchasing
{
    static class ImageHelper
    {
        internal static void AddRequiredTexture(Image image, Texture2D texture)
        {
            if (image == null)
            {
                return;
            }

            if (texture != null)
            {
                image.image = texture;
            }
            else
            {
                image.parent?.Remove(image);
            }
        }

        internal static Texture2D MakeTexture(string assetFilename)
        {
            Texture2D texture = null;

            if (!string.IsNullOrEmpty(assetFilename))
            {
                var assetPath = $"{SettingsUIConstants.packageImageRoot}/{assetFilename}";
                texture = (Texture2D)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
            }

            return texture;
        }
    }
}
