using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CameraState
    {
        Main = 0,
        CatchCatchLeft = 1,
        CatchCatchMiddle = 2,
        CatchCatchRight = 3,
        PushPushLeft = 4,
        PushPushMiddle = 5,
        PushPushRight = 6,
        SpinSpinLeft = 7,
        SpinSpinMiddle = 8,
        SpinSpinRight = 9
    }

    public CameraState cameraState;  

    private static CameraManager instance;
    public static CameraManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<CameraManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<CameraManager>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<CameraManager>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        else
            DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        cameraState = CameraState.Main;
    }

    public void CatchCatchCameraMoveLeft()
    {
        if (cameraState == CameraState.CatchCatchRight)
        {
            transform.position = new Vector3(1000.0f, 290.0f, -200.0f);
            transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
            cameraState = CameraState.CatchCatchMiddle;
        }
        else if (cameraState == CameraState.CatchCatchMiddle)
        {
            transform.position = new Vector3(780.0f, 290.0f, 0f);
            transform.rotation = Quaternion.Euler(15.0f, 90.0f, 0f);
            cameraState = CameraState.CatchCatchLeft;
        }
    }

    public void CatchCatchCameraMoveRight()
    {
        if (cameraState == CameraState.CatchCatchLeft)
        {
            transform.position = new Vector3(1000.0f, 290.0f, -200.0f);
            transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
            cameraState = CameraState.CatchCatchMiddle;
        }
        else if (cameraState == CameraState.CatchCatchMiddle)
        {
            transform.position = new Vector3(1220.0f, 290.0f, 0f);
            transform.rotation = Quaternion.Euler(15.0f, -90.0f, 0f);
            cameraState = CameraState.CatchCatchRight;
        }
    }

    public void PushPushCameraMoveLeft()
    {
        if (cameraState == CameraState.PushPushRight)
        {
            transform.position = new Vector3(2000.0f, 290.0f, -288.0f);
            transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
            cameraState = CameraState.PushPushMiddle;
        }
        else if (cameraState == CameraState.PushPushMiddle)
        {
            transform.position = new Vector3(1780.0f, 290.0f, -88.0f);
            transform.rotation = Quaternion.Euler(15.0f, 90.0f, 0f);
            cameraState = CameraState.PushPushLeft;
        }
    }

    public void PushPushCameraMoveRight()
    {
        if (cameraState == CameraState.PushPushLeft)
        {
            transform.position = new Vector3(2000.0f, 290.0f, -288.0f);
            transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
            cameraState = CameraState.PushPushMiddle;
        }
        else if (cameraState == CameraState.PushPushMiddle)
        {

            transform.position = new Vector3(2220.0f, 290.0f, -88.0f);
            transform.rotation = Quaternion.Euler(15.0f, -90.0f, 0f);
            cameraState = CameraState.PushPushRight;
        }
    }

    public void SpinSpinCameraMoveLeft()
    {
        if (cameraState == CameraState.SpinSpinRight)
        {
            transform.position = new Vector3(3000.0f, 290.0f, -288.0f);
            transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
            cameraState = CameraState.SpinSpinMiddle;
        }
        else if (cameraState == CameraState.SpinSpinMiddle)
        {
            transform.position = new Vector3(2780.0f, 290.0f, -88.0f);
            transform.rotation = Quaternion.Euler(15.0f, 90.0f, 0f);
            cameraState = CameraState.SpinSpinLeft;
        }
    }

    public void SpinSpinCameraMoveRight()
    {
        if (cameraState == CameraState.SpinSpinLeft)
        {
            transform.position = new Vector3(3000.0f, 290.0f, -288.0f);
            transform.rotation = Quaternion.Euler(15.0f, 0f, 0f);
            cameraState = CameraState.SpinSpinMiddle;
        }
        else if (cameraState == CameraState.SpinSpinMiddle)
        {
            transform.position = new Vector3(3220.0f, 290.0f, -88.0f);
            transform.rotation = Quaternion.Euler(15.0f, -90.0f, 0f);
            cameraState = CameraState.SpinSpinRight;
        }
    }
}