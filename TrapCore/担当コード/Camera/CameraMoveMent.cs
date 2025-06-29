using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMent : MonoBehaviour
{
    [SerializeField] private Transform player; // Player��Transform
    [SerializeField] private float thresholdX = 2.0f; // �J�����E�[���牽�P�ʂŒǏ]���n�߂邩

    private float minCameraX; // �J�����̈ړ������i�߂�Ȃ��悤�Ɂj

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraMove�FPlayer���ݒ肳��Ă��܂���I");
            enabled = false;
            return;
        }

        minCameraX = transform.position.x; // �ŏ��̈ʒu�����
    }

    private void LateUpdate()
    {
        if (player == null) return;

        float targetX = transform.position.x;

        // Player���������l���E�ɍs������Ǐ]
        float playerRightEdge = player.position.x;
        float cameraRightEdge = transform.position.x + thresholdX;

        if (playerRightEdge > cameraRightEdge)
        {
            targetX = player.position.x - thresholdX;
        }

        // �J�������߂�Ȃ��悤�ɐ���
        if (targetX > minCameraX)
        {
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            minCameraX = targetX; // �X�V
        }
    }
}
