using UnityEngine;
using System;
using UniRx;

public class SpeedPotion : InventoryItemData
{
    [SerializeField] private float boostAmount;
    [SerializeField] private float boostTime;

    public override void OnAdd()
    {

    }

    public override void OnUsed()
    {
        GlobalData.BonusSpeedPercent += boostAmount;
        Observable.Timer(TimeSpan.FromSeconds(boostTime)).Subscribe(_ =>
        {
            GlobalData.BonusSpeedPercent -= boostAmount;
        });
    }

    public override void OnRemoved()
    {

    }
}
