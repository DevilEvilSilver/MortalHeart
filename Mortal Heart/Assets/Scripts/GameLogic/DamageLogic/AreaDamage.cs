using UnityEngine;

public class AreaDamage : IDamage
{
    public float radius;
    public LayerMask maskLayer;

    private Collider[] _colliders;

    public float Damage(Transform collider, float damage)
    {
        return DamageOnPosition(collider.position, damage);
    }

    public float DamageOnPosition(Vector3 position, float damage)
    {
        if (_colliders == null)
            _colliders = new Collider[50];
        int count = Physics.OverlapSphereNonAlloc(position, radius , _colliders, maskLayer);
        for (var index = 0; index < count; index++)
        {
            var item = _colliders[index];
            var health = item.gameObject.GetComponent<IHeath>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        return damage;
    }
}