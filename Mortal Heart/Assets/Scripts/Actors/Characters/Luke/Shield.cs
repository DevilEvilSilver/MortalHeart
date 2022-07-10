using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class Shield : MonoBehaviour
{
    [SerializeField] private LayerMask effectLayer;

    internal int blockedCount;

    private void OnEnable()
    {
        blockedCount = 0;
    }

    public virtual void OnTriggerEnter(Collider coll)
    {
        if ((effectLayer.value & (1 << coll.gameObject.layer)) == 0) return;

        blockedCount++;
    }
}