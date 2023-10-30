using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class ToggleAttribute : Attrebute
    {
        public bool mToggle;
        public bool tempToggle;
        public ToggleAttribute(Rect r) : base(r)
        {
            rect = r;
        }

        public override void Draw(Vector2 position)
        {
            Rect boxRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y, rect.width, 20);
            Rect r = new Rect(rect.x + 15 + position.x - rect.width / 2, rect.y + position.y, rect.width - 30, 20);
            GUI.color = Color.gray;
            GUI.Box(boxRect, "");
            mToggle = GUI.Toggle(boxRect, mToggle,name);
            //GUI.Label(boxRect, name + ":" + ((int)mFloat));
            //mFloat = GUI.HorizontalSlider(new Rect(r.x + 40f, r.y, r.width - 30, r.height), mFloat, Min, Max);

            if (mToggle == tempToggle)
            {
                return;
            }

            tempToggle = mToggle;
            if (WallEditorController.Instance.autoDraw)
                FunctionProccesor.Instance.ProcessFunctions();
        }
    }
}