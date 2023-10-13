﻿using UnityEngine;

namespace WallDesigner
{
    public abstract class Attrebute
    {
        protected Rect rect = new Rect();
        protected string name;
        public Attrebute(Rect r) 
        {
            rect = r;
            name = "name";
        }

        public virtual void Draw(Vector2 position)
        {
            Debug.Log("Drawing the Attrebute!!!");
        }

        public void SetName(string n)
        {
            name = n;
        }
    }

    public class FloatAttrebute : Attrebute
    {
        public float mFloat;
        public float temfloat;
        float Min=0;
        float Max=10;

        public FloatAttrebute(Rect r) : base(r)
        {
            rect = r;
        }

        public override void Draw(Vector2 position)
        {
            Rect boxRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y, rect.width, 20);
            Rect r = new Rect(rect.x + 15 + position.x-rect.width/2, rect.y+position.y, rect.width-30,20);
            GUI.color = Color.gray;
            GUI.Box(boxRect, "");
            GUI.Label(boxRect, name+":"+ ((int)mFloat));
            mFloat = GUI.HorizontalSlider(new Rect(r.x + 40f,r.y,r.width-30,r.height), mFloat, Min, Max);
            if (mFloat != temfloat )
            {
                temfloat = mFloat;
                if(WallEditorController.Instance.autoDraw)
                    FunctionProccesor.Instance.ProcessFunctions();
            }
        }
    }
}
