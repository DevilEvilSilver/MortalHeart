using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    public float timeOut = 0.5f;
    public TYPE_DESTROY typeDestroy = TYPE_DESTROY.DISABLE;

    float timeStart;

    private void OnEnable()
    {
        timeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeStart > timeOut)
            OnDestroy();
    }

    public void OnDestroy()
    {
        if (typeDestroy == TYPE_DESTROY.DISABLE)
            gameObject.SetActive(false);
        else if (typeDestroy == TYPE_DESTROY.DESPAWN)
            SimplePool.Despawn(gameObject);
        else if (typeDestroy == TYPE_DESTROY.DESTROY)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        OnDestroy();
    }
}

public enum TYPE_DESTROY
{
    DISABLE,
    DESPAWN,
    DESTROY
}