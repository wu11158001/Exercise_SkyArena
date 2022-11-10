using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    static CameraControl cameraFollow;
    public static CameraControl Instance => cameraFollow;

    [Header("Value")]
    [SerializeField] [Tooltip("CameraDistance")] float cameraDistance;
    [SerializeField] [Tooltip("LookHight")] float lookHight;
    [SerializeField] [Tooltip("CameraDistanceLimit(max, min)")] readonly float[] cameraDistanceLimit = new float[] { 8, 4 };
    [SerializeField] [Tooltip("CameraHight")] float cameraHight;
    [SerializeField] [Tooltip("CameraHightLimit(max, min)")] readonly float[] cameraHightLimit = new float[] { 6.5f, -0.5f };
    [SerializeField] [Tooltip("LerpTime")] const float lerpTime = 0.01f;

    [Header("Input")]
    [SerializeField] [Tooltip("InputMouseX")] float inputX;
    [SerializeField] [Tooltip("InputMouseY")] float inputY;
    [SerializeField] [Tooltip("InputMouseScollWhell")] float inputMouseScollWhell;
    [SerializeField] [Tooltip("InputMoreThanTheValue")] float inputMoreThanTheValue;
    [SerializeField] [Tooltip("UpAngle")] float upAngle;
    [SerializeField] [Tooltip("RotateSpeed")] float rotateSpeed;

    [Header("TargetObject")]
    [SerializeField] [Tooltip("targetObject")] Transform targetObject;
    [Tooltip("targetPosition")] Vector3 targetPosition;
    [Tooltip("forwardVector")] Vector3 forwardVector;
    [Tooltip("LookAtPosition")] Vector3 lookAtPosition;

    [Header("Touch")]
    [Tooltip("TouchBeganPosition")] Vector2 touchBeganPosition;
    [Tooltip("TouchRotateSpeedX")] const float touchRotateSpeedX = 1.55f;
    [Tooltip("TouchRotateSpeedY")] const float touchRotateSpeedY = 4.5f;
    [Tooltip("TouchStartActionDistance")] const float touchStartActionDistance = 80;
    [Tooltip("IsZoom")] bool isZoom;
    [Tooltip("DoubleTouchLastDistance")] float DoubleTouchLastDistance;
    [Tooltip("TouchZoomSpeed")] float touchZoomSpeed = 4.5f;

    private void Awake()
    {
        if (cameraFollow != null)
        {
            Destroy(this);
            return;
        }
        cameraFollow = this;
        transform.position = new Vector3(-7.4f, 8, 13);
        transform.rotation = Quaternion.Euler(25, 145, 0);

        //Value
        cameraDistance = 6;
        lookHight = 3;        
        cameraHight = 1.0f;

        //Input
        inputMoreThanTheValue = 0.2f;
        rotateSpeed = 2;
    }

    private void LateUpdate()
    {
        if (targetObject != null)
        {
            OnCameraFollow();
        }
    }

    /// <summary>
    /// CameraFollow
    /// </summary>
    void OnCameraFollow()
    {
#if UNITY_STANDALONE_WIN
        OnMouseControl();
#elif UNITY_ANDROID
        OnTouch();
#endif

        OnCameraRotate();

        //Zoom        
        cameraDistance += -inputMouseScollWhell * rotateSpeed;
        if (cameraDistance >= cameraDistanceLimit[0]) cameraDistance = cameraDistanceLimit[0];
        if (cameraDistance <= cameraDistanceLimit[1]) cameraDistance = cameraDistanceLimit[1];

        //Position
        targetPosition = lookAtPosition + Vector3.up * cameraHight - forwardVector * cameraDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpTime);
        transform.LookAt(targetObject);
    }

    /// <summary>
    /// MouseControl
    /// </summary>
    void OnMouseControl()
    {
        if (Input.GetMouseButton(1))
        {
            inputX = Input.GetAxis("Mouse X");
            inputY = Input.GetAxis("Mouse Y");
        }
        else
        {
            inputX = 0;
            inputY = 0;
        }

        inputMouseScollWhell = Input.GetAxis("Mouse ScrollWheel");
    }

    /// <summary>
    /// Touch
    /// </summary>
    void OnTouch()
    {
        if (Input.touchCount <= 0) return;
        
        //Rotate
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchBeganPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    if (Mathf.Abs(touch.position.x - touchBeganPosition.x) > touchStartActionDistance)
                    {
                        inputX = Mathf.Clamp(touch.position.x - touchBeganPosition.x, -touchRotateSpeedX, touchRotateSpeedX);
                    }
                    if (Mathf.Abs(touch.position.y - touchBeganPosition.y) > touchStartActionDistance)
                    {
                        inputY = Mathf.Clamp(touch.position.y - touchBeganPosition.y, -touchRotateSpeedY, touchRotateSpeedY);
                    }                
                    break;
                case TouchPhase.Stationary:
                    touchBeganPosition = touch.position;                    
                    break;
                case TouchPhase.Ended:
                    inputX = 0;
                    inputY = 0;
                    break;
            }
        }
        
        //Zoom
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);

                if(!isZoom)
                {
                    DoubleTouchLastDistance = currentDistance;
                    isZoom = true;
                }

                float distance = currentDistance - DoubleTouchLastDistance;                
                if (Mathf.Abs(distance) > 10)
                {                    
                    inputMouseScollWhell = Mathf.Clamp(distance, -touchZoomSpeed, touchZoomSpeed);
                }
                DoubleTouchLastDistance = currentDistance;                
            }
        }
        else
        {
            inputMouseScollWhell = 0;
            isZoom = false;
        }
    }

    /// <summary>
    /// OnCameraRotate
    /// </summary>
    void OnCameraRotate()
    {
        //Horizontal Rotate
        if (Mathf.Abs(inputX) > inputMoreThanTheValue)
        {
            lookAtPosition = targetObject.position + targetObject.GetComponent<CapsuleCollider>().center;
            transform.RotateAround(lookAtPosition, Vector3.up, rotateSpeed * inputX);
            forwardVector = Vector3.Cross(transform.right, Vector3.up);
        }

        //Vertical Rotate
        if (Mathf.Abs(inputY) > inputMoreThanTheValue)
        {
            cameraHight += -inputY * rotateSpeed;
            if (cameraHight >= cameraHightLimit[0]) cameraHight = cameraHightLimit[0];
            if (cameraHight <= cameraHightLimit[1]) cameraHight = cameraHightLimit[1];
        }
    }

    /// <summary>
    /// SetFollowTarget
    /// </summary>
    public Transform SetFollowTarget
    {
        set
        {
            targetObject = value;
            forwardVector = targetObject.forward;
            lookAtPosition = targetObject.position + Vector3.up * lookHight;
            targetPosition = lookAtPosition + Vector3.up * cameraHight - Vector3.Cross(transform.right, Vector3.up) * cameraDistance;
        }
    }
}
