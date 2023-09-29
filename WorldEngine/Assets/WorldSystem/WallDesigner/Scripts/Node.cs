using Assets.WorldSystem.Interfaces;

public class Node : INode
{
    public bool IsConnected{ get; set; }

    public IFunctionItem AttachedFunctionItem { get; set; }
    public Node ConnectedNode { get; set; }

    public IFunctionItem GetConnectedItem()
    {
        return AttachedFunctionItem==null?null: AttachedFunctionItem;
    }

    Node() 
    {
        IsConnected = false; 
        AttachedFunctionItem = null;
        ConnectedNode = null;
    }
}
