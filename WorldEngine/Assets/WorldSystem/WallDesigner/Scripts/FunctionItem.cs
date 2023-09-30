using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WallDesigner
{
    abstract public class FunctionItem : MonoBehaviour
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
            //GetNodes = getNodes;
            //GiveNodes = giveNodes;
        }
        /*
        public FunctionItem(string name,string description, Vector2 position, List<INode> getNodes, List<INode> giveNodes, Rect rect)
        {
            Name = name;
            Description = description;
            this.position = position;
            this.rect = rect;
            GetNodes = getNodes;
            GiveNodes = giveNodes;
        }

        public FunctionItem(string name, string description, Vector2 position, List<INode> getNodes, List<INode> giveNodes)
        {
            Name = name;
            Description = description;
            this.position = position;
            GetNodes = getNodes;
            GiveNodes = giveNodes;
            CalculateRect();
        }*/


        public string GetName() => Name;
        public Action GetAction() => action;


        protected void CalculateRect()
        {
            int maxNode = Mathf.Max(GetNodes.Count, GiveNodes.Count);

            float heigh = maxNode * 10 + 25;
            float width = 150;
            rect = new Rect(0,0,width,heigh);
        }

        public void Draw()
        {


            GUI.skin.box.normal.background = Texture2D.whiteTexture;
            GUI.color = basecolor;
            GUI.contentColor = Color.black;

            if (GUI.RepeatButton(rect, Name))
            {
                position = Event.current.mousePosition;
                rect = new Rect(position.x - rect.width * 0.5f, position.y - rect.height * 0.5f, rect.width, rect.height);
            }


            if (GUI.RepeatButton(rect, Name))
            {
                if (!isDragging)
                {
                    isDragging = true;
                    position = Event.current.mousePosition;
                    //rect = new Rect(position.x - rect.width * 0.5f, position.y - rect.height * 0.5f, rect.width, rect.height);
                }
            }
            else
            {
                isDragging = false;
            }

            if(isDragging)
            {
                Vector2 dragDelta = Event.current.mousePosition - position;
                //rect = new Rect(rect.x + dragDelta.x,);
                rect.x += dragDelta.x;
                rect.y += dragDelta.y;
            }

            GUI.color = Color.red;
            if (GetNodes.Count > 0)
            {
                for (int i = 0; i < GetNodes.Count; i++)
                {
                    GUI.Button(new Rect(rect.x - 10, rect.y - 5 - (+15 * i), 10, 10), "");
                }
            }

            if (GiveNodes.Count > 0)
            {
                for (int i = 0; i < GiveNodes.Count; i++)
                {
                    GUI.Button(new Rect(rect.x + rect.width, rect.y - 5 + (-15 * i), 10, 10), "");
                }
            }
        }

        void DragButton(Rect buttonRect)
        {
            switch (Event.current.type)
            {
                case EventType.Repaint:
                    GUI.RepeatButton(buttonRect, "Button ");
                    break;
                case EventType.MouseDrag:
                    buttonRect.x += Event.current.delta.x;
                    buttonRect.y += Event.current.delta.y;
                    break;
                case EventType.MouseUp:
                    GUIUtility.hotControl = 0;
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    Event.current.Use();
                    break;
            }
        }
    }
}
