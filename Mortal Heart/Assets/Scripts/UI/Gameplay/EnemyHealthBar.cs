using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float height;

    private void Update()
    {
        transform.position = Helpers.GetWorldToScreenPoint(target.position + Vector3.up * height);
    }
}
