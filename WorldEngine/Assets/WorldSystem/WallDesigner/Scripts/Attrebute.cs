using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WallDesigner
{
    public abstract class Attrebute
    {
        protected Rect rect = new Rect();

        public Attrebute(Rect r) 
        {
            rect = r;
        }

        public virtual void Draw(Vector2 position)
        {
            Debug.Log("Drawing the Attrebute!!!");
        }
    }




    public class FloatAttrebute : Attrebute
    {
        float mFloat;
        float Min=0;
        float Max=10;

        public FloatAttrebute(Rect r) : base(r)
        {
            rect = r;
        }

        public override void Draw(Vector2 position)
        {
            
            Rect r = new Rect(rect.x + position.x, rect.y+position.y, rect.width,rect.height);
            GUI.Box(r,"");
            mFloat = GUI.HorizontalSlider(r, mFloat, Min, Max);
        }
    }
}
