using System.Collections.Generic;

namespace Assets.WorldSystem.Interfaces
{
    internal interface INode
    {
        public bool IsConnected { get; }
        public INode ConnectedNode { get; set; }

        public IFunctionItem AttachedFunctionItem { get; set; }
        public IFunctionItem GetConnectedItem();
    }
}
