using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace WallDesigner
{
    public abstract partial class FunctionItem
    {
        private bool isDragging = false;
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public Color basecolor;
        public Vector2 position { get; set; }
        //public Action<Mesh> action { get; set; }
        public Func<Mesh,Mesh> myFunction { get; set; }

        public Rect rect;
        public List<Node> GetNodes;
        public List<Node> GiveNodes;
        public List<System.Object> attrebutes;
        public FunctionItem()
        {
            Init();
        }

        protected void Init()
        {
            Name = "";
            ClassName = "FunctionItem";
            Description = "the Base class for Functions";
            this.position = new Vector2();
            this.rect = new Rect();
            basecolor = Color.white;
            GiveNodes = new List<Node>();
            GetNodes = new List<Node>();
            attrebutes = new List<System.Object>();
        }
        public string GetName() => Name;
        //public Action GetAction() => action;
        protected void CalculateRect()
        {
            int maxNode = Mathf.Max(GetNodes.Count, GiveNodes.Count);
            maxNode += attrebutes.Count;
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
            DrawAttrebutes();
        }

        private void DrawAttrebutes()
        {
            if (attrebutes.Count > 0)
            {
                for (int i = 0; i < attrebutes.Count; i++)
                {
                    AttrebuteType AT = AttrebuteCheckType(attrebutes[i]);
                    DrawAttrebuteOnType(new Vector2(position.x + 20, position.y + i * 15), AT);
                }
            }
        }
        private AttrebuteType AttrebuteCheckType(System.Object ATObject)
        {
            if (ATObject.GetType() == typeof(float))
                return AttrebuteType.ATFloat;
            else if(ATObject.GetType() == typeof(int))
                    return AttrebuteType.ATInt;
            else if( ATObject.GetType() == typeof(string))
                return AttrebuteType.ATString;
            else if( ATObject.GetType() == typeof(bool))
                return AttrebuteType.ATBool;

            return AttrebuteType.ATString;
        }

        private void DrawAttrebuteOnType(Vector2 startpos,AttrebuteType type)
        {
            switch (type)
            {
                case AttrebuteType.ATFloat:
                    GUI.HorizontalSlider(new Rect(startpos.x,startpos.y,60,10),)
                    break;
                case AttrebuteType.ATInt:

                    break;
                case AttrebuteType.ATString:

                    break;

                case AttrebuteType.ATBool: 

                    break;
            }
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
        //this two method can be one and get array as argument
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
        private void DrawNodeLine(Node node,bool isGetNode)
        {
            if(node.clicked)
            {
                ConnectLineController.DrawLine(node.position, Event.current.mousePosition, Color.yellow, 1);
            }
        }
    }
}