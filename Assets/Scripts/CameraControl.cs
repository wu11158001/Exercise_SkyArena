using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攝影機跟隨
/// </summary>
public class CameraControl : MonoBehaviour
{
    static CameraControl cameraFollow;
    public static CameraControl Instance => cameraFollow;

    [Header("跟隨")]
    [SerializeField] [Tooltip("攝影機距離")] float cameraDistance;
    [SerializeField] [Tooltip("攝影機距離限制(最大,最小)")] float[] cameraDistanceLimit;
    [SerializeField] [Tooltip("攝影機高度")] float cameraHight;
    [SerializeField] [Tooltip("攝影機高度限制(最大,最小)")] float[] cameraHightLimit;
    [SerializeField] [Tooltip("LerpTime")] float lerphTime;

    [Header("選轉")]
    [SerializeField] [Tooltip("輸入MouseX")] float inputMouseX;
    [SerializeField] [Tooltip("輸入MouseY")] float inputMouseY;
    [SerializeField] [Tooltip("輸入MouseScollWhell")] float inputMouseScollWhell;
    [SerializeField] [Tooltip("輸入值大於多少開始轉動")] float inputMoreThanTheValue;
    [SerializeField] [Tooltip("向上角度")] float upAngle;
    [SerializeField] [Tooltip("選轉速度")] float rotateSpeed;

    [Header("跟隨物件")]
    [SerializeField] [Tooltip("跟隨物件")] Transform targetObject;
    [Tooltip("跟隨物件位置")] Vector3 targetPosition;
    [Tooltip("Forward向量")] Vector3 forwardVector;

    private void Awake()
    {
        if (cameraFollow != null)
        {
            Destroy(this);
            return;
        }
        cameraFollow = this;

        transform.position = Vector3.zero;//初始位置

        //跟隨
        cameraDistance = 4;//攝影機距離
        cameraDistanceLimit = new float[] { 6, 2 };//攝影機距離限制(最大,最小)
        cameraHight = 3;//攝影機高度
        cameraHightLimit = new float[] { 5, 0.8f };//攝影機高度限制(最大,最小)
        lerphTime = 0.01f;//LerpTime

        //選轉
        inputMoreThanTheValue = 0.2f;
        rotateSpeed = 2;//選轉速度
    }

    private void LateUpdate()
    {
        if (targetObject != null)
        {
            OnCameraFollow();
        }
    }

    /// <summary>
    /// 攝影機控制
    /// </summary>
    void OnCameraFollow()
    {
        //左右選轉
        if (Input.GetMouseButton(1))
        {
            inputMouseX = Input.GetAxis("Mouse X");
            inputMouseY = Input.GetAxis("Mouse Y");

            //左右轉動
            if (Mathf.Abs(inputMouseX) > inputMoreThanTheValue)
            {
                transform.RotateAround(targetObject.position, Vector3.up, rotateSpeed * inputMouseX);
                forwardVector = Vector3.Cross(transform.right, Vector3.up);
            }

            //高度移動
            if (Mathf.Abs(inputMouseY) > inputMoreThanTheValue)
            {
                cameraHight += -inputMouseY * rotateSpeed;
                if (cameraHight >= cameraHightLimit[0]) cameraHight = cameraHightLimit[0];
                if (cameraHight <= cameraHightLimit[1]) cameraHight = cameraHightLimit[1];
            }
        }

        //遠近移動
        inputMouseScollWhell = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance += -inputMouseScollWhell * rotateSpeed;
        if (cameraDistance >= cameraDistanceLimit[0]) cameraDistance = cameraDistanceLimit[0];
        if (cameraDistance <= cameraDistanceLimit[1]) cameraDistance = cameraDistanceLimit[1];

        //跟隨
        targetPosition = targetObject.position + Vector3.up * cameraHight - forwardVector * cameraDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerphTime);
        transform.LookAt(targetObject);
    }

    /// <summary>
    /// 設定跟隨目標
    /// </summary>
    public Transform SetFollowTarget
    {
        set
        {
            targetObject = value;//跟隨物件
            forwardVector = targetObject.forward;//Forward向量            
            targetPosition = targetObject.position + Vector3.up * cameraHight - Vector3.Cross(transform.right, Vector3.up) * cameraDistance;//跟隨物件位置            
        }
    }
}
