using UnityEngine;

public class OneTimeDamage : IDamage
{
    public float Damage(Transform collider, float damage)
    {
        IHeath heath;
        if (collider == null || (heath = collider.GetComponent<IHeath>()) == null)
            return -1f;

        heath.TakeDamage(damage);
        return damage;
    }

    public float DamageOnPosition(Vector3 position, float damage)
    {
        return -1f;
    }
}