using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public int defaultLevel = 0;
    public int maxLevel = 1;
    public int level = 0;
    public UpgradeData[] requirements;
    public Sprite icon;
    public string description;
    public int[] prices;

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
    }

    public void UpgradeLevel(int newLevel)
    {
        level = newLevel;
    }
}
