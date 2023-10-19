using UnityEngine;

namespace WallDesigner
{
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
}