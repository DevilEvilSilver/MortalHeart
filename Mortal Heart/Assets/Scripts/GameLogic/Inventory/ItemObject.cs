using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class ItemObject : SerializedMonoBehaviour
{
    [SerializeField] private InventoryItemData data;
    [SerializeField] private LayerMask playerLayer;

    public virtual void OnTriggerEnter(Collider coll)
    {
        if ((playerLayer.value & (1 << coll.gameObject.layer)) == 0) return;

        InventorySystem.Instance.Add(data);
        SimplePool.Despawn(gameObject);
    }
}
