
namespace WallDesigner
{
    public interface INode
    {
        public bool IsConnected { get; set; }

        public FunctionItem AttachedFunctionItem { get; set; }
        public FunctionItem GetConnectedItem();
    }
}
