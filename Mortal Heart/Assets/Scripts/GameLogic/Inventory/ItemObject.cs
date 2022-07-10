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

        Debug.Log("In Item Pick Up Range !!!");
        //InventorySystem.Instance.Add(data);
        //SimplePool.Despawn(gameObject);
    }

    public bool PickUpObject()
    {
        if (InventorySystem.Instance.Add(data))
        {
            SimplePool.Despawn(gameObject);
            return true;
        }
        return false;
    }
}
