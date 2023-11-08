using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class AttributeNode : Node
    {
        public Attrebute AttachedAttribute { get; set; }
    }

    public class GiveAttributeNode : AttributeNode
    {
        public GiveAttributeNode()
        {
            IsConnected = false;
            color = Color.red;
            AttachedAttribute = null;
            ConnectedNode = null;
        }
    }

    public class GetAttributeNode : AttributeNode
    {
        public GetAttributeNode()
        {
            IsConnected = false;
            color = Color.red;
            AttachedFunctionItem = null;
            ConnectedNode = null;
        }
    }
}