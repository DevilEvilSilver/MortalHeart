using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using UniRx;

public class Sparkling : MonoBehaviour
{
    [SerializeField] protected BaseBulletBehaviour bullet;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float damage;

    private Transform _player;
    private Vector3 _offset;
    private Transform _target;
    private float _timeSinceLastShoot;

    private void OnEnable()
    {
        _timeSinceLastShoot = Time.time;
        _player = GameObject.FindGameObjectWithTag(GameUtils.TagManager.TAG_PLAYER)?.transform;
        _offset = new Vector3(1f, 1f, 1f);
        _target = FindClosestEnemy().transform;
    }

    private GameObject FindClosestEnemy()
    {
        var lsTargets = GameObject.FindGameObjectsWithTag(GameUtils.TagManager.TAG_ENEMY);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        if (lsTargets != null && lsTargets.Length > 0)
        {
            if (lsTargets.Length > 1)
            {
                foreach (GameObject go in lsTargets)
                {
                    if (go != null)
                    {
                        Vector3 diff = go.transform.position - position;
                        float curDistance = diff.sqrMagnitude;
                        if (curDistance < distance)
                        {
                            closest = go;
                            distance = curDistance;
                        }
                    }
                }
            }
            else
            {
                closest = lsTargets[0];
            }
        }
        return closest;
    }

    private void Update()
    {
        if (Time.time > _timeSinceLastShoot + fireRate)
        {
            if (_target != null || !_target.gameObject.activeInHierarchy)
            {
                var b = SimplePool.Spawn(bullet.gameObject, transform.position, Quaternion.identity);
                var comp = b.GetComponent<BaseBulletBehaviour>();
                comp.Init(damage);
                comp.Shoot(_target.position - transform.position);
                _timeSinceLastShoot = Time.time;
            }
            else
                _target = FindClosestEnemy().transform;
        }
        Follow();
    }

    private void Follow()
    {
        transform.position = _player.position + _offset;
    }
}
