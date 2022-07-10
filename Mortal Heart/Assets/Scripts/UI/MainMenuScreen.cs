using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    public GameObject chooseSaveFileScreen;
    public GameObject optionScreen;
    //public GameObject NewDataNameScreen;

    public SaveFileUI[] saveFiles;

    public void OnChooseSaveFile()
    {
        chooseSaveFileScreen.SetActive(true);
        foreach (var save in saveFiles)
        {
            save.UpdateData();
        }
    }

    public void OnOptionSelect()
    {
        optionScreen.SetActive(true);
    }

    public void OnQuitSaveFile()
    {
        chooseSaveFileScreen.SetActive(false);
    }

    public void OnOptionQuit()
    {
        optionScreen.SetActive(false);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

}
