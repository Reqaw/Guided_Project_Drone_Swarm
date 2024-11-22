using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }


    public void LTSearchAndDestroy()
    {
        SceneManager.LoadSceneAsync("LLScene");
    }

    public void BTSearchAndDestroy()
    {
        SceneManager.LoadSceneAsync("BTScene");
    }

    public void MainMenuLoad()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void GraphNetwork()
    {
        SceneManager.LoadSceneAsync("GraphNetworkScene");
    }

    public void FeatureUILoad()
    {
        SceneManager.LoadSceneAsync("FeaturesScene");
    }

    public void Exitbtn()
    {
        Application.Quit();
    }

}
