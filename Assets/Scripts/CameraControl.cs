using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    static CameraControl cameraFollow;
    public static CameraControl Instance => cameraFollow;

    [Header("Value")]
    [SerializeField] [Tooltip("CameraDistance")] float cameraDistance;
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

    private void Awake()
    {
        if (cameraFollow != null)
        {
            Destroy(this);
            return;
        }
        cameraFollow = this;
        transform.position = Vector3.zero;

        //Value
        cameraDistance = 4;
        cameraDistanceLimit = new float[] { 6, 2 };
        cameraHight = 3;
        cameraHightLimit = new float[] { 5, 0.8f };
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
        //¥ª¥k¿ïÂà
        if (Input.GetMouseButton(1))
        {
            inputMouseX = Input.GetAxis("Mouse X");
            inputMouseY = Input.GetAxis("Mouse Y");

            //Horizontal Rotate
            if (Mathf.Abs(inputMouseX) > inputMoreThanTheValue)
            {
                transform.RotateAround(targetObject.position, Vector3.up, rotateSpeed * inputMouseX);
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

        //Zoom
        inputMouseScollWhell = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance += -inputMouseScollWhell * rotateSpeed;
        if (cameraDistance >= cameraDistanceLimit[0]) cameraDistance = cameraDistanceLimit[0];
        if (cameraDistance <= cameraDistanceLimit[1]) cameraDistance = cameraDistanceLimit[1];

        //Position
        targetPosition = targetObject.position + Vector3.up * cameraHight - forwardVector * cameraDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpTime);
        transform.LookAt(targetObject);
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
            targetPosition = targetObject.position + Vector3.up * cameraHight - Vector3.Cross(transform.right, Vector3.up) * cameraDistance;
        }
    }
}
