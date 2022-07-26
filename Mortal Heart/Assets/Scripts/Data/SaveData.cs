using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using System.IO;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData")]
public class SaveData : ScriptableObject
{
    public bool isNewSaveData;

    public float playTime;
    public int experience;
    public int enemyKilled;

    [TableList]
    public List<UpgradeSaveData> upgradesLevel;

    private void Awake()
    {
        Debug.Log("init save data !!!");
        LoadFromFile();
    }

    [Button]
    public void ShowPath()
    {
        Debug.Log(GetPath());
    }

    private string GetPath()
    {
        return Application.persistentDataPath + "/" + name + ".json";
    }

    public void LoadData()
    {
        foreach (var save in upgradesLevel)
        {
            save.upgrade.level = save.level;
        }
    }

    public void SaveToFile()
    {
        SaveFileFormat format = new SaveFileFormat(isNewSaveData, playTime, experience, enemyKilled
            , upgradesLevel);
        var jsonString = JsonUtility.ToJson(format, true);
        File.WriteAllText(GetPath(), jsonString);
    }

    public void LoadFromFile()
    {
        string jsonString;
        if (File.Exists(GetPath()))
        {
            jsonString = File.ReadAllText(GetPath());
            var format = JsonUtility.FromJson<SaveFileFormat>(jsonString);

            isNewSaveData = format.isNewSaveData;
            playTime = format.playTime;
            experience = format.experience;
            enemyKilled = format.enemyKilled;
            for (int i = 0; i < format.upgradesLevel.Count; i++)
            {
                upgradesLevel[i].level = format.upgradesLevel[i];
            }
        }
        else
        {
            Debug.Log("save file does not exist !");
            ResetData();
            SaveToFile();
        }
    }

    public void ResetData()
    {
        isNewSaveData = true;

        playTime = 0f;
        experience = 0;
        enemyKilled = 0;

        foreach (var save in upgradesLevel)
        {
            save.upgrade.ResetData();
            save.level = save.upgrade.defaultLevel;
        }

        SaveToFile();
    }

    public void SaveUpgrade(UpgradeData upgrade)
    {
        foreach (var save in upgradesLevel)
        {
            if (save.upgrade == upgrade)
                save.level = save.upgrade.level;
        }
        SaveToFile();
    }

    public void SavePlayData(float playTime, int enemyKilled = 0, int money = 0)
    {
        this.playTime += playTime;
        this.enemyKilled += enemyKilled;
        this.experience += money;
        SaveToFile();
    }
}

[Serializable]
public class UpgradeSaveData
{
    public UpgradeData upgrade;
    public int level;
}

public class SaveFileFormat
{
    public bool isNewSaveData;

    public float playTime;
    public int experience;
    public int enemyKilled;

    public List<int> upgradesLevel;
    
    public SaveFileFormat(bool isNewSaveData, float playTime, int experience, int enemyKilled
        , List<UpgradeSaveData> upgradesLevel)
    {
        this.isNewSaveData = isNewSaveData;
        this.playTime = playTime;
        this.experience = experience;
        this.enemyKilled = enemyKilled;
        this.upgradesLevel = new List<int>();
        foreach (var item in upgradesLevel)
        {
            this.upgradesLevel.Add(item.level);
        }
    }
}