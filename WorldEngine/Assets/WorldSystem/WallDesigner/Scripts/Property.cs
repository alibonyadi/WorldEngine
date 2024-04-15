using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace WallDesigner
{
    public class Property
    {
        public Attrebute attrebute;

        public List<GetPropertyNode> GetNodes;
        public List<GivePropertyNode> GiveNodes;
        public Rect rect = new Rect();

        public Property(Rect r) 
        {
            rect = r;
            GetNodes = new List<GetPropertyNode>();
            GiveNodes = new List<GivePropertyNode>();
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

        public SerializedProperty SaveNodes(int index)
        {
            SerializedProperty allNodes= new SerializedProperty();
            allNodes.getnodeCount = GetNodes.Count;
            allNodes.givennodeCount = GiveNodes.Count;
            for (int i=0;i<GetNodes.Count;i++)
            {
                //SerializedProperty serializedProperty = new SerializedProperty();
                if (GetNodes[i].ConnectedNode!=null)
                {
                    allNodes.hasGetConnected.Add(true);
                    GivePropertyNode gpn = (GivePropertyNode)GetNodes[i].ConnectedNode;
                    int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(gpn.AttachedProperty.attrebute.GetFunctionItem());
                    allNodes.getnodeConnectedFI.Add(connectedGetNodeNumber);
                    allNodes.getnodeConnectedAttrebute.Add(gpn.AttachedProperty.attrebute.GetFunctionItem().GetAttrebuteIndex(gpn.AttachedProperty.attrebute));
                    allNodes.getnodeItems.Add(GetNodes[i].ConnectedNode.id);
                }
                else//create Fake
                {
                    allNodes.hasGetConnected.Add(false);
                    int connectedGetNodeNumber = 0;
                    allNodes.getnodeConnectedFI.Add(connectedGetNodeNumber);
                    allNodes.getnodeConnectedAttrebute.Add(index);
                    allNodes.getnodeItems.Add(0);
                }
            }

            for(int i=0;i<GiveNodes.Count;i++)
            {
                if (GiveNodes[i].ConnectedNode != null)
                {
                    allNodes.hasGiveConnected.Add(true);
                    GetPropertyNode gpn = (GetPropertyNode)GiveNodes[i].ConnectedNode;
                    int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(gpn.AttachedProperty.attrebute.GetFunctionItem());
                    allNodes.givenodeConnectedFI.Add(connectedGiveNodeNumber);
                    allNodes.givenodeConnectedAttrebute.Add(gpn.AttachedProperty.attrebute.GetFunctionItem().GetAttrebuteIndex(gpn.AttachedProperty.attrebute));
                    allNodes.givenodeItems.Add(GiveNodes[i].ConnectedNode.id);
                }
                else // Create Fake Give
                {
                    allNodes.hasGiveConnected.Add(false);
                    int connectedGiveNodeNumber = 0;
                    allNodes.givenodeConnectedFI.Add(connectedGiveNodeNumber);
                    allNodes.givenodeConnectedAttrebute.Add(index);
                    allNodes.givenodeItems.Add(0);
                }
            }
            return allNodes;
        }

        public void LoadNodes(SerializedProperty item,List<FunctionItem> functionItems)
        {
            for (int i = 0; i < item.getnodeCount; i++)
            {
                //Debug.Log(attrebute.GetFunctionItem().Name+" -- att name = " + attrebute +" -- get node "+i);
                if (item.hasGetConnected[i])
                {
                    //Debug.Log("FI = "+item.getnodeConnectedFI[i]+" -- function Item = "+ functionItems[item.getnodeConnectedFI[i]]);
                    //Debug.Log("att = "+ item.getnodeConnectedAttrebute[i]+" -- attrebute ="+ functionItems[item.getnodeConnectedFI[i]].attrebutes[item.getnodeConnectedAttrebute[i]]);
                    //Debug.Log(" property = " + functionItems[item.getnodeConnectedFI[i]].attrebutes[item.getnodeConnectedAttrebute[i]].GetProperty());
                    //Debug.Log("node = "+ item.getnodeItems[i]);

                    GetNodes[i].ConnectedNode = functionItems[item.getnodeConnectedFI[i]].attrebutes[item.getnodeConnectedAttrebute[i]].GetProperty().GiveNodes[item.getnodeItems[i]];
                }
            }

            for(int i=0;i<item.givennodeCount;i++)
            {
                //Debug.Log(attrebute.GetFunctionItem().Name+" -- give node "+i);
                if (item.hasGiveConnected[i])
                    GiveNodes[i].ConnectedNode = functionItems[item.givenodeConnectedFI[i]].attrebutes[item.givenodeConnectedAttrebute[i]].GetProperty().GetNodes[item.givenodeItems[i]];
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

        public virtual Attrebute Execute()
        {
            Attrebute item = attrebute;
            if (GetNodes[0].ConnectedNode != null)
            {
                //Debug.Log(GetNodes[0].ConnectedNode.at)
                GivePropertyNode givePropertyNode = (GivePropertyNode)GetNodes[0].ConnectedNode;
                try
                {
                    item = (Attrebute)givePropertyNode.AttachedProperty.Execute();
                }
                catch
                {
                    Debug.LogWarning("Attribute Type Missmatch!!!");
                }
            }
            return item;
        }
    }
}