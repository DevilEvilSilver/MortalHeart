using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GraphicManager : SingletonMonoBehaviour<GraphicManager>
{
    private Resolution[] resolutions;

    public ConfigData configData;

    private int _currentResolutionIndex = -1;
    public bool IsFullScreen { get; private set; }
    public string CurrentResolution
    {
        get
        {
            if (_currentResolutionIndex >= 0 && _currentResolutionIndex < resolutions.Length)
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

        resolutions = Screen.resolutions;
        LoadConfigData();
    }

    public void LoadConfigData()
    {
        var x = configData.resolution.width;
        var y = configData.resolution.height;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == x && resolutions[i].height == y)
                _currentResolutionIndex = i;
        }
        if (_currentResolutionIndex < 0 || _currentResolutionIndex >= resolutions.Length)
            _currentResolutionIndex = resolutions.Length - 1;

        IsFullScreen = configData.isFullscreen;
    }

    public string ChangeResolutionByIndex(int change)
    {
        var newIndex = _currentResolutionIndex + change;
        if (newIndex >= 0 && newIndex < resolutions.Length)
        {
            _currentResolutionIndex = newIndex;
            return CurrentResolution;
        }
        return null;
    }

    public void ApplyConfig(bool isFullscreen)
    {
        if (_currentResolutionIndex >= 0 && _currentResolutionIndex < resolutions.Length)
        {
            IsFullScreen = isFullscreen;
            Screen.SetResolution(resolutions[_currentResolutionIndex].width
                , resolutions[_currentResolutionIndex].height
                , isFullscreen);

            configData.SaveGraphic(resolutions[_currentResolutionIndex], isFullscreen);
        }
    }
}
