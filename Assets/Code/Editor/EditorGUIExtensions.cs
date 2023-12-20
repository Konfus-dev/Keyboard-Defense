using UnityEditor;
using UnityEngine;

namespace KeyboardCats.Editor
{
    public static class EditorGUIExtensions
    {
        public static void Line(int height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height );
            rect.height = height;
            EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
        }
        
        public static Texture2D MakeBackgroundTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D backgroundTexture = new Texture2D(width, height);

            backgroundTexture.SetPixels(pixels);
            backgroundTexture.Apply();

            return backgroundTexture;
        }
    }
}