using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GraphicManager : SingletonMonoBehaviour<GraphicManager>
{
    private List<Resolution> resolutions;

    public ConfigData configData;

    private int _currentResolutionIndex = -1;
    public bool IsFullScreen { get; private set; }
    public string CurrentResolution
    {
        get
        {
            if (_currentResolutionIndex >= 0 && _currentResolutionIndex < resolutions.Count)
            {
                var res = resolutions[_currentResolutionIndex];
                return (res.width + "x" + res.height);
            }
            return null;
        }
    }

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);

        resolutions = new List<Resolution>();
        var allResolutions = Screen.resolutions;
        foreach (var reso in allResolutions)
        {
            if (resolutions.Count <= 0 
                || resolutions[resolutions.Count - 1].width != reso.width
                || resolutions[resolutions.Count - 1].height != reso.height)
            {
                resolutions.Add(reso);
            }
        }
        LoadConfigData();
    }

    public void LoadConfigData()
    {
        var x = configData.resolutionWidth;
        var y = configData.resolutionHeight;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (resolutions[i].width == x && resolutions[i].height == y)
                _currentResolutionIndex = i;
        }
        if (_currentResolutionIndex < 0 || _currentResolutionIndex >= resolutions.Count)
            _currentResolutionIndex = resolutions.Count - 1;

        IsFullScreen = configData.isFullscreen;
    }

    public string ChangeResolutionByIndex(int change)
    {
        var newIndex = _currentResolutionIndex + change;
        if (newIndex >= 0 && newIndex < resolutions.Count)
        {
            _currentResolutionIndex = newIndex;
            return CurrentResolution;
        }
        return null;
    }

    public void ApplyConfig(bool isFullscreen)
    {
        if (_currentResolutionIndex >= 0 && _currentResolutionIndex < resolutions.Count)
        {
            IsFullScreen = isFullscreen;
            Screen.SetResolution(resolutions[_currentResolutionIndex].width
                , resolutions[_currentResolutionIndex].height
                , isFullscreen);

            configData.SaveGraphic(resolutions[_currentResolutionIndex], isFullscreen);
        }
    }
}
