using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class RandomFloatProperty : Property
    {
        //private 
        public RandomFloatProperty(Rect r) : base(r)
        {
            rect = r;
            GetNodes = new List<GetPropertyNode>();
            GiveNodes = new List<GivePropertyNode>();
        }

        public override Attrebute Execute()
        {
            RandomFloatAttrebute randomFloatAtt = (RandomFloatAttrebute)attrebute;
            FloatAttrebute resultItem = new FloatAttrebute(rect, attrebute.GetFunctionItem());
            //Debug.Log("Random = "+ randomFloatAtt.mFloat);
            randomFloatAtt.GenerateRandomFloat();
            resultItem.mFloat = randomFloatAtt.mFloat;
            return resultItem;
        }
    }
}