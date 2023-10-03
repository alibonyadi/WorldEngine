using UnityEngine;

namespace WallDesigner
{
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