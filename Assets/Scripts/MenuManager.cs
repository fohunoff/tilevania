using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
}
