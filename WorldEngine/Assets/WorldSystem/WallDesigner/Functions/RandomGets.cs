using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class RandomGets : FunctionItem, IFunctionItem
{
    public RandomGets()
    {
        Init();
        Name = "Random Get";
        ClassName = typeof(RandomGets).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);

    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
        {
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];
        }

        if (item.givenodeConnectedFI.Count > 0)
        {
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];
        }
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        //item.attributeName.Add("FloatAttrebute");
        //item.attributeName.Add("FloatAttrebute");

        /*FloatAttrebute att1 = (FloatAttrebute)attrebutes[0];
        string stringfloat1 = att1.GetValue().ToString();
        item.attributeValue.Add(stringfloat1);

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[1];
        string stringfloat2 = att2.GetValue().ToString();
        item.attributeValue.Add(stringfloat2);*/

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

        return item;
    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        /*FloatAttrebute att = (FloatAttrebute)attrebutes[0];
        att.mFloat = float.Parse(item.attributeValue[0]);
        attrebutes[0] = att;

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[1];
        att2.mFloat = float.Parse(item.attributeValue[1]);
        attrebutes[1] = att2;*/
    }

    protected override void FIUpdate()
    {
        if (GetNodes[GetNodes.Count-1].ConnectedNode != null)
        {
            GetNode gnode = new GetNode();
            gnode.AttachedFunctionItem = this;
            gnode.id = GetNodes.Count - 1;
            GetNodes.Add(gnode);

            CalculateRect();
            UpdateBoardPos();
        }
    }
     
    public object Execute(object mMesh, object id)
    {
        List<WallPartItem> output = new List<WallPartItem>();
        List<List<WallPartItem>> tempList = new List<List<WallPartItem>>();
        for(int i = 0; i < GetNodes.Count;i++)
        {
            if (GetNodes[i].ConnectedNode != null)
            {
                List<WallPartItem> item = (List<WallPartItem>)GetNodes[i].ConnectedNode.AttachedFunctionItem.myFunction(output, GetNodes[i].ConnectedNode.id);
                tempList.Add(item);
            }
        }

        if (tempList.Count <= 0)
            return output;

        int rand = (int)(Mathf.Round(Random.value * (GetNodes.Count-2)));
        if (GetNodes[rand].ConnectedNode != null)
        {
            output = (List<WallPartItem>)GetNodes[rand].ConnectedNode.AttachedFunctionItem.myFunction(output, GetNodes[rand].ConnectedNode.id);
        }

        return output;
    }
}
