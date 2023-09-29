using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace WallDesigner
{
    public class RightClickMenu
    {
        List<RCMenuItem> menuItems;
        public RightClickMenu(List<FunctionItem> item) 
        {
            Update(item);
        }

        public void Update(List<FunctionItem> item) 
        {
            menuItems = new List<RCMenuItem>();
            //menuItems.Clear();

            for (int i = 0; i < item.Count; i++)
            {
                RCMenuItem menuItem = new RCMenuItem();
                menuItem.Name = item[i].GetName();
                menuItem.action = item[i].GetAction();
                menuItems.Add(menuItem);
            }
        }


        public GenericMenu GetAllMenuItems()
        {
            GenericMenu gmenu = new GenericMenu();

            for (int i = 0; i < menuItems.Count; i++)
            {
                gmenu.AddItem(new GUIContent(menuItems[i].Name), false, menuItems[i].action.Invoke );
            }

            return gmenu;
        }
    }

    
}
