using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    public GameObject chooseSaveFileScreen;
    public GameObject optionScreen;
    //public GameObject NewDataNameScreen;

    public SaveFileUI[] saveFiles;

    public void OnStart()
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

    public void OnQuit()
    {
        Application.Quit();
    }

}
