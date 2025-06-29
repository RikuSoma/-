using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("監視対象")]
    [SerializeField] private GameObject DetectionObject;

    [Header("動作設定")]
    [SerializeField] private float MoveSpeed = 1.0f;
    [SerializeField] private float MoveTime = 1.0f;

    [Header("戻り設定")]
    [SerializeField] private bool isReturnable = true;            // 戻るかどうか
    [SerializeField] private float ReturnWaitTime = 3.0f;         // 戻るまでの待機時間（秒）
    [SerializeField] private bool canWatchAgainAfterReturn = true; // 戻った後もう一度監視するか

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
        // プレイヤーを一度でも感知したらトリガーON
        if (!hasTriggered && playerdetection.IsPlayer())
        {
            hasTriggered = true;
        }

        // ドアを落とす動作
        if (hasTriggered && nowtime <= MoveTime)
        {
            nowtime += Time.deltaTime;
            transform.Translate(0, -(MoveSpeed * Time.deltaTime), 0);

            // 落下が終わったら戻り処理へ
            if (nowtime >= MoveTime && isReturnable && !isReturning)
            {
                StartCoroutine(ReturnAfterDelay(ReturnWaitTime));
            }
        }

        // 戻り処理中
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
                transform.position = initialPos; // ピタッと位置補正
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
