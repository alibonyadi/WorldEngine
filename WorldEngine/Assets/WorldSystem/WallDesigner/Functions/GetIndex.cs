using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class GetIndex : FunctionItem, IFunctionItem
{
    private int Index = 0;

    public GetIndex(int gets, int gives)
    {
        Init();
        Name = "Get Index";
        ClassName = typeof(GetIndex).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        GiveNode givenode2 = new GiveNode();
        givenode2.AttachedFunctionItem = this;
        givenode2.id = 1;
        GiveNodes.Add(givenode2);

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);

        ToggleAttribute ta1 = new ToggleAttribute(at1Rect);
        ta1.mToggle = false;
        ta1.SetName("From Last");
        attrebutes.Add(ta1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y + 20, rect.width, rect.height);

        IntAttrebute fl1 = new IntAttrebute(at2Rect);
        fl1.mInt = Index;
        fl1.SetMinMax(0,15);
        fl1.SetName("Index");
        attrebutes.Add(fl1);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];

        if (item.givenodeConnectedFI.Count > 0)
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];

        if (item.givenodeConnectedFI.Count > 1)
            GiveNodes[1].ConnectedNode = functionItems[item.givenodeConnectedFI[1]].GetNodes[item.givenodeItems[1]];
    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        ta1.mToggle = item.attributeValue[0] == "True";
        attrebutes[0] = ta1;

        IntAttrebute att = (IntAttrebute)attrebutes[1];
        att.mInt = int.Parse(item.attributeValue[1]);
        attrebutes[1] = att;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("Toggle");

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        string stringtoggle = ta1.mToggle.ToString();
        item.attributeValue.Add(stringtoggle);

        IntAttrebute att1 = (IntAttrebute)attrebutes[1];
        string stringint = att1.mInt.ToString();
        item.attributeValue.Add(stringint);

        if (GetNodes[0].ConnectedNode != null)
        {
            int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GetNodes[0].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnectedFI.Add(connectedGetNodeNumber);
            item.getnodeItems.Add(GetNodes[0].ConnectedNode.id);
        }

        if (GiveNodes[0].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[0].ConnectedNode.AttachedFunctionItem);
            item.givenodeConnectedFI.Add(connectedGiveNodeNumber);
            item.givenodeItems.Add(GiveNodes[0].ConnectedNode.id);
        }

        if (GiveNodes[1].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[1].ConnectedNode.AttachedFunctionItem);
            item.givenodeConnectedFI.Add(connectedGiveNodeNumber);
            item.givenodeItems.Add(GiveNodes[1].ConnectedNode.id);
        }
        return item;
    }

    public object Execute(object mMesh, object id)
    {
        IntAttrebute fa1 = (IntAttrebute)attrebutes[1];
        Index = (int)fa1.GetValue();

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        bool fromTop = ta1.mToggle;

        WallItem wpi = (WallItem)mMesh;

        if (GetNodes[0].ConnectedNode != null)
            wpi = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        List<WallPartItem> OtherIndexes = new List<WallPartItem>();
        List<WallPartItem> thisIndex = new List<WallPartItem>();
        int tempIndex = fromTop? wpi.wallPartItems.Count-Index-1:Index;
        if (GetNodes[0].ConnectedNode != null)
        {
            for(int i=0;i<wpi.wallPartItems.Count;i++)
            {
                if(i == tempIndex)
                {
                    thisIndex.Add(wpi.wallPartItems[i]);
                }
                else
                {
                    OtherIndexes.Add(wpi.wallPartItems[i]);
                }
            }

            WallItem output = new WallItem();

            output.wallPartItems = (int)id==0 ? thisIndex : OtherIndexes;
            output.buildingDirection =wpi.buildingDirection;
            return output;
            /*if ((int)id == 0)
            {
                return thisIndex;
            }
            else
            {
                return OtherIndexes;
            }*/
        }
        else
        {
            return wpi;
        }
    }
}
