using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{

    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 1f, -10f);
    public float minX = 0f, maxX = 30f;

    public float followYLine = 5f;  // �Ǐ]���J�n����Y���C��

    void LateUpdate()
    {
        CameraTracking();
    }

    void CameraTracking()
    {
        if (!player) return;

        float targetX = player.position.x;
        float targetY;

        // �v���C���[��Y��followYLine���z������Ǐ]�A����ȉ��Ȃ�Œ�
        if (player.position.y > followYLine)
        {
            // �Ȃ߂炩�Ƀv���C���[��Y�ɒǏ]
            targetY = player.position.y + offset.y;
        }
        else
        {
            // �Ǐ]���Ȃ��Ƃ��̓J������Y��offset.y�ɌŒ�
            targetY = offset.y;  
        }

        float targetZ = offset.z;

        Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

        // X�͂Ȃ߂炩�ɕ��
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        // �[�̐����iX���j
        smoothPos.x = Mathf.Clamp(smoothPos.x, minX, maxX);

        transform.position = smoothPos;
    }
}
