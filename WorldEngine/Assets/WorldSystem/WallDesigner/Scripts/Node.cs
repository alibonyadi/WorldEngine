namespace WallDesigner
{
    public class Node : INode
    {
        public bool IsConnected { get; set; }

        public IFunctionItem AttachedFunctionItem { get; set; }
        public Node ConnectedNode { get; set; }

        public IFunctionItem GetConnectedItem()
        {
            return AttachedFunctionItem == null ? null : AttachedFunctionItem;
        }

        public Node()
        {
            IsConnected = false;
            AttachedFunctionItem = null;
            ConnectedNode = null;
        }
    }

}