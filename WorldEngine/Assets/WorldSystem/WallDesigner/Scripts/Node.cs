using UnityEngine;

namespace WallDesigner
{
    public class Node : INode
    {
        public bool IsConnected { get; set; }
        public Color color;
        public bool clicked = false;
        public Vector3 position = Vector3.zero;
        public IFunctionItem AttachedFunctionItem { get; set; }
        public Node ConnectedNode { get; set; }

        public IFunctionItem GetConnectedItem()
        {
            return AttachedFunctionItem == null ? null : AttachedFunctionItem;
        }

        public Node()
        {
            IsConnected = false;
            color = Color.red;
            AttachedFunctionItem = null;
            ConnectedNode = null;
        }
    }

    public class GetNode:Node
    {
        public GetNode()
        {
            IsConnected = false;
            color = Color.red;
            AttachedFunctionItem = null;
            ConnectedNode = null;
        }
    }

    public class GiveNode:Node
    {
        public GiveNode()
        {
            IsConnected = false;
            color = Color.red;
            AttachedFunctionItem = null;
            ConnectedNode = null;
        }
    }

}