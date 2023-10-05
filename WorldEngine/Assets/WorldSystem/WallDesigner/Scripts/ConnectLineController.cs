using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace WallDesigner
{
    public class ConnectLineController
    {
        Node inDragNode;
        public bool isLineInDraw;
        private static ConnectLineController instance;
        private ConnectLineController()
        {
            isLineInDraw = false;
            inDragNode = new Node();
        }
        public static ConnectLineController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConnectLineController();
                }
                return instance;
            }
        }
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float width)
        {
            GUI.color = Color.white;
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            GUIUtility.RotateAroundPivot(angle, start);
            GUI.DrawTexture(new Rect(start.x, start.y, (end - start).magnitude, width), texture);
            GUIUtility.RotateAroundPivot(-angle, start);
        }
        public bool IsLineInDraw() => isLineInDraw;
        public void SetInDragNode(Node n) => inDragNode = n;
        public Node GetInDragNode() => inDragNode;
        public bool isGetNodeInDraw() => inDragNode.GetType() == typeof(GetNode);
        public static bool isGetNode(Node node) => node.GetType() == typeof(GetNode);
        public static void NodeClicked(Node node, bool isGetNode)
        {
            if (!ConnectLineController.Instance.IsLineInDraw())
            {
                CheckNotLineInDrawState(node, isGetNode);
            }
            else if (ConnectLineController.Instance.GetInDragNode() == node)
            {
                node.clicked = false;
                node.color = node.ConnectedNode == null ? Color.red: Color.blue;
                ConnectLineController.Instance.isLineInDraw = false;
                ConnectLineController.Instance.SetInDragNode(null);
            }
            else if ((ConnectLineController.Instance.isGetNodeInDraw() && !isGetNode) || (!ConnectLineController.Instance.isGetNodeInDraw() && isGetNode))
            {
                ConnectTwoNode(node);
            }
        }
        public void CheckClick()
        {
            if(isLineInDraw)
            {
                inDragNode.clicked = false;
                inDragNode.color = inDragNode.ConnectedNode == null ? Color.red : Color.blue;
                ConnectLineController.Instance.SetInDragNode(null);
                isLineInDraw = false;
            }
        }
        private static void CheckNotLineInDrawState(Node node, bool isGetNode)
        {
            if(!isGetNode || (isGetNode && node.ConnectedNode == null))
            {
                node.clicked = true;
                node.color = Color.green;
                ConnectLineController.Instance.isLineInDraw = true;
                ConnectLineController.Instance.SetInDragNode(node);
            }
        }
        private static void ConnectTwoNode(Node node)
        {
            if(isGetNode(node) && node.ConnectedNode != null )
            {
                return;
            }

            node.clicked = false;
            node.color = Color.blue;
            ConnectLineController.Instance.isLineInDraw = false;
            ConnectLineController.Instance.GetInDragNode().clicked = false;
            ConnectLineController.Instance.GetInDragNode().color = Color.blue;
            node.ConnectedNode = ConnectLineController.Instance.GetInDragNode();
            ConnectLineController.Instance.GetInDragNode().ConnectedNode = node;
        }
    }
}
