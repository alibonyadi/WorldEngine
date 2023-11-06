using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public class TextureAttribute : Attrebute
    {
        public string adress;
        string tempAdress;
        [NonSerialized]
        public Texture2D texture;

        public TextureAttribute(Rect r) : base(r)
        {
            rect = r;
        }

        public override void Draw(Vector2 position)
        {
            GUI.color = Color.white;
            Rect ButtonRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y, rect.width, 20);
            if (GUI.Button(ButtonRect, "SelectTexture"))
            {
                string path = EditorUtility.OpenFilePanel("Select Texture", "", "png,jpg,jpeg");
                adress = path;
                // Load the texture from the file path
                if (!string.IsNullOrEmpty(path))
                {
                    texture = new Texture2D(2, 2);
                    byte[] rawData = File.ReadAllBytes(path);
                    texture.LoadImage(rawData);
                }
            }

            if (texture != null)
            {
                GUI.DrawTexture(new Rect(ButtonRect.x, ButtonRect.y + 20, 50, 50), texture);
            }
            if (adress == tempAdress)
                return;

            tempAdress = adress;
            if (WallEditorController.Instance.autoDraw)
                FunctionProccesor.Instance.ProcessFunctions();
        }

        public static Texture2D GenerateTextureFromPath(string path)
        {
            byte[] imageData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return texture;
        }


    }

    
}