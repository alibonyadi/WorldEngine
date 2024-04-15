using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public class IntAttrebute : Attrebute
    {
        public float mInt;
        public float tempInt;
        float Min = 1;
        float Max = 10;

        public IntAttrebute(Rect r,FunctionItem fi) : base(r,fi)
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

        public void SetMinMax(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public override void Draw(Vector2 position)
        {
            base.Draw(position);
            Rect boxRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y, rect.width, 20);
            GUI.color = Color.cyan;
            Rect GetPos = new Rect(boxRect.x + boxRect.width, boxRect.y + 5, 10, 10);
            property.rect = boxRect;
            Rect r = new Rect(rect.x + 15 + position.x - rect.width / 2, rect.y + position.y, rect.width - 30, 20);
            GUI.color = Color.gray;
            GUI.Box(boxRect, "");
            GUI.Label(boxRect, name + ":" + ((int)mInt));
            mInt = Mathf.Round(GUI.HorizontalSlider(new Rect(r.x + 40f, r.y, r.width - 60, r.height), mInt, Min, Max));
            GUI.contentColor = Color.white;
            mInt = int.Parse(GUI.TextField(new Rect(r.x + 45 + r.width - 60, r.y, 25, r.height), mInt.ToString()));


            property.rect = boxRect;

            if (mInt == tempInt)
            {
                return;
            }

            tempInt = mInt;
            if (WallEditorController.Instance.autoDraw)
                FunctionProccesor.Instance.ProcessFunctions();
        }

        public object GetValue()
        {
            IntAttrebute att1 = (IntAttrebute)property.Execute();
            mInt = att1.mInt;
            return (int)mInt;
        }
    }
}