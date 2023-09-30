
namespace WallDesigner
{
    public interface INode
    {
        public bool IsConnected { get; set; }

        public IFunctionItem AttachedFunctionItem { get; set; }
        public IFunctionItem GetConnectedItem();
    }
}
