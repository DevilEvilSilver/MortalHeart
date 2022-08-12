using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class PlayerHitCollider : HitCollider
{
    [SerializeField] private MainCharacterController owner;

    public override void OnTriggerEnter(Collider coll)
    {
        if ((effectLayer.value & (1 << coll.gameObject.layer)) == 0) return;

        if ((shieldLayer.value & (1 << coll.gameObject.layer)) != 0)
        {
            gameObject.SetActive(false);
            _disposable?.Dispose();
            return;
        }

        var health = coll.GetComponent<IHeath>();
        if (health != null)
        {
           var totalDamge = damageType.Damage(coll.transform, _damage );

            owner?.OnDealDamage(totalDamge);
        }
    }

    private float CalculateDamage()
    {
        return _damage * (1 + GlobalData.GetBonusAttackPercent());
    }
}
