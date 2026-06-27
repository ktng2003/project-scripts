using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float distance = 150.0f;
    public float speed = 25.0f;

    private Vector3 lastMousePos;

    private bool isMouseDown = false;

    Quaternion rotation;

    void Start()
    {
        rotation = transform.rotation;
    }

    void LateUpdate()
    {
        MouseLook();
    }

    bool IsTouchingUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
    if (Input.touchCount > 0)
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    else
        return false;
#else
    return false;
#endif
    }

    void MouseLook()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsTouchingUI())
            {
                isMouseDown = true;
                lastMousePos = Input.mousePosition;
            }
            else
                isMouseDown = false;
        }

        if (isMouseDown && Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 delta = currentMousePos - lastMousePos;

            float normX = delta.x / Screen.width;
            float normY = delta.y / Screen.height;

            Quaternion yaw = Quaternion.AngleAxis(normX * speed, transform.up);
            Quaternion pitch = Quaternion.AngleAxis(-normY * speed, transform.right);

            rotation = yaw * pitch * rotation;
            transform.rotation = rotation;

            lastMousePos = currentMousePos;
        }

        if (Input.GetMouseButtonUp(0))
            isMouseDown = false;

        transform.position = GameManager.Instance.blackHole.position - transform.forward * distance;
    }
}