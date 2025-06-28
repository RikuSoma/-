using UnityEngine;
using System.Collections; // �R���[�`�����g������

public class FallEnemyMovement : MonoBehaviour // EnemyBase���p�����邩�̓v���W�F�N�g�\���ɂ��܂�
{
    [Header("�ړ��ݒ�")]
    [Tooltip("�㉺�ɓ�������")]
    [SerializeField] private float moveDistance = 3.0f;

    [Header("�^�C�~���O�ݒ�i�b���Ŏw��j")]
    [Tooltip("�㏸�ɂ����鎞�ԁB���̎��Ԃ𒷂�����ƁA���t���b�ƕ����オ��܂��B")]
    [SerializeField] private float moveUpDuration = 1.5f;

    [Tooltip("���_�Œ�~���鎞��")]
    [SerializeField] private float pauseAtTopDuration = 0.5f;

    [Tooltip("���~�ɂ����鎞�ԁB������𒷂�����ƁA�������̕��V�������܂�܂��B")]
    [SerializeField] private float moveDownDuration = 2.0f;

    [Tooltip("�n�ʂ̉��ŉB��Ă���i�ҋ@����j����")]
    [SerializeField] private float pauseAtBottomDuration = 1.0f;

    // --- �����ϐ� ---
    private Vector3 startPosition;
    private Vector3 topPosition;

    void Start()
    {
        startPosition = transform.position;
        topPosition = startPosition + new Vector3(0, moveDistance, 0);

        // ����J�n
        StartCoroutine(MovementCycle());
    }

    private IEnumerator MovementCycle()
    {
        // ����while(true)�ŁA�㏸?���~�̃T�C�N���𖳌��ɌJ��Ԃ��܂�
        while (true)
        {
            // 1. �n�ʂ̉��őҋ@
            if (pauseAtBottomDuration > 0)
            {
                yield return new WaitForSeconds(pauseAtBottomDuration);
            }

            // 2. �㏸
            yield return MoveBetween(startPosition, topPosition, moveUpDuration);

            // 3. ���_�őҋ@
            if (pauseAtTopDuration > 0)
            {
                yield return new WaitForSeconds(pauseAtTopDuration);
            }

            // 4. ���~
            yield return MoveBetween(topPosition, startPosition, moveDownDuration);
        }
    }

    /// <summary>
    /// 2�̒n�_�Ԃ��w�肵�����ԂŊ��炩�Ɉړ����鏈��
    /// </summary>
    private IEnumerator MoveBetween(Vector3 from, Vector3 to, float duration)
    {
        if (duration <= 0)
        {
            transform.position = to;
            yield break;
        }

        float timer = 0f;
        while (timer < duration)
        {
            // timer/duration �ŁA0.0����1.0�ւ̐i�������v�Z
            float progress = timer / duration;

            // SmoothStep�Ői���������炩�ɂ���i���ꂪ���V���̐��́j
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            // 2�_�Ԃ��A�v�Z�����i�����̕������ړ�������
            transform.position = Vector3.Lerp(from, to, easedProgress);

            timer += Time.deltaTime;
            yield return null; // 1�t���[���҂�
        }

        // �҂�����I�_�ɍ��킹��
        transform.position = to;
    }
}