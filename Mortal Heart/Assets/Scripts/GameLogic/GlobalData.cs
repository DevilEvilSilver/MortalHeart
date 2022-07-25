using UnityEngine;

public class GlobalData
{
    public static float BonusHealthValue;
    public static float BonusHealthPercent;
    public static float BonusSpeedPercent;
    public static float BonusAttackPercent;
    public static float BonusGoldPercent;

    public static void ReloadData()
    {
        BonusHealthValue = 0;
        BonusHealthPercent = 0;
        BonusSpeedPercent = 0;
        BonusAttackPercent = 0;
        BonusGoldPercent = 0;
    }

    public static float GetMaxHealth()
    {
        return (BonusHealthValue + UpgradeSystem.Instance.GetBaseHealth()) * (1 + BonusHealthPercent);
    }

    public static float GetPlayerSpeed()
    {
        return UpgradeSystem.Instance.GetBaseSpeed() * (1 + BonusSpeedPercent);
    }

    public static float GetBonusAttackPercent()
    {
        return UpgradeSystem.Instance.GetBonusAttack() + BonusAttackPercent;
    }

    public static float GetBonusGoldPercent()
    {
        return UpgradeSystem.Instance.GetBonusGold() + BonusGoldPercent;
    }
}