using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

enum CameraState
{
    Overview,//大地图
    Gameplay,//进入操作视角
}
public class CameraScripts : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    CameraState cameraState;
    private Camera cam;

    public Transform target;
    public Vector2 deadZone = new Vector2(2f, 2f);
    public float smoothTime = 0.2f;
    private Vector3 _velocity = Vector3.zero;
//====================================相机变量
    private float GAMEPLAYSIZE = 10f;
    private float overviewSize;
    private float targetSize;
    private float sizeVelocity;
    public float sizeSmoothTime = 0.1f;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    private void Start()
    {
        gameManager.OnLevelLoaded += CameraInit;
        cameraState = CameraState.Overview;
    }
    private void Update()
    {
        int level = gameManager.GetLevel();
        EnterGameplay();
    }
    private void LateUpdate()
    {
        CameraSizeSmoothDamp();
        switch(cameraState)
        {   
            default:
            case CameraState.Overview:


            break;
            case CameraState.Gameplay:
                CameraFollow();
            break;
        }
    }
    private void EnterGameplay()
    {
        if(cameraState == CameraState.Overview && 
          (GameInput.Instance.IsUpactionPressed()||
           GameInput.Instance.IsRightactionPressed()||
           GameInput.Instance.IsLeftactionPressed()))
        {
            cameraState = CameraState.Gameplay;
            targetSize = GAMEPLAYSIZE;
        }
    }
    private void CameraSizeSmoothDamp()
    {
        cam.orthographicSize = Mathf.SmoothDamp
        (
            cam.orthographicSize,
            targetSize,
            ref sizeVelocity,
            sizeSmoothTime
        );
    }
    private void CameraInit(CameraSize size)
    {
        if(size != null)
        {   
            overviewSize = size.config.overviewSize;
        }
        else
        {
            Debug.Log("No size was found.");
            overviewSize = 20f;
        }
        targetSize = overviewSize;
        cam.orthographicSize = overviewSize;
    }
    
    
    private void CameraFollow()
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
public class CameraChange
{
    
}
