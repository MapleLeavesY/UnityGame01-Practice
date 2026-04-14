using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 deadZone = new Vector2(2f, 2f);
    public float smoothTime = 0.2f;
    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate()
    {
        if(target == null) return;
        Vector3 cameraCurrent = transform.position;
        Vector3 targetCurrent = target.position;
        
        float targetOffsetX = targetCurrent.x - cameraCurrent.x;
        float targetOffsetY = targetCurrent.y - cameraCurrent.y;

        float borderX = deadZone.x * 0.5f;
        float borderY = deadZone.y * 0.5f;

        float moveX = 0f;
        float moveY = 0f;
        if (Mathf.Abs(targetOffsetX) > borderX)
        {
            moveX = targetOffsetX - Mathf.Sign(targetOffsetX) * borderX;
        }

        if (Mathf.Abs(targetOffsetY) > borderY)
        {
            moveY = targetOffsetY - Mathf.Sign(targetOffsetY) * borderY;
        }

        if (moveX != 0 || moveY != 0)
        {
            Vector3 cameraOffset = new Vector3
            (
                cameraCurrent.x + moveX,
                cameraCurrent.y + moveY,
                cameraCurrent.z
            );
            transform.position = Vector3.SmoothDamp
            (
                cameraCurrent,
                cameraOffset,
                ref _velocity,
                smoothTime
            );
        }
    }
}
