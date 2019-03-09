using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private Menu MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        MainMenu = GameObject.Find("Main Menu").GetComponent<Menu>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        MenuManager.Instance.hideMenu(MainMenu.menuClassifier);

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
