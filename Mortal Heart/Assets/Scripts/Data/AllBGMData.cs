using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "AllBGMData", menuName = "ScriptableObjects/AllBGMData")]
public class AllBGMData : ScriptableObject
{
    [TableList]
    public BGMClipData[] ItemList;

    public void PlayBGM(string scene)
    {
        foreach (var data in ItemList)
        {
            if (data.scene.Equals(scene))
            {
                AudioManager.Instance.PlayMusic(data.clip);
                return;
            }
        }
    }
}

[Serializable]
public class BGMClipData
{
    public AudioClip clip;
    public string scene;
}
