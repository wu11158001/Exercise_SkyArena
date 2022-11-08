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
    [SerializeField] [Tooltip("InputMouseX")] float inputMouseX;
    [SerializeField] [Tooltip("InputMouseY")] float inputMouseY;
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
        OnTouch();
#if UNITY_EDITOR_WIN
        //Rotate
        if (Input.GetMouseButton(1))
        {
            inputMouseX = Input.GetAxis("Mouse X");
            inputMouseY = Input.GetAxis("Mouse Y");

            OnCameraRotate();            
        }
#endif

        //Zoom
        inputMouseScollWhell = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance += -inputMouseScollWhell * rotateSpeed;
        if (cameraDistance >= cameraDistanceLimit[0]) cameraDistance = cameraDistanceLimit[0];
        if (cameraDistance <= cameraDistanceLimit[1]) cameraDistance = cameraDistanceLimit[1];

        //Position
        targetPosition = lookAtPosition + Vector3.up * cameraHight - forwardVector * cameraDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpTime);
        transform.LookAt(targetObject);
    }

    [Tooltip("TouchStartPosition")] Vector2 touchStartPosition;
    /// <summary>
    /// Touch
    /// </summary>
    void OnTouch()
    {
#if UNITY_ANDROID
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                    inputMouseX += Mathf.Clamp01((touch.position - touchStartPosition).magnitude) * 0.01f;
                    OnCameraRotate();
                    break;
            }
        }
#endif
    }

    /// <summary>
    /// OnCameraRotate
    /// </summary>
    void OnCameraRotate()
    {
        //Horizontal Rotate
        if (Mathf.Abs(inputMouseX) > inputMoreThanTheValue)
        {
            lookAtPosition = targetObject.position + targetObject.GetComponent<CapsuleCollider>().center;
            transform.RotateAround(lookAtPosition, Vector3.up, rotateSpeed * inputMouseX);
            forwardVector = Vector3.Cross(transform.right, Vector3.up);
        }

        //Vertical Rotate
        if (Mathf.Abs(inputMouseY) > inputMoreThanTheValue)
        {
            cameraHight += -inputMouseY * rotateSpeed;
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
