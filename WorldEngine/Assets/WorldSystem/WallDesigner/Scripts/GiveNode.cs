using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
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