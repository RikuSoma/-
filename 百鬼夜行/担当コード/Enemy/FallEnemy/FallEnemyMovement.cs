using UnityEngine;
using System.Collections; // コルーチンを使うため

public class FallEnemyMovement : MonoBehaviour // EnemyBaseを継承するかはプロジェクト構成によります
{
    [Header("移動設定")]
    [Tooltip("上下に動く高さ")]
    [SerializeField] private float moveDistance = 3.0f;

    [Header("タイミング設定（秒数で指定）")]
    [Tooltip("上昇にかかる時間。この時間を長くすると、よりフワッと浮き上がります。")]
    [SerializeField] private float moveUpDuration = 1.5f;

    [Tooltip("頂点で停止する時間")]
    [SerializeField] private float pauseAtTopDuration = 0.5f;

    [Tooltip("下降にかかる時間。こちらを長くすると、落下時の浮遊感が強まります。")]
    [SerializeField] private float moveDownDuration = 2.0f;

    [Tooltip("地面の下で隠れている（待機する）時間")]
    [SerializeField] private float pauseAtBottomDuration = 1.0f;

    // --- 内部変数 ---
    private Vector3 startPosition;
    private Vector3 topPosition;

    void Start()
    {
        startPosition = transform.position;
        topPosition = startPosition + new Vector3(0, moveDistance, 0);

        // 動作開始
        StartCoroutine(MovementCycle());
    }

    private IEnumerator MovementCycle()
    {
        // このwhile(true)で、上昇?下降のサイクルを無限に繰り返します
        while (true)
        {
            // 1. 地面の下で待機
            if (pauseAtBottomDuration > 0)
            {
                yield return new WaitForSeconds(pauseAtBottomDuration);
            }

            // 2. 上昇
            yield return MoveBetween(startPosition, topPosition, moveUpDuration);

            // 3. 頂点で待機
            if (pauseAtTopDuration > 0)
            {
                yield return new WaitForSeconds(pauseAtTopDuration);
            }

            // 4. 下降
            yield return MoveBetween(topPosition, startPosition, moveDownDuration);
        }
    }

    /// <summary>
    /// 2つの地点間を指定した時間で滑らかに移動する処理
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
            // timer/duration で、0.0から1.0への進捗率を計算
            float progress = timer / duration;

            // SmoothStepで進捗率を滑らかにする（これが浮遊感の正体）
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            // 2点間を、計算した進捗率の分だけ移動させる
            transform.position = Vector3.Lerp(from, to, easedProgress);

            timer += Time.deltaTime;
            yield return null; // 1フレーム待つ
        }

        // ぴったり終点に合わせる
        transform.position = to;
    }
}