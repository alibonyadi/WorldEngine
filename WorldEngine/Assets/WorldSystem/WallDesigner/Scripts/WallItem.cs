using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class WallItem
    {
        public List<WallPartItem> wallPartItems = new List<WallPartItem>();
        public BuildingDirection buildingDirection = new BuildingDirection();
        public GameObject Caller;// = new GameObject();
        public bool isInEditMode = true;
    }
}
