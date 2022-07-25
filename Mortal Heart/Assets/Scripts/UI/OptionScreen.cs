using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionScreen : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public TMP_Text resolution;
    public Toggle fullscreen;

    public Button applyBtn;

    private float _originalBGMVolume, _originalSFXVolume;
    private string _originalResolution;
    private bool _isFullScreen;

    private void OnEnable()
    {
        AudioManager.Instance.LoadConfigData();
        GraphicManager.Instance.LoadConfigData();

        _originalBGMVolume = AudioManager.Instance.configData.bgmVolume;
        _originalSFXVolume = AudioManager.Instance.configData.sfxVolume;
        _originalResolution = GraphicManager.Instance.CurrentResolution;
        _isFullScreen = GraphicManager.Instance.IsFullScreen;

        bgmSlider.value = _originalBGMVolume;
        sfxSlider.value = _originalSFXVolume;
        resolution.text = _originalResolution;
        fullscreen.isOn = _isFullScreen;

        applyBtn.interactable = false;
        bgmSlider.onValueChanged.AddListener(value => OnValueChange());
        sfxSlider.onValueChanged.AddListener(value => OnValueChange());
        fullscreen.onValueChanged.AddListener(value => OnValueChange());
    }

    public void OnValueChange()
    {
        if (bgmSlider.value == _originalBGMVolume
            && sfxSlider.value == _originalSFXVolume
            && resolution.text == _originalResolution
            && fullscreen.isOn == _isFullScreen)
                applyBtn.interactable = false;
        else
            applyBtn.interactable = true;
    }

    public void ApplyConfig()
    {
        AudioManager.Instance.ApplyConfig((int)bgmSlider.value, (int)sfxSlider.value);
        GraphicManager.Instance.ApplyConfig(fullscreen.isOn);

        applyBtn.interactable = false;
    }

    public void OnLeftResolution()
    {
        var text = GraphicManager.Instance.ChangeResolutionByIndex(-1);
        if (text != null)
        {
            resolution.text = text;
            OnValueChange();
        }
    }

    public void OnRightResolution()
    {
        var text = GraphicManager.Instance.ChangeResolutionByIndex(1);
        if (text != null)
        {
            resolution.text = text;
            OnValueChange();
        }
    }
}
