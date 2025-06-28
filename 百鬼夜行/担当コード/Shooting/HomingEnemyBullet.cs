using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingEnemyBullet : Bullet
{
    private GameObject targetObject;
    private Rigidbody rb;

    [Header("弾の性能")]
    [SerializeField] private float homingStrength = 20f; // 追尾の強さ

    new void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (targetObject == null)
        {
            return; // ターゲットがいない場合は直進するだけ
        }

        // --- Y軸追尾ロジック ---
        Vector3 currentVelocity = rb.linearVelocity;
        float speed = currentVelocity.magnitude; // 現在の速度を取得

        Vector3 targetDirection = (targetObject.transform.position - transform.position).normalized;
        float desiredYVelocity = targetDirection.y * speed;

        float newYVelocity = Mathf.MoveTowards(
            currentVelocity.y,
            desiredYVelocity,
            homingStrength * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(currentVelocity.x, newYVelocity, currentVelocity.z);

        if (rb.linearVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }
}