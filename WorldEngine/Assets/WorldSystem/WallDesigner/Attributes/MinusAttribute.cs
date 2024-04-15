using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WallDesigner
{
    [System.Serializable]
    public class MinusAttribute : Attrebute
    {
        public MinusAttribute(Rect r,FunctionItem fi) : base(r,fi)
        {
            rect = r;
            property = new MinusProperty(rect);
            property.attrebute = this;
            functionItem = fi;

            GetPropertyNode getPropertyNode = new GetPropertyNode();
            getPropertyNode.AttachedProperty = property;
            property.GetNodes.Add(getPropertyNode);

            GetPropertyNode getPropertyNode2 = new GetPropertyNode();
            getPropertyNode2.AttachedProperty = property;
            property.GetNodes.Add(getPropertyNode2);

            GivePropertyNode givePropertyNode = new GivePropertyNode();
            givePropertyNode.AttachedProperty = property;
            property.GiveNodes.Add(givePropertyNode);

            PropertyManager.Instance.AddProperty(property);

        }

        public override void Draw(Vector2 position)
        {
            base.Draw(position);
            Rect boxRect = new Rect(rect.x + position.x - rect.width / 2, rect.y + position.y, rect.width, 30);
            GUI.color = Color.cyan;
            GUI.contentColor = Color.black;
            Rect GetPos = new Rect(boxRect.x + boxRect.width, boxRect.y + 5, 10, 10);
            property.rect = boxRect;
            GUI.color = Color.gray;
            GUI.Box(boxRect, "");
            GUI.Label(boxRect, name);
        }

        public object GetValue()
        {
            FloatAttrebute att1 = (FloatAttrebute)property.Execute();
            float mFloat = att1.mFloat;
            return mFloat;
        }
    }
}
