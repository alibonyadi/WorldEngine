using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class PropertyManager
{
    private List<Property> properties;
    private static PropertyManager instance;
    private PropertyManager() 
    {
        properties = new List<Property>();
    }

    public static PropertyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PropertyManager();
            }
            return instance;
        }
    }

    public void AddProperty(Property property)
    {
        properties.Add(property);
    }
}
