using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData")]
public class SaveData : SerializedScriptableObject
{
    public bool isNewSaveData = true;
    public float baseMaxHealth = 100f;
    public float baseSpeed = 5f;

    public float playTime = 0f;
    public int money = 0;
    public int enemyKilled = 0;

    public Dictionary<UpgradeData, int> upgradesLevel;

    public void LoadData()
    {
        foreach (var upgrade in upgradesLevel)
        {
            upgrade.Key.level = upgrade.Value;
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

        foreach (var key in upgradesLevel.Keys.ToList())
        {
            key.ResetData();
            upgradesLevel[key] = key.level;
        }
    }

    public void SaveUpgrade(UpgradeData upgrade)
    {
        if (upgradesLevel.TryGetValue(upgrade, out int value))
        {
            upgradesLevel[upgrade] = upgrade.level;
        }
    }
}
