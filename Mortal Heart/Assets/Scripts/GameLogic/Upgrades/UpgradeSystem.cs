using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class UpgradeSystem : SingletonMonoBehaviour<UpgradeSystem>
{
    [SerializeField] private UpgradeData healthUpgrade;
    [SerializeField] private UpgradeData sppedUpgrade;
    [SerializeField] private UpgradeData attackUpgrade;
    [SerializeField] private UpgradeData goldEarnUpgrade;

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public bool IsSkillUpgradeable(UpgradeData data)
    {
        bool isUpgradeAble = true;
        if (data.level <= 0)
        {
            foreach (var require in data.requirements)
            {
                if (require.level <= 0)
                    isUpgradeAble = false;
            }
        }
        else if (data.level >= data.maxLevel)
        {
            isUpgradeAble = false;
        }

        if (isUpgradeAble && GameController.Instance.currSaveData.experience > data.GetNextLevelPrice())
            isUpgradeAble = true;
        else
            isUpgradeAble = false;

        return isUpgradeAble;
    }

    public void UpgradeSkill(UpgradeData data)
    {
        GameController.Instance.currSaveData.experience -= data.GetNextLevelPrice();
        data.UpgradeNextLevel();
        GameController.Instance.currSaveData.SaveUpgrade(data);
    }

    public float GetBaseHealth()
    {
        return healthUpgrade.GetCurrentBuffValue();
    }

    public float GetBaseSpeed()
    {
        return sppedUpgrade.GetCurrentBuffValue();
    }

    public float GetBonusAttack()
    {
        return attackUpgrade.GetCurrentBuffValue();
    }

    public float GetBonusGold()
    {
        return goldEarnUpgrade.GetCurrentBuffValue();
    }
}
