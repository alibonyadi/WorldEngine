using System;
using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public class RandomColorAttrebute : Attrebute
    {
        [NonSerialized]
        public Color mColor;
        [NonSerialized]
        public Color temColor;
        float R = 1;
        float G = 1;
        float B = 1;
        float A = 1;

        public RandomColorAttrebute(Rect r) : base(r)
        {
            rect = r;
        }

        public void SetColor(Color c)
        {
            mColor = c;
            temColor = c;
            R = c.r;
            G = c.g;
            B = c.b;
            A = c.a;
        }

        public override void Draw(Vector2 position)
        {
            Rect ButtonRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y, rect.width, 20);
            Rect boxRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y+20, rect.width, 60);
            Rect r = new Rect(rect.x + 15 + position.x - rect.width / 2, rect.y + position.y, rect.width - 30, 20);
            GUI.color = mColor;
            if(GUI.Button(ButtonRect, "RandomColor"))
            {
                R = UnityEngine.Random.value; 
                G = UnityEngine.Random.value;
                B = UnityEngine.Random.value;
            }

            GUI.Box(boxRect,"");
            //GUI.Label(boxRect, name + ":" + ((int)mFloat));

            GUI.Label(new Rect(ButtonRect.x+5, r.y + 20, 300, r.height), "Red   :");
            GUI.Label(new Rect(ButtonRect.x+5, r.y + 40, 300, r.height), "Green :");
            GUI.Label(new Rect(ButtonRect.x+5, r.y + 60, 300, r.height), "Blue  :");

            GUI.color = Color.red;
            R = GUI.HorizontalSlider(new Rect(r.x + 40f, r.y+20, r.width - 30, r.height), R, 0, 1);
            GUI.color = Color.green;
            G = GUI.HorizontalSlider(new Rect(r.x + 40f, r.y+40, r.width - 30, r.height), G, 0, 1);
            GUI.color = Color.blue;
            B = GUI.HorizontalSlider(new Rect(r.x + 40f, r.y+60, r.width - 30, r.height), B, 0, 1);


            mColor = new Color(R,G,B);
            GUI.color = mColor;

            if (mColor == temColor)
            {
                return;
            }

            temColor = mColor;
            if (WallEditorController.Instance.autoDraw)
                FunctionProccesor.Instance.ProcessFunctions();
        }
    }
}
