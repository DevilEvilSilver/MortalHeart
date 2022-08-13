using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    private const int MIN_VOLUME = 0;
    private const int MAX_VOLUME = 10;

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public ConfigData configData;
    public float delayTime = 0.5f;

    private AudioClip _currentMusic;

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);

        LoadConfigData();
    }

    public void LoadConfigData()
    {
        bgmSource.volume = (float)configData.bgmVolume / MAX_VOLUME;
        sfxSource.volume = (float)configData.sfxVolume / MAX_VOLUME;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || _currentMusic == clip)
            return;

        _currentMusic = clip;
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.PlayDelayed(delayTime);
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void ApplyConfig(int bgmVolume, int sfxVolume)
    {
        if (bgmVolume < 0 || bgmVolume > 10
            || sfxVolume < 0 || sfxVolume > 10)
            return;

        bgmSource.volume = (float)bgmVolume / MAX_VOLUME;
        sfxSource.volume = (float)sfxVolume / MAX_VOLUME;

        configData.SaveVolume(bgmVolume, sfxVolume);
    }
}
