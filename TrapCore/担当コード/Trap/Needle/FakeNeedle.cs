using System.Collections.Generic;
using UnityEngine;

public class FakeNeedle : TrapBase
{
    [System.Serializable]
    public enum TriggerType
    {
        None,
        Shake,
        Jump
    }

    [System.Serializable]
    public class DetectionCondition
    {
        public GameObject targetDetection;
        public TriggerType triggerType;
    }

    [SerializeField] private float shakeAmount = 0.05f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Vector2 jumpDirection = Vector2.up; // ✅ デフォルトは上方向

    private List<DetectionCondition> conditions = new();
    private List<PlayerDetection> playerDetections = new();
    private List<bool> shakeTriggeredFlags = new();

    private Vector3 originalPosition;
    private bool isShaking = false;
    private bool hasActivated = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;

        foreach (var cond in conditions)
        {
            var detection = cond.targetDetection?.GetComponent<PlayerDetection>();
            if (detection != null)
            {
                playerDetections.Add(detection);
                shakeTriggeredFlags.Add(false);
            }
            else
            {
                Debug.LogWarning($"{cond.targetDetection?.name} に PlayerDetection がついてないよ！");
            }
        }
    }

    void Update()
    {
        if (hasActivated) return;

        bool shouldJump = true;
        isShaking = false;

        for (int i = 0; i < conditions.Count; i++)
        {
            var detection = playerDetections[i];
            var triggered = detection.IsPlayer();
            var type = conditions[i].triggerType;

            if (type == TriggerType.Jump)
            {
                if (!triggered)
                    shouldJump = false;
            }
            else if (type == TriggerType.Shake)
            {
                if (triggered)
                {
                    shakeTriggeredFlags[i] = true;
                }

                if (shakeTriggeredFlags[i])
                {
                    isShaking = true;
                }
            }
        }

        if (shouldJump) Jump();

        if (isShaking)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * shakeAmount;
            transform.position = originalPosition + (Vector3)shakeOffset;
        }
        else
        {
            transform.position = originalPosition;
        }
    }

    void Jump()
    {
        rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode2D.Impulse); // ✅ 任意方向にジャンプ
        hasActivated = true;
        isShaking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealthManager playerhealth = collision.gameObject.GetComponent<PlayerHealthManager>();
        if (playerhealth)
        {
            playerhealth.TakeDamage(Power);
        }
    }

    // 外部から条件をセットするための関数
    public void SetConditions(List<DetectionCondition> externalConditions)
    {
        conditions = externalConditions;
    }

    // 外部からジャンプ方向を設定できるように
    public void SetJumpDirection(Vector2 direction)
    {
        jumpDirection = direction;
    }

    // 外部からジャンプ力も変更できるように（任意）
    public void SetJumpForce(float force)
    {
        jumpForce = force;
    }
}
