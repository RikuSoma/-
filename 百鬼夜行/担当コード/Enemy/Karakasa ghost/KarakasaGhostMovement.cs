using UnityEngine;
using System.Collections;

public class KarakasaGhostMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float bounceUpForce = 10f;
    [SerializeField] private float floatFallSpeed = 1f;
    [SerializeField] private float swingSpeed = 0.5f;
    [SerializeField] private float swingRange = 0.2f;
    private float swingTimer = 0f;
    [SerializeField] private float jumpForce = 7f; // ← やや強めに
    [SerializeField] private float jumpInterval = 2f;
    [SerializeField] private float crouchDuration = 0.4f;
    private float nextJumpTime;
    private TargetingEnemy targetingEnemy;
    private GroundCheck groundChecker;
    private Vector3 originalScale;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchScaleY = 0.5f;

    private bool isFalling = true;
    private bool hasBounced = false;
    private bool isChargingJump = false;
    private bool justJumped = false;
    private float justJumpedTime = 0f;
    [SerializeField] private float groundedIgnoreTime = 0.4f;

    public bool IsActuallyGrounded =>
        groundChecker != null && groundChecker.IsGrounded && !JustJumped();

    public bool JustJumped()
    {
        return justJumped && (Time.time - justJumpedTime < groundedIgnoreTime);
    }

    public void Initialize(TargetingEnemy enemy)
    {
        targetingEnemy = enemy;
        isFalling = true;
        hasBounced = false;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = false;
            }
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        nextJumpTime = Time.time + jumpInterval;

        groundChecker = GetComponentInChildren<GroundCheck>();
        if (groundChecker == null)
        {
            Debug.LogError("GroundCheck 子オブジェクトが見つかりません！");
        }

        originalScale = transform.localScale;
        isChargingJump = false;
    }

    void FixedUpdate()
    {
        if (targetingEnemy == null) return;

        if (isFalling && !hasBounced)
        {
            rb.linearVelocity = Vector3.down * floatFallSpeed;
            rb.angularVelocity = Vector3.zero;

            swingTimer += Time.deltaTime * swingSpeed;
            float horizontalOffset = Mathf.Sin(swingTimer) * swingRange;
            rb.linearVelocity = new Vector3(horizontalOffset, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else if (IsActuallyGrounded && targetingEnemy?.Target != null)
        {
            if (!isChargingJump && Time.time >= nextJumpTime)
            {
                StartCoroutine(JumpSequence());
            }
        }
    }

    private IEnumerator JumpSequence()
    {
        isChargingJump = true;
        rb.linearVelocity = Vector3.zero;

        Vector3 crouchScale = new Vector3(originalScale.x, crouchScaleY, originalScale.z);
        float heightDiff = (originalScale.y - crouchScaleY) / 2f;
        transform.localScale = crouchScale;
        transform.position -= new Vector3(0f, heightDiff, 0f);

        yield return new WaitForSeconds(crouchDuration);

        transform.position += new Vector3(0f, heightDiff * 1.1f, 0f); // 少し余裕
        transform.localScale = originalScale;

        // Y方向にさらに浮かせてGroundCheckを回避
        transform.position += new Vector3(0f, 0.1f, 0f);

        JumpTowardsTarget();

        isChargingJump = false;
        nextJumpTime = Time.time + jumpInterval;
    }

    private void JumpTowardsTarget()
    {
        if (targetingEnemy.Target == null) return;

        Vector3 toTarget = (targetingEnemy.Target.transform.position - transform.position);
        toTarget.y = 0f;

        Vector3 jumpDir = (toTarget.normalized + Vector3.up).normalized;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(jumpDir * jumpForce, ForceMode.VelocityChange);

        justJumped = true;
        justJumpedTime = Time.time;

        Debug.Log($"[Jump] Unified Jump at: {Time.time}, Dir: {jumpDir}, Pos: {transform.position}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFalling && !hasBounced && collision.gameObject.CompareTag("Ground"))
        {
            isFalling = false;
            hasBounced = true;
            rb.linearVelocity = Vector3.up * bounceUpForce;
            Invoke(nameof(StartFloatingFall), 0.2f);
        }
    }

    private void StartFloatingFall()
    {
        isFalling = true;
        hasBounced = false;
        swingTimer = 0f;
        isChargingJump = false;
    }

    public void SetConfig(KarakasaMovementConfig config) 
    {
        if (config == null) return;
        bounceUpForce = config.bounceUpForce;
        floatFallSpeed = config.floatFallSpeed;
        swingSpeed = config.swingSpeed;
        swingRange = config.swingRange;
        jumpForce = config.jumpForce;
        jumpInterval = config.jumpInterval;
        crouchDuration = config.crouchDuration;
        crouchScaleY = config.crouchScaleY;
        groundedIgnoreTime = config.groundedIgnoreTime;
    }
}
