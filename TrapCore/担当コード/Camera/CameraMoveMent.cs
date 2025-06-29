using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMent : MonoBehaviour
{
    [SerializeField] private Transform player; // PlayerのTransform
    [SerializeField] private float thresholdX = 2.0f; // カメラ右端から何単位で追従を始めるか

    private float minCameraX; // カメラの移動制限（戻らないように）

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraMove：Playerが設定されていません！");
            enabled = false;
            return;
        }

        minCameraX = transform.position.x; // 最初の位置を基準に
    }

    private void LateUpdate()
    {
        if (player == null) return;

        float targetX = transform.position.x;

        // Playerがしきい値より右に行ったら追従
        float playerRightEdge = player.position.x;
        float cameraRightEdge = transform.position.x + thresholdX;

        if (playerRightEdge > cameraRightEdge)
        {
            targetX = player.position.x - thresholdX;
        }

        // カメラが戻らないように制限
        if (targetX > minCameraX)
        {
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            minCameraX = targetX; // 更新
        }
    }
}
