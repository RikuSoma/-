using UnityEngine;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;

public class BlueBoundEnemyMovement : MonoBehaviour
{
    // --- Publicな変数やプロパティ（各Stateからアクセスできるようにpublicにする） ---
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float moveForce = 3f;
    public float crouchDuration = 0.4f;

    [Header("Crouch Settings")]
    public float crouchScaleY = 0.5f;

    [Header("Wall Detection")]
    public float wallDetectionDistance = 1f;

    [Header("State Durations")]
    public float idleDuration = 0.4f;
    public float stuckDuration = 0.4f;

    // --- コンポーネントへの参照 ---
    public Rigidbody Rb { get; private set; }
    public GameObject TargetObject { get; private set; }
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Vector3 OriginalScale { get; private set; }


    // --- ステート管理 ---
    private BaseState currentState;
    // 各ステートのインスタンスを保持
    public IdleState idleState;
    public PreparingState preparingState;
    public JumpingState jumpingState;
    public StuckRecoveryState stuckRecoveryState;

    void Awake()
    {
        // --- コンポーネントの取得 ---
        Rb = GetComponent<Rigidbody>();
        OriginalScale = transform.localScale;

        // --- 全てのステートをインスタンス化 ---
        idleState = new IdleState(this);
        preparingState = new PreparingState(this);
        jumpingState = new JumpingState(this);
        stuckRecoveryState = new StuckRecoveryState(this);
    }

    void Start()
    {
        // 初期ステートを設定
        currentState = idleState;
        currentState.EnterState();
    }

    void Update()
    {
        if (TargetObject == null) return;

        // 現在のステートのUpdate処理を呼び出すだけ
        currentState?.UpdateState();
    }

    // ステートを切り替えるためのメソッド
    public void ChangeState(BaseState newState)
    {
        currentState?.ExitState(); // 現在のステートの終了処理を呼ぶ
        currentState = newState;
        currentState.EnterState();  // 新しいステートの開始処理を呼ぶ
    }

    // しゃがみ動作のコルーチン（Stateから呼び出される）
    public IEnumerator CrouchCoroutine()
    {
        transform.localScale = new Vector3(OriginalScale.x, crouchScaleY, OriginalScale.z);
        yield return new WaitForSeconds(crouchDuration);
        transform.localScale = OriginalScale;

        // しゃがみ終わったらジャンプ実行
        PerformJump();
    }

    // ジャンプ実行処理
    private void PerformJump()
    {
        if (TargetObject == null)
        {
            ChangeState(idleState);
            return;
        }

        Vector3 toPlayer = (TargetObject.transform.position - transform.position).normalized;
        toPlayer.y = 0f;

        if (Physics.Raycast(transform.position, toPlayer, wallDetectionDistance, wallLayer))
        {
            ChangeState(stuckRecoveryState);
        }
        else
        {
            Vector3 jumpDirection = toPlayer * moveForce + Vector3.up * jumpForce;
            Rb.linearVelocity = Vector3.zero;
            Rb.AddForce(jumpDirection, ForceMode.VelocityChange);
            ChangeState(jumpingState);
        }
    }

    // 地面接地判定
    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
    }

    // BlueBoundEnemyMovement.cs に追記
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f); // IsGroundedの半径と合わせる
        }
    }

    // --- 外部から設定するためのメソッド ---
    public void SetTargetObject(GameObject target) { TargetObject = target; }
    public void SetMovementConfig(BlueBoundMovementConfig config) { /* ... */ }
}