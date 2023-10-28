using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
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