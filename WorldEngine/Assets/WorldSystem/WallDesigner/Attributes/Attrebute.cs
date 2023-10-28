using System;
using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public abstract class Attrebute
    {
        [NonSerialized]
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
}
