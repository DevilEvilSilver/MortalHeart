using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData")]
public class SaveData : ScriptableObject
{
    public bool isNewSaveData = true;
    public float baseMaxHealth = 100f;
    public float baseSpeed = 5f;

    public float playTime = 0f;
    public int money = 0;
    public int enemyKilled = 0;

    [TableList]
    public List<UpgradeSaveData> upgradesLevel;

    public void LoadData()
    {
        foreach (var save in upgradesLevel)
        {
            save.upgrade.level = save.level;
        }
    }

    public void ResetData()
    {
        isNewSaveData = true;
        baseMaxHealth = 100f;
        baseSpeed = 5f;

        playTime = 0f;
        money = 0;
        enemyKilled = 0;

        foreach (var save in upgradesLevel)
        {
            save.upgrade.ResetData();
            save.level = save.upgrade.defaultLevel;
        }
    }

    public void SaveUpgrade(UpgradeData upgrade)
    {
        foreach (var save in upgradesLevel)
        {
            if (save.upgrade == upgrade)
                save.level = save.upgrade.level;
        }
    }
}

[Serializable]
public class UpgradeSaveData
{
    public UpgradeData upgrade;
    public int level;
}