using UnityEngine;
using System.Collections;

public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null)
                _camera = Camera.main;
            return _camera;
        }
    }

    public static Vector3 GetScreenPointToWorldPointInRectangle(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, rect.position, Camera, out var result);
        return result;
    }

    public static Vector2 GetWorldToScreenPoint(Vector3 pos)
    {
        return RectTransformUtility.WorldToScreenPoint(Camera, pos);
    }
}
