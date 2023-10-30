using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace WallDesigner
{
    [System.Serializable]
    public abstract partial class FunctionItem
    {
        private bool isDragging = false;
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        [NonSerialized]
        public Color basecolor;
        [NonSerialized]
        public Vector2 position;
        //public Action<Mesh> action { get; set; }
        public Func<object, object, object> myFunction { get; set; }
        [NonSerialized]
        public Rect rect;
        [XmlArray("GetNodes"), XmlArrayItem("GetNode")]
        public List<Node> GetNodes;
        [XmlArray("GiveNodes"), XmlArrayItem("GiveNode")]
        public List<Node> GiveNodes;
        public List<Attrebute> attrebutes;
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
            attrebutes = new List<Attrebute>();
        }

        public virtual SerializedFunctionItem SaveSerialize()
        {
            SerializedFunctionItem item = new SerializedFunctionItem();

            item.name = Name;
            item.ClassName = ClassName;

            return item;
        }

        public virtual void LoadSerializedAttributes(SerializedFunctionItem item)
        {
            Name = item.name;
            ClassName = item.ClassName;
        }

        public virtual void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
        {

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

        public void Remove()
        {
            ConnectLineController.Instance.isLineInDraw = false;
            ConnectLineController.Instance.SetInDragNode(null);

            foreach (Node node in GetNodes)
            {
                if(node.ConnectedNode != null)
                {
                    node.ConnectedNode.ConnectedNode = null;
                    node.ConnectedNode = null;
                }
            }

            foreach (Node node in GiveNodes)
            {
                if (node.ConnectedNode != null)
                {
                    node.ConnectedNode.ConnectedNode = null;
                    node.ConnectedNode = null;
                }
            }

            WallEditorController.Instance.canShowMenu = false;
            WallEditorController.Instance.GetAllCreatedItems().Remove(this);
        }

        private void DrawAttrebutes()
        {
            if (attrebutes.Count > 0)
            {
                for (int i = 0; i < attrebutes.Count; i++)
                {
                    attrebutes[i].Draw(position + BoardController.Instance.boardPosition);
                }
            }
        }

        public void UpdateBoardPos()
        {
            Vector2 tempposition = position + BoardController.Instance.boardPosition;
            rect = new Rect(tempposition.x - rect.width * 0.5f, tempposition.y - rect.height * 0.5f, rect.width, rect.height);
        }

        private void DrawAndDrag()
        {

            if (GUI.RepeatButton(rect, Name))
            {
                if (Event.current.button == 1)
                    Remove();
                else
                {
                    isDragging = true;
                    WEInputManager.Instance.isItemOnDrag = true;
                    position = Event.current.mousePosition - BoardController.Instance.boardPosition;
                    rect = new Rect(Event.current.mousePosition.x - rect.width * 0.5f, Event.current.mousePosition.y - rect.height * 0.5f, rect.width, rect.height);
                    WallEditorController.Instance.RepaintBoard();
                }
            }
            else if (isDragging)// && Event.current.type == EventType.MouseUp)
            {
                isDragging = false;
                WEInputManager.Instance.isItemOnDrag = false;
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