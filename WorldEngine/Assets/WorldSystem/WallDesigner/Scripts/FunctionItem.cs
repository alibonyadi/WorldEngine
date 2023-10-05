using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WallDesigner
{
    abstract public class FunctionItem
    {
        private bool isDragging = false;
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public Color basecolor;
        public Vector2 position { get; set; }
        public Action action { get; set; }
        public Rect rect;
        public List<Node> GetNodes;
        public List<Node> GiveNodes;
        public FunctionItem()
        {
            Name = "";
            ClassName = "FunctionItem";
            Description = "the Base class for Functions";
            this.position = new Vector2();
            this.rect = new Rect();
            basecolor = Color.white;
        }
        public string GetName() => Name;
        public Action GetAction() => action;
        protected void CalculateRect()
        {
            int maxNode = Mathf.Max(GetNodes.Count, GiveNodes.Count);
            float heigh = maxNode * 10 + 25;
            float width = 150;
            rect = new Rect(0, 0, width, heigh);
        }
        public void Draw()
        {
            GUI.skin.box.normal.background = Texture2D.whiteTexture;
            GUI.color = basecolor;
            GUI.contentColor = Color.black;
            DrawAndDrag();
            DrawNodes();
        }
        private void DrawAndDrag()
        {
            if (GUI.RepeatButton(rect, Name))
            {
                position = Event.current.mousePosition;
                rect = new Rect(position.x - rect.width * 0.5f, position.y - rect.height * 0.5f, rect.width, rect.height);
            }
        }
        private void DrawNodes()
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

                NodeClicked(GiveNodes[index], false);
                /*if (ConnectLineController.Instance.IsLineInDraw())
                {
                    Debug.Log(index + "s Give node Pressed!!!");
                    GiveNodes[index].clicked = true;
                    GiveNodes[index].color = Color.green;
                }*/

            }
        }

        private void DrawGetNode(int index)
        {
            GetNodes[index].position = new Vector3(rect.x - 10 + 5, rect.y + 5 + (+15 * index) + 5, 0);
            DrawNodeLine(GetNodes[index], true);
            GUI.color = GetNodes[index].color;
            if (GUI.Button(new Rect(rect.x - 10, rect.y + 5 + (+15 * index), 10, 10), ""))
            {
                NodeClicked(GetNodes[index], true);
            }
        }
        private void NodeClicked(Node node,bool isGetNode)
        {
            if (!ConnectLineController.Instance.IsLineInDraw())
            {
                node.clicked = true;
                node.color = Color.green;
                ConnectLineController.Instance.isLineInDraw = true;
                ConnectLineController.Instance.SetInDragNode(node);
            }
            else if (ConnectLineController.Instance.GetInDragNode() == node)
            {
                node.clicked = false;
                node.color = Color.red;
                ConnectLineController.Instance.isLineInDraw = false;
                ConnectLineController.Instance.SetInDragNode(null);
            }
            else if( ( ConnectLineController.Instance.isGetNodeInDraw() && isGetNode) || (!ConnectLineController.Instance.isGetNodeInDraw() && !isGetNode))
            {

            }
        }

        private void DrawNodeLine(Node node,bool isGetNode)
        {
            if(node.clicked)
            {
                ConnectLineController.DrawLine(node.position, Event.current.mousePosition, Color.yellow, 1);
            }
        }
    }
}