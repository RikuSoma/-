using Unity.VisualScripting;
using UnityEngine;

public class RocketEnemyMovement : MonoBehaviour
{
    [SerializeField] private float hoverSpeed = 1f;
    [SerializeField] private float attackDelay = 2f;
    [SerializeField] private float attackSpeed = 15f;
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isAttacking = false;
    private bool attackStarted = false;
    private float attackTimer = 0f;

    private Vector3 initialPosition;

    private GameObject TargetObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetObject == null || isAttacking) return;

        Vector3 toCenter = GetCameraCenterWorldPosition() - transform.position;

        if (!attackStarted && toCenter.magnitude <= detectionRange)
        {
            attackStarted = true;
            attackTimer = 0f;
        }

        if (attackStarted)
        {
            attackTimer += Time.deltaTime;

            if (!isAttacking)
            {
                ChargeWiggle(); // ← タメ時間中の上下揺れ
            }

            if (attackTimer >= attackDelay)
            {
                AttackTarget();
            }
        }
        else
        {
            Hover(); // 通常のふわふわホバー移動
        }
    }

    void Hover()
    {
        Vector3 hoverDirection = new Vector3(Mathf.Sin(Time.time * 2f), Mathf.Sin(Time.time * 3f), 0f);
        rb.linearVelocity = hoverDirection.normalized * hoverSpeed;
    }

    void AttackTarget()
    {
        isAttacking = true;
        Vector3 direction = (TargetObject.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * attackSpeed;
    }

    Vector3 GetCameraCenterWorldPosition()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 10f); // Z�����͓K�X����
        return Camera.main.ScreenToWorldPoint(screenCenter);
    }

    void ChargeWiggle()
    {
        float frequency = 15f;
        float amplitude = 10f;
        float wiggleY = Mathf.Sin(Time.time * frequency) * amplitude;

        if (wiggleY >= 0f)
        {
            // 上に行くとき → 初期位置に戻る方向（斜め上）
            Vector3 toInitial = (initialPosition - transform.position).normalized;
            toInitial.y = 0f; // 水平成分だけ使う
            Vector3 moveDirection = (toInitial + Vector3.up).normalized;
            rb.linearVelocity = moveDirection * Mathf.Abs(wiggleY);
        }
        else
        {
            // 下に行くとき → ターゲット方向に寄りつつ落下
            if (TargetObject != null)
            {
                Vector3 toTarget = (TargetObject.transform.position - transform.position).normalized;
                toTarget.y = 0f;
                Vector3 moveDirection = (toTarget + Vector3.down).normalized;
                rb.linearVelocity = moveDirection * Mathf.Abs(wiggleY);
            }
            else
            {
                rb.linearVelocity = new Vector3(0f, wiggleY, 0f);
            }
        }
    }

    public void Initialize(RocketEnemyMovementConfig config , GameObject target)
    {
        if (config == null || target == null) return;

        hoverSpeed = config.hoverSpeed;
        attackDelay = config.attackDelay;
        attackSpeed = config.attackSpeed;
        detectionRange = config.detectionRange;

        TargetObject = target;
    }

    public void SetGroundLayer(LayerMask layer) { groundLayer = layer; }
}
