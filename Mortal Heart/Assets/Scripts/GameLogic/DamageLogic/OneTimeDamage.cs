using UnityEngine;

public class OneTimeDamage : IDamage
{
    public float Damage(Transform collider, float damage)
    {
        IHeath health;
        if (collider == null || (health = collider.GetComponent<IHeath>()) == null)
            return -1f;

        health.TakeDamage(damage);
        return damage;
    }

    public float DamageOnPosition(Vector3 position, float damage)
    {
        return -1f;
    }
}