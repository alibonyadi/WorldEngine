using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class MinusProperty : Property
    {
        public MinusProperty(Rect r) : base(r)
        {
            rect = r;
            GetNodes = new List<GetPropertyNode>();
            GiveNodes = new List<GivePropertyNode>();
        }

        public override Attrebute Execute()
        {
            FloatAttrebute item = new FloatAttrebute(rect, attrebute.GetFunctionItem());
            FloatAttrebute item2 = new FloatAttrebute(rect, attrebute.GetFunctionItem());
            GivePropertyNode givePropertyNode;
            GivePropertyNode givePropertyNode2;
            float sumitem1 = 0;
            float sumitem2 = 0;
            //Debug.Log(GetNodes[0].ConnectedNode.at)
            if (GetNodes[0].ConnectedNode != null)
            {
                givePropertyNode = (GivePropertyNode)GetNodes[0].ConnectedNode;
                try
                {
                    item = (FloatAttrebute)givePropertyNode.AttachedProperty.Execute();
                    sumitem1 = (float)item.GetValue();
                }
                catch
                {
                    sumitem1 = 0;
                    Debug.LogWarning("Attribute Type Missmatch!!!");
                }
            }
            if (GetNodes.Count > 1 && GetNodes[1].ConnectedNode != null)
            {
                givePropertyNode2 = (GivePropertyNode)GetNodes[1].ConnectedNode;

                try
                {
                    item2 = (FloatAttrebute)givePropertyNode2.AttachedProperty.Execute();
                    sumitem2 = (float)item2.GetValue();
                }
                catch
                {
                    sumitem2 = 0;
                    Debug.LogWarning("Attribute Type Missmatch!!!");
                }
            }
            FloatAttrebute resultItem = new FloatAttrebute(rect, attrebute.GetFunctionItem());
            resultItem.mFloat = sumitem1-sumitem2;
            return resultItem;
        }
    }
}