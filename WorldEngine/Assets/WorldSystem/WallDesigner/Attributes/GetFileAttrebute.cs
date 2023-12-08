using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public class GetFileAttrebute : Attrebute
    {
        public string adress;
        string tempAdress;
        public string folderlocation;
        public string extension;
        public byte[] rawData;
        public GetFileAttrebute(Rect r) : base(r)
        {
            rect = r;
        }

        public override void Draw(Vector2 position)
        {
            if (folderlocation == null)
                folderlocation = Application.dataPath;

            GUI.color = Color.white;
            Rect ButtonRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y, rect.width, 20);
            if (GUI.Button(ButtonRect, "Select File"))
            {
                string path = EditorUtility.OpenFilePanel("Select Item", folderlocation, extension/*"wall,Building,mudule"*/);
                adress = path;
                // Load the texture from the file path
                if (!string.IsNullOrEmpty(path))
                {
                    byte[] rawData = File.ReadAllBytes(path);
                    //texture.LoadImage(rawData);
                }
            }

            if (!string.IsNullOrEmpty(adress))
            {
                GUI.Label(new Rect(ButtonRect.x, ButtonRect.y + 20, 50, 50),"Loaded!!!");
            }
            if (adress == tempAdress)
                return;

            tempAdress = adress;
            if(WallEditorController.Instance.autoDraw)
                FunctionProccesor.Instance.ProcessFunctions();
        }
    }
}