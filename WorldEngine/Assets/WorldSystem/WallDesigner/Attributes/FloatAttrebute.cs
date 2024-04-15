using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public class FloatAttrebute : Attrebute
    {
        public float mFloat;
        public float temfloat;
        float Min=0;
        float Max=10;

        public FloatAttrebute(Rect r,FunctionItem fi) : base(r,fi)
        {
            rect = r;
            property = new Property(rect);
            property.attrebute = this;
            functionItem = fi;

            GetPropertyNode getPropertyNode = new GetPropertyNode();
            getPropertyNode.AttachedProperty = property;
            property.GetNodes.Add(getPropertyNode);

            GivePropertyNode givePropertyNode = new GivePropertyNode();
            givePropertyNode.AttachedProperty = property;
            property.GiveNodes.Add(givePropertyNode);

            PropertyManager.Instance.AddProperty(property);

        }

        public void SetMinMax(float min,float max)
        {
            Min = min;
            Max = max;
        }

        public override void Draw(Vector2 position)
        {
            base.Draw(position);
            Rect boxRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y, rect.width, 20);
            GUI.color = Color.cyan;
            GUI.contentColor = Color.black;
            Rect GetPos = new Rect(boxRect.x + boxRect.width, boxRect.y + 5, 10, 10);
            property.rect = boxRect;
            //GUI.Button(new Rect(boxRect.x - 10, boxRect.y + 5,10, 10), "Give");
            Rect r = new Rect(rect.x + 15 + position.x-rect.width/2, rect.y+position.y, rect.width-30,20);
            GUI.color = Color.gray;
            GUI.Box(boxRect, "");
            GUI.Label(boxRect, name+":"+ ((int)mFloat));
            
            mFloat = GUI.HorizontalSlider(new Rect(r.x + 40f,r.y,r.width-60,r.height), mFloat, Min, Max);
            GUI.contentColor = Color.white;
            mFloat = float.Parse(GUI.TextField(new Rect(r.x + 45 + r.width - 60,r.y,25,r.height), mFloat.ToString()));
            
            if (mFloat == temfloat )
            {
                return;
            }

            temfloat = mFloat;
            if (WallEditorController.Instance.autoDraw)
                FunctionProccesor.Instance.ProcessFunctions();
        }

        public object GetValue()
        {
            FloatAttrebute att1 = (FloatAttrebute)property.Execute();
            mFloat = att1.mFloat;
            return mFloat;
        }
    }
}