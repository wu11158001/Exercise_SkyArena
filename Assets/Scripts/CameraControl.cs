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
    [SerializeField] [Tooltip("CameraDistanceLimit(max, min)")] float[] cameraDistanceLimit;
    [SerializeField] [Tooltip("CameraHight")] float cameraHight;
    [SerializeField] [Tooltip("CameraHightLimit(max, min)")] float[] cameraHightLimit;
    [SerializeField] [Tooltip("LerpTime")] float lerpTime;

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
        cameraDistanceLimit = new float[] { 10, 4 };
        cameraHight = 1.0f;
        cameraHightLimit = new float[] { 8, 0.8f };
        lerpTime = 0.01f;

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
        OnCameraRotate();

        //Zoom
        inputMouseScollWhell = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance += -inputMouseScollWhell * rotateSpeed;
        if (cameraDistance >= cameraDistanceLimit[0]) cameraDistance = cameraDistanceLimit[0];
        if (cameraDistance <= cameraDistanceLimit[1]) cameraDistance = cameraDistanceLimit[1];

        //Position
        targetPosition = lookAtPosition + Vector3.up * cameraHight - forwardVector * cameraDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpTime);
        transform.LookAt(targetObject);

#if UNITY_EDITOR_WIN
        OnMouseControl();
#elif UNITY_ANDROID
        OnTouch();
#endif
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
    }

    [Tooltip("TouchBeganPosition")] Vector2 touchBeganPosition;
    [Tooltip("TouchRotateSpeedX")] const float touchRotateSpeedX = 1;
    [Tooltip("TouchRotateSpeedY")] const float touchRotateSpeedY = 5;
    [Tooltip("Touch")]
    /// <summary>
    /// Touch
    /// </summary>
    void OnTouch()
    {        
        Touch touch = Input.GetTouch(0);
        
        if (Input.touchCount == 1)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchBeganPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    Vector2 pos = touch.position;
                    float distanceX = Mathf.Abs((touchBeganPosition.x - pos.x));
                    float distanceY = Mathf.Abs((touchBeganPosition.y - pos.y));
                    if (distanceX > 100)
                    {
                        if (touchBeganPosition.x > pos.x) inputX = touchRotateSpeedX;
                        if (touchBeganPosition.x < pos.x) inputX = -touchRotateSpeedX;
                    }
                    if (distanceY > 100)
                    {
                        if (touchBeganPosition.y < pos.y) inputY = touchRotateSpeedY * 2;
                        if (touchBeganPosition.y > pos.y) inputY = -touchRotateSpeedY * 2;
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
