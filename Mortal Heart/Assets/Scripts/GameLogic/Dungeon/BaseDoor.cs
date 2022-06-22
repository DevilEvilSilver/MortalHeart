using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float openPos;
    [SerializeField] private float closePos;

    protected BaseRoom _room;
    private bool _isOpened;

    public void Init(BaseRoom room)
    {
        _isOpened = true;
        _room = room;
    }

    public void OpenDoor()
    {
        if (_isOpened) return;

        _isOpened = true;
        door.transform.DOMoveY(openPos, 0.05f).SetEase(Ease.InSine);
    }

    public void CloseDoor()
    {
        if (!_isOpened) return;

        _isOpened = false;
        door.transform.DOMoveY(closePos, 0.05f).SetEase(Ease.InSine);
    }
}
