using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class PropertyNode : Node
    {
        public Property AttachedProperty { get; set; }
    }

    public class GivePropertyNode : PropertyNode
    {
        public GivePropertyNode()
        {
            IsConnected = false;
            color = Color.cyan;
            AttachedProperty = null;
            ConnectedNode = null;
        }
    }

    public class GetPropertyNode : PropertyNode
    {
        public GetPropertyNode()
        {
            IsConnected = false;
            color = Color.magenta;
            AttachedProperty = null;
            ConnectedNode = null;
        }
    }
}