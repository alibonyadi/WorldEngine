using System.Xml.Serialization;
using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public class Node : INode
    {
        public int id = 0; 
        public bool IsConnected { get; set; }
        public Color color;
        public bool clicked = false;
        public Vector3 position = Vector3.zero;
        [XmlElement("AttachedFunctionItem")]
        public FunctionItem AttachedFunctionItem { get; set; }
        [XmlElement("ConnectedNode")]
        public Node ConnectedNode { get; set; }

        public FunctionItem GetConnectedItem()
        {
            return AttachedFunctionItem == null ? null : AttachedFunctionItem;
        }

        public Node()
        {
            IsConnected = false;
            id = 0;
            color = Color.red;
            AttachedFunctionItem = null;
            ConnectedNode = null;
        }
    }
}