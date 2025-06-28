using UnityEngine;

public class PlayerDash : IPlayerState
{
    private Player player;
    private PlayerStateMachine PlayerStateMachine;
    private Rigidbody rb;

    [Header("ダッシュ設定")]
    public float dashSpeed = 15f;           // ダッシュのスピード
    public float dashDuration = 0.3f;       // ダッシュの継続時間
    public float dashCooldown = 0.5f;         // クールダウン時間

    private bool isDashing = false;
    private float dashStartTime = 0f;
    private float nextDashTime = 0f;        // 次にダッシュできる時間

        private Vector3 dashDirection;

    //デバッグ
    //private Vector3 dashStartPosition;

    // 初期化
    public void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.PlayerStateMachine = playerStateMachine;

        rb = player.GetComponent<Rigidbody>();
    }

    // 入力処理
    public void HandleInput()
    {
        if (PlayerStateMachine.DashPressed && !PlayerStateMachine.IsCrouching && Time.time >= nextDashTime && !isDashing)
        {
            float moveInput = PlayerStateMachine.HorizontalInput;

            if (moveInput != 0f)
            {
                dashDirection = new Vector3(moveInput, 0f, 0f).normalized;
                isDashing = true;
                dashStartTime = Time.time;
                nextDashTime = Time.time + dashCooldown;

                ////デバッグ用
                //dashStartPosition = player.transform.position;

            }
        }
    }

    // ダッシュ処理
    public void Update()
    {
        if (isDashing)
        {
            // ダッシュ時間内は高速移動
            if (Time.time < dashStartTime + dashDuration)
            {
                rb.linearVelocity = dashDirection * dashSpeed;
            }
            else
            {
                // ダッシュ終了、移動状態に戻る
                isDashing = false;
                rb.linearVelocity = Vector3.zero;

                ////  距離を計測・ログ表示（デバッグ）
                //float dashDistance = Vector3.Distance(dashStartPosition, player.transform.position);
                //Debug.Log($"[Dash] Distance dashed: {dashDistance:F2} units");


                PlayerStateMachine.ActivateState(new PlayerMoving());  // ダッシュ後に移動状態に戻る
            }
        }
    }

    // 状態終了時の処理
    public void Remove()
    {
        isDashing = false;
    }
}
