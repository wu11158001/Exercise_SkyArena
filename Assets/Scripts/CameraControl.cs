using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��v�����H
/// </summary>
public class CameraControl : MonoBehaviour
{
    static CameraControl cameraFollow;
    public static CameraControl Instance => cameraFollow;

    [Header("���H")]
    [SerializeField] [Tooltip("��v���Z��")] float cameraDistance;
    [SerializeField] [Tooltip("��v���Z������(�̤j,�̤p)")] float[] cameraDistanceLimit;
    [SerializeField] [Tooltip("��v������")] float cameraHight;
    [SerializeField] [Tooltip("��v�����׭���(�̤j,�̤p)")] float[] cameraHightLimit;
    [SerializeField] [Tooltip("LerpTime")] float lerphTime;

    [Header("����")]
    [SerializeField] [Tooltip("��JMouseX")] float inputMouseX;
    [SerializeField] [Tooltip("��JMouseY")] float inputMouseY;
    [SerializeField] [Tooltip("��JMouseScollWhell")] float inputMouseScollWhell;
    [SerializeField] [Tooltip("��J�Ȥj��h�ֶ}�l���")] float inputMoreThanTheValue;
    [SerializeField] [Tooltip("�V�W����")] float upAngle;
    [SerializeField] [Tooltip("����t��")] float rotateSpeed;

    [Header("���H����")]
    [SerializeField] [Tooltip("���H����")] Transform targetObject;
    [Tooltip("���H�����m")] Vector3 targetPosition;
    [Tooltip("Forward�V�q")] Vector3 forwardVector;

    private void Awake()
    {
        if (cameraFollow != null)
        {
            Destroy(this);
            return;
        }
        cameraFollow = this;

        transform.position = Vector3.zero;//��l��m

        //���H
        cameraDistance = 4;//��v���Z��
        cameraDistanceLimit = new float[] { 6, 2 };//��v���Z������(�̤j,�̤p)
        cameraHight = 3;//��v������
        cameraHightLimit = new float[] { 5, 0.8f };//��v�����׭���(�̤j,�̤p)
        lerphTime = 0.01f;//LerpTime

        //����
        inputMoreThanTheValue = 0.2f;
        rotateSpeed = 2;//����t��
    }

    private void LateUpdate()
    {
        if (targetObject != null)
        {
            OnCameraFollow();
        }
    }

    /// <summary>
    /// ��v������
    /// </summary>
    void OnCameraFollow()
    {
        //���k����
        if (Input.GetMouseButton(1))
        {
            inputMouseX = Input.GetAxis("Mouse X");
            inputMouseY = Input.GetAxis("Mouse Y");

            //���k���
            if (Mathf.Abs(inputMouseX) > inputMoreThanTheValue)
            {
                transform.RotateAround(targetObject.position, Vector3.up, rotateSpeed * inputMouseX);
                forwardVector = Vector3.Cross(transform.right, Vector3.up);
            }

            //���ײ���
            if (Mathf.Abs(inputMouseY) > inputMoreThanTheValue)
            {
                cameraHight += -inputMouseY * rotateSpeed;
                if (cameraHight >= cameraHightLimit[0]) cameraHight = cameraHightLimit[0];
                if (cameraHight <= cameraHightLimit[1]) cameraHight = cameraHightLimit[1];
            }
        }

        //���񲾰�
        inputMouseScollWhell = Input.GetAxis("Mouse ScrollWheel");
        cameraDistance += -inputMouseScollWhell * rotateSpeed;
        if (cameraDistance >= cameraDistanceLimit[0]) cameraDistance = cameraDistanceLimit[0];
        if (cameraDistance <= cameraDistanceLimit[1]) cameraDistance = cameraDistanceLimit[1];

        //���H
        targetPosition = targetObject.position + Vector3.up * cameraHight - forwardVector * cameraDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerphTime);
        transform.LookAt(targetObject);
    }

    /// <summary>
    /// �]�w���H�ؼ�
    /// </summary>
    public Transform SetFollowTarget
    {
        set
        {
            targetObject = value;//���H����
            forwardVector = targetObject.forward;//Forward�V�q            
            targetPosition = targetObject.position + Vector3.up * cameraHight - Vector3.Cross(transform.right, Vector3.up) * cameraDistance;//���H�����m            
        }
    }
}
