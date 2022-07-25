using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using System.IO;

[CreateAssetMenu(fileName = "ConfigData", menuName = "ScriptableObjects/ConfigData")]
public class ConfigData : ScriptableObject
{
    [Header("Audio")]
    public int bgmVolume;
    public int sfxVolume;
    [Header("Graphic")]
    public Resolution resolution;
    public bool isFullscreen;

    private void Awake()
    {
        Debug.Log("init config data !!!");
        LoadFromFile();
    }

    public void SaveToFile()
    {
        var jsonString = JsonUtility.ToJson(this, true);
        File.WriteAllText(GetPath(), jsonString);
    }

    public void LoadFromFile()
    {
        string jsonString;
        if (File.Exists(GetPath()))
        {
            jsonString = File.ReadAllText(GetPath());
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }
        else
        {
            Debug.Log("save file does not exist !");
            ResetData();
            jsonString = JsonUtility.ToJson(this, true);
            File.WriteAllText(GetPath(), jsonString);
        }
    }

    private string GetPath()
    {
        return Application.persistentDataPath + "/" + name + ".json";
    }

    public void SaveVolume(int bgmVolume, int sfxVolume)
    {
        this.bgmVolume = bgmVolume;
        this.sfxVolume = sfxVolume;
    }

    public void SaveGraphic(Resolution resolution, bool isFullscreen)
    {
        this.resolution = resolution;
        this.isFullscreen = isFullscreen;
    }

    public void ResetData()
    {
        bgmVolume = 5;
        sfxVolume = 5;
        var resolutions = Screen.resolutions;
        resolution = resolutions[resolutions.Length - 1];
        isFullscreen = true;
    }

}