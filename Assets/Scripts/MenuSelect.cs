using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{
    public void menuSelect()
    {
        switch(this.gameObject.name)
        {
            case ("Start"):
                SceneManager.LoadScene("Cutscene2");
                break;
            case ("Option"):
                SceneManager.LoadScene("Settings");
                break;
            case ("Menu"):
                SceneManager.LoadScene("Main Menu");
                break;
            case ("Quit"):
                Debug.Log("exit");
                Application.Quit();
                break;
        }
    }
}
