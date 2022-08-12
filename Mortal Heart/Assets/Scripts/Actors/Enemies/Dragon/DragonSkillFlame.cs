using System;
using System.Collections;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class DragonSkillFlame : EnemySkillRange
{
    protected override void Shoot()
    {
        var b = SimplePool.Spawn(bullet.gameObject, shootPos.position, Quaternion.identity);
        var comp = b.GetComponent<BaseBulletBehaviour>();
        comp.Init(damage);
        var dir = shootPos.forward;
        dir.y = 0f;
        comp.Shoot(dir);
    }
}