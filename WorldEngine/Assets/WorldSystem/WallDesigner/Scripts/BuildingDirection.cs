using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class BuildingDirection
    {
        bool hasStreet = false;
        bool hasAlley=false;

        public List<Vector3> streetDirections = new List<Vector3>();
        public List<Vector3> alleyDirections = new List<Vector3>();

        public bool CheckStreet()
        {
            return hasStreet;
        }

        public bool CheckAlley()
        {
            return hasAlley;
        }
    }
}
