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

            MoveOnDrag();

            if (GetNodes.Count > 0)
            {
                for (int i = 0; i < GetNodes.Count; i++)
                {
                    GetNodes[i].position = new Vector3(rect.x - 10, rect.y + 5 + (+15 * i), 0);
                    DrawNodeLine(GetNodes[i]);
                    GUI.color = GetNodes[i].color;
                    if (GUI.Button(new Rect(rect.x - 10, rect.y + 5 + (+15 * i), 10, 10), ""))
                    {
                        Debug.Log(i + "s Get node Pressed!!!");
                        GetNodes[i].clicked = true;
                        GetNodes[i].color = Color.green;

                    }
                }
            }

            if (GiveNodes.Count > 0)
            {
                
                for (int i = 0; i < GiveNodes.Count; i++)
                {
                    GiveNodes[i].position = new Vector3(rect.x + rect.width, rect.y + 5 + (15 * i),0);
                    DrawNodeLine(GiveNodes[i]);
                    GUI.color = GiveNodes[i].color;

                    if (GUI.Button(new Rect(rect.x + rect.width, rect.y + 5 + (15 * i), 10, 10), ""))
                    {
                        Debug.Log(i + "s Give node Pressed!!!");
                        GiveNodes[i].clicked = true;
                        GiveNodes[i].color = Color.green;
                    }
                }
            }
        }

        private void MoveOnDrag()
        {
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

            if (isDragging)
            {
                Vector2 dragDelta = Event.current.mousePosition - position;
                //rect = new Rect(rect.x + dragDelta.x,);
                rect.x += dragDelta.x;
                rect.y += dragDelta.y;
            }
        }

        private void DrawNodeLine(Node node)
        {
            if(node.clicked)
            {
                DrawLine(node.position, Event.current.mousePosition, Color.yellow, 1);
            }
        }

        private void DrawLine(Vector3 start, Vector3 end, Color color, float width)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();

            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            GUIUtility.RotateAroundPivot(angle, start);
            GUI.DrawTexture(new Rect(start.x, start.y, (end - start).magnitude, width), texture);
            GUIUtility.RotateAroundPivot(-angle, start);
        }
    }
}
