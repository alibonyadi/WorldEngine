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

        public Attrebute(Rect r)
        {
            rect = r;
            name = "name";
        }



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
