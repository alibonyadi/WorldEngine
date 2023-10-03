using System;
using Unity.VisualScripting;
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
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();

            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            GUIUtility.RotateAroundPivot(angle, start);
            GUI.DrawTexture(new Rect(start.x, start.y, (end - start).magnitude, width), texture);
            GUIUtility.RotateAroundPivot(-angle, start);
        }

        public bool IsLineInDraw() =>
            isLineInDraw;

        public void SetInDragNode(Node n) => inDragNode = n;

        public Node GetInDragNode() => inDragNode;

        public bool isGetNodeInDraw() =>
             inDragNode.GetType() == typeof(GetNode);


    }
}
