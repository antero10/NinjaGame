using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    private Dictionary<string, Menu> MenuList = new Dictionary<string, Menu>();

    public void addMenu(Menu _menu)
    {
        if (MenuList.ContainsKey(_menu.menuClassifier.name))
        {
            Debug.Log("Menu already Exists: " + _menu.menuClassifier.name);
            return;
        }

        MenuList.Add(_menu.menuClassifier.name, _menu);
    }

    public T getMenu<T>(MenuClassifier menuClassifier) where T : Menu
    {
        if (MenuList.ContainsKey(menuClassifier.name))
        {
            return (T)MenuList[menuClassifier.name];
        }
        return null;
    }

    public void showMenu(MenuClassifier menuClassifier)
    {
        if (MenuList.ContainsKey(menuClassifier.name))
        {
            MenuList[menuClassifier.name].onShowMenu();
        }
        else
        {
            Debug.Log("Unable to show menu: " + menuClassifier.name);
        }
    }

    public void hideMenu(MenuClassifier menuClassifier)
    {
        if (MenuList.ContainsKey(menuClassifier.name))
        {
            MenuList[menuClassifier.name].onHideMenu();
        }
        else
        {
            Debug.Log("Unable to hide menu: " + menuClassifier.name);
        }
    }
}
