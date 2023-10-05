using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace WallDesigner
{
    public class RightClickMenu
    {
        List<RCMenuItem> menuItems;
        public RightClickMenu() 
        {
            Update(WallEditorController.Instance);
        }
        public void Update(WallEditorController CTRL) 
        {
            menuItems = new List<RCMenuItem>();
            List<FunctionItem> item = CTRL.GetAllFunctionItems();
            //menuItems.Clear();

            for (int i = 0; i < item.Count; i++)
            {
                RCMenuItem menuItem = new RCMenuItem();
                menuItem.Name = item[i].GetName();
                menuItem.action = CTRL.GetCreateAction();
                menuItems.Add(menuItem);
            }
            Debug.Log("Menu Updated!!");
        }
        public GenericMenu GetAllMenuItems()
        {
            GenericMenu gmenu = new GenericMenu();

            for (int i = 0; i < menuItems.Count; i++)
            {
                //gmenu.AddItem(new GUIContent(menuItems[i].Name), false, menuItems[i].action.Invoke );
                gmenu.AddItem( new GUIContent(menuItems[i].Name), false, menuItems[i].action.Invoke, i);
            }

            return gmenu;
        }
        public void tempfunction(object a)
        {

        }
    }  
}
