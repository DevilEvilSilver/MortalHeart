using UnityEngine;

public interface IDamage
{
    float Damage(Transform collider, float damage);
    float DamageOnPosition(Vector3 position, float damage);
}
