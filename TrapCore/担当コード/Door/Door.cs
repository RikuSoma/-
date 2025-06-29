using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("�Ď��Ώ�")]
    [SerializeField] private GameObject DetectionObject;

    [Header("����ݒ�")]
    [SerializeField] private float MoveSpeed = 1.0f;
    [SerializeField] private float MoveTime = 1.0f;

    [Header("�߂�ݒ�")]
    [SerializeField] private bool isReturnable = true;            // �߂邩�ǂ���
    [SerializeField] private float ReturnWaitTime = 3.0f;         // �߂�܂ł̑ҋ@���ԁi�b�j
    [SerializeField] private bool canWatchAgainAfterReturn = true; // �߂����������x�Ď����邩

    private float nowtime = 0.0f;
    private bool hasTriggered = false;
    private bool isReturning = false;

    private PlayerDetection playerdetection;
    private Vector3 initialPos;

    void Start()
    {
        playerdetection = DetectionObject.GetComponent<PlayerDetection>();
        initialPos = transform.position;
    }

    void Update()
    {
        // �v���C���[����x�ł����m������g���K�[ON
        if (!hasTriggered && playerdetection.IsPlayer())
        {
            hasTriggered = true;
        }

        // �h�A�𗎂Ƃ�����
        if (hasTriggered && nowtime <= MoveTime)
        {
            nowtime += Time.deltaTime;
            transform.Translate(0, -(MoveSpeed * Time.deltaTime), 0);

            // �������I�������߂菈����
            if (nowtime >= MoveTime && isReturnable && !isReturning)
            {
                StartCoroutine(ReturnAfterDelay(ReturnWaitTime));
            }
        }

        // �߂菈����
        if (isReturning)
        {
            float dist = Vector3.Distance(transform.position, initialPos);

            if (dist > 0.01f)
            {
                Vector3 dir = (initialPos - transform.position).normalized;
                transform.Translate(dir * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = initialPos; // �s�^�b�ƈʒu�␳
                isReturning = false;

                if (canWatchAgainAfterReturn)
                {
                    hasTriggered = false;
                    nowtime = 0f;
                }
            }
        }
    }

    IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isReturning = true;
    }
}
