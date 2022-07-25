using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public int defaultLevel;
    public int maxLevel;
    public int level;
    public UpgradeData[] requirements;
    public Sprite icon;
    public string description;
    public int[] prices;
    public float[] buffvalues;

    public void ResetData()
    {
        level = defaultLevel;
    }

    public int GetNextLevelPrice()
    {
        if (level < maxLevel)
            return prices[level];
        else
            return 0;
    }

    public void UpgradeNextLevel()
    {
        level++;

        if (level > maxLevel)
        {
            level = maxLevel;
            Debug.LogError("Upgrade Exceed Max Level !!!");
        }
    }

    public void UpgradeLevel(int newLevel)
    {
        level = newLevel;
    }

    public float GetCurrentBuffValue()
    {
        if (level <= 0) return 0;

        return buffvalues[level - 1];
    }
}
