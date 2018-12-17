using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenDjikstra()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenAStar()
    {
        SceneManager.LoadScene(2);
    }
}
