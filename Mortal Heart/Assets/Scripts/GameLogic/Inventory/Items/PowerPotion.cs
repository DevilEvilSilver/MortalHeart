using UnityEngine;
using System;
using UniRx;

public class PowerPotion : InventoryItemData
{
    [SerializeField] private float boostAmount;
    [SerializeField] private float boostTime;

    public override void OnAdd()
    {

    }

    public override void OnUsed()
    {
        GlobalData.BonusAttackPercent += boostAmount;
        Observable.Timer(TimeSpan.FromSeconds(boostTime)).Subscribe(_ =>
        {
            GlobalData.BonusAttackPercent -= boostAmount;
        });
    }

    public override void OnRemoved()
    {

    }
}
