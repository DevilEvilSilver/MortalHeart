using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "ConfigData", menuName = "ScriptableObjects/ConfigData")]
public class ConfigData : ScriptableObject
{
    [Header("Audio")]
    public int bgmVolume = 5;
    public int sfxVolume = 5;
    [Header("Graphic")]
    public Resolution resolution;
    public bool isFullscreen;

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
}