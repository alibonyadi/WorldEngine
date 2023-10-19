using UnityEngine;
using WallDesigner;

public class AddColor : FunctionItem, IFunctionItem
{
    private Color outputColor;

    public AddColor()
    {
        Init();
        Name = "Color";
        outputColor = Color.white;
        ClassName = typeof(AddColor).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        GiveNodes.Add((Node)node);
        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        RandomColorAttrebute colorAtt1 = new RandomColorAttrebute(at1Rect);
        colorAtt1.mColor = Random.ColorHSV();
        colorAtt1.SetName("Random Color");
        attrebutes.Add(colorAtt1);
    }

    public object Execute(object color)
    {
        return outputColor;
    }
}
