using System;
using System.Collections;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class LichSkill1 : EnemySkillRange
{
    [SerializeField] protected int numWays;
    [SerializeField] protected Vector2 range;

    protected override void Shoot()
    {
        var dir = _player.position - shootPos.position;
        var start = range.x;
        var step = (range.y - range.x) / (numWays - 1);
        for (int i = 0; i < numWays; i++)
        {
            var b = SimplePool.Spawn(bullet.gameObject, shootPos.position, Quaternion.identity);
            var comp = b.GetComponent<BaseBulletBehaviour>();
            comp.Init(damage);
            var angle = start + step * i;
            var vec = Quaternion.AngleAxis(angle, Vector3.up) * dir;
            vec.y = 0;
            comp.Shoot(vec);
        }  
    }
}