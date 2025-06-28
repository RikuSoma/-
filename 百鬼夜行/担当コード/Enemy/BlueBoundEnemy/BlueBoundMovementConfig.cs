using UnityEngine;

[CreateAssetMenu(fileName = "BlueBoundMovementConfig", menuName = "Enemy/BlueBoundMovementConfig")]
public class BlueBoundMovementConfig : ScriptableObject
{
    public float jumpForce = 10f;              // �W�����v�̏�����̗�
    public float moveForce = 3f;               // �������̈ړ���
    public float jumpInterval = 1.5f;          // �W�����v�܂ł̊Ԋu
    public float crouchDuration = 0.4f;        // ���Ⴊ�݁i���߁j����
    public float crouchScaleY = 0.5f;          // ���Ⴊ�ݎ��̃X�P�[��
    public float wallDetectionDistance = 1f;   // �ǂƂ̐ڋߋ���
}
