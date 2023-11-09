using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class Property
    {
        public Attrebute attrebute;

        public List<Node> GetNodes;
        public List<Node> GiveNodes;
        protected Rect rect = new Rect();

        public Property(Rect r) 
        {
            rect = r;
        }

        public virtual void Draw(Vector2 position)
        {
            if (GetNodes.Count > 0)
            {
                for (int i = 0; i < GetNodes.Count; i++)
                {
                    DrawGetNode(i);
                }
            }
            if (GiveNodes.Count > 0)
            {
                for (int i = 0; i < GiveNodes.Count; i++)
                {
                    DrawGiveNode(i);
                }
            }
        }

        private void DrawGiveNode(int index)
        {
            GiveNodes[index].position = new Vector3(rect.x + rect.width + 5, rect.y + 5 + (15 * index) + 5, 0);
            DrawNodeLine(GiveNodes[index], false);
            GUI.color = GiveNodes[index].color;
            if (GUI.Button(new Rect(rect.x + rect.width, rect.y + 5 + (15 * index), 10, 10), ""))
            {
                ConnectLineController.NodeClicked(GiveNodes[index], false);
            }
        }

        private void DrawGetNode(int index)
        {
            GetNodes[index].position = new Vector3(rect.x - 10 + 5, rect.y + 5 + (+15 * index) + 5, 0);
            DrawNodeLine(GetNodes[index], true);
            GUI.color = GetNodes[index].color;
            if (GUI.Button(new Rect(rect.x - 10, rect.y + 5 + (+15 * index), 10, 10), ""))
            {
                ConnectLineController.NodeClicked(GetNodes[index], true);
            }

            if (GetNodes[index].ConnectedNode != null)
            {
                ConnectLineController.DrawLine(GetNodes[index].ConnectedNode.position, GetNodes[index].position, Color.green, 2);
            }
        }

        private void DrawNodeLine(Node node, bool isGetNode)
        {
            if (node.clicked)
            {
                ConnectLineController.DrawLine(node.position, Event.current.mousePosition, Color.yellow, 1);
            }
        }

        public Attrebute Execute()
        {
            return attrebute;
        }
    }
}