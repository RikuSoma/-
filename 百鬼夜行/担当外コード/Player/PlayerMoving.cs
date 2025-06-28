using UnityEngine;

public class PlayerMoving : IPlayerState
{
    private Player player;
    private PlayerStateMachine playerStateMachine;
    private Rigidbody rb;

    [Header("移動設定")]
    public float moveSpeed = 8f;
    public float airControlMultiplier = 0.7f;  // 空中制御の効き具合
    public float groundFriction = 15f;         // 地面での摩擦力
    public float airFriction = 3f;            // 空中での摩擦力

    private float moveInput = 0f;

    // 初期化
    public void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;

        rb = player.GetComponent<Rigidbody>();
    }

    // 入力を受け取り、処理に反映
    public void HandleInput()
    {
        moveInput = playerStateMachine.HorizontalInput;  // 横方向の入力を保存
    }

    // 毎フレームの移動処理
    public void Update()
    {
        // しゃがんでいる状態では移動させない
        var crouchState = playerStateMachine.GetState<PlayerCrouching>();
        if (crouchState != null && crouchState.GetCrouching()) { return; }

        Move();
    }

    // 必要に応じて状態終了時に処理
    public void Remove()
    {
        // 必要があれば終了処理
    }

    // 移動の実行
    private void Move()
    {
        Vector3 inputDir = new Vector3(moveInput, 0f, 0f).normalized;

        bool grounded = player.IsGrounded(); // 地面に接しているかどうか
        float controlFactor = grounded ? 1f : airControlMultiplier;  // 空中か地上かによる制御の効き具合
        float friction = grounded ? groundFriction : airFriction;  // 地面か空中かによる摩擦の効き具合

        // 現在の速度を取得
        Vector3 currentVel = rb.linearVelocity;
        // 目標の速度を設定（x軸方向の移動とy軸方向の移動はそのまま）
        Vector3 targetVel = new Vector3(inputDir.x * moveSpeed * controlFactor, currentVel.y, currentVel.z);

        // 現在の速度を目標の速度に向けてスムーズに補間
        rb.linearVelocity = Vector3.Lerp(currentVel, targetVel, friction * Time.deltaTime);
    }
}
