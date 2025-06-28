using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{

    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 1f, -10f);
    public float minX = 0f, maxX = 30f;

    public float followYLine = 5f;  // 追従を開始するYライン

    void LateUpdate()
    {
        CameraTracking();
    }

    void CameraTracking()
    {
        if (!player) return;

        float targetX = player.position.x;
        float targetY;

        // プレイヤーのYがfollowYLineを越えたら追従、それ以下なら固定
        if (player.position.y > followYLine)
        {
            // なめらかにプレイヤーのYに追従
            targetY = player.position.y + offset.y;
        }
        else
        {
            // 追従しないときはカメラのYをoffset.yに固定
            targetY = offset.y;  
        }

        float targetZ = offset.z;

        Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

        // Xはなめらかに補間
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        // 端の制限（X軸）
        smoothPos.x = Mathf.Clamp(smoothPos.x, minX, maxX);

        transform.position = smoothPos;
    }
}
