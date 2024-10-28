using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void Startbtn()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void Exitbtn()
    {
        Application.Quit();
    }
}
