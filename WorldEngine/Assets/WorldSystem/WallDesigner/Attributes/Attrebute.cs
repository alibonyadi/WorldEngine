using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public abstract class Attrebute
    {
        [NonSerialized]
        protected Rect rect = new Rect();
        protected string name;
        protected Property property;
        protected FunctionItem functionItem;

        public FunctionItem GetFunctionItem() => functionItem;

        public Attrebute(Rect r,FunctionItem fi)
        {
            rect = r;
            name = "name";
            property = new Property(r);
            functionItem = fi;
        }

        public SerializedProperty SaveProperty(int index)
        {
            return property.SaveNodes(index);
        }

        public void LoadProperty(SerializedProperty item,List<FunctionItem> functionItems)
        {
            property.LoadNodes(item, functionItems);
        }

        public Property GetProperty() => property;

        public virtual void Draw(Vector2 position)
        {
            property.Draw(position);
        }

        public void SetName(string n)
        {
            name = n;
        }
    }
}
