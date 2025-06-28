using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーなどのオブジェクトが下から通り抜けられるようにし、
/// 外部からの命令で衝突判定をON/OFFする一方通行の足場。
/// </summary>
public class OneWayPlatform3D : MonoBehaviour
{
    [Tooltip("上に乗るための、薄い物理コライダー（IsTriggerがOFFのもの）")]
    [SerializeField]
    private Collider platformCollider;

    [Tooltip("オブジェクトの出入りを検知するための、大きなトリガー（IsTriggerがONのもの）")]
    [SerializeField]
    private Collider platformTrigger;

    private const string DEBUG_KEY = "[OneWayPlatformDebug]";

    private void Start()
    {
        if (platformCollider == null)
            Debug.LogError($"{DEBUG_KEY} ({gameObject.name}): 'Platform Collider'がインスペクターで設定されていません！", this);
        if (platformTrigger == null)
            Debug.LogError($"{DEBUG_KEY} ({gameObject.name}): 'Platform Trigger'がインスペクターで設定されていません！", this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y < platformTrigger.bounds.center.y)
            {
                IgnoreCollision(other);
            }
        }
    }

    // ▼▼▼ このメソッドが修正されました ▼▼▼
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーの中心点が、足場のコライダーの中心点より上に出た場合のみ、衝突を元に戻す
            // これにより、下からジャンプで突き抜けている途中で引っかからなくなる
            if (other.transform.position.y > platformCollider.bounds.center.y)
            {
                StartCoroutine(RestoreCollisionAfterFrame(other));
                Debug.Log($"[OneWayPlatformDebug] ({gameObject.name}): Player is above platform, restoring collision.");
            }
        }
    }

    public void IgnoreCollision(Collider objectToIgnore)
    {
        if (this.enabled && platformCollider != null && objectToIgnore != null)
        {
            Physics.IgnoreCollision(objectToIgnore, platformCollider, true);
            Debug.Log($"{DEBUG_KEY} ({gameObject.name}): IGNORE collision with '{objectToIgnore.name}'.");
        }
    }

    public void RestoreCollision(Collider objectToRestore)
    {
        if (this.enabled && platformCollider != null && objectToRestore != null)
        {
            Physics.IgnoreCollision(objectToRestore, platformCollider, false);
            Debug.Log($"{DEBUG_KEY} ({gameObject.name}): RESTORE collision with '{objectToRestore.name}'.");
        }
    }

    private IEnumerator RestoreCollisionAfterFrame(Collider objectToRestore)
    {
        yield return new WaitForEndOfFrame();
        RestoreCollision(objectToRestore);
    }
}