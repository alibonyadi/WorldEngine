using Assets.WorldSystem.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    abstract public class FunctionItem : MonoBehaviour
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Vector2 position { get; set; }

        public Action action { get; set; }

        public Rect rect { get; set; }

        public List<INode> GetNodes;
        public List<INode> GiveNodes;

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
        }


        public string GetName() => Name;
        public Action GetAction() => action;


        private void CalculateRect()
        {
            int maxNode = Mathf.Max(GetNodes.Count, GiveNodes.Count);

            float heigh = maxNode * 10 + 25;
            float width = 150;
            rect = new Rect(0,0,width,heigh);
        }

        public void Draw()
        {
            GUI.Box(rect, Name);
            GUI.color = Color.red;
            for(int i=0;i< GetNodes.Count;i++)
            {
                GUI.Box(new Rect(rect.x - 10, rect.y - 5 + (- 15 * i),10,10), "");
            }

            for(int i=0; i< GiveNodes.Count;i++)
            {
                GUI.Box(new Rect(rect.x+rect.width, rect.y - 5 + (-15 * i), 10, 10), "");
            }
        }
    }
}
