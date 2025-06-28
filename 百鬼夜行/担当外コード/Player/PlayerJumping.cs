using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumping : IPlayerState
{
    private Player player;
    PlayerStateMachine playerStateMachine;
    private Rigidbody rb;

    [Header("ジャンプ設定")]
    public float jumpForce = 19f;

    [Header("重力補正")]
    public float fallMultiplier = 2.0f;
    public float ascentMultiplier = 2.0f;

    private bool isJumping = false;

    public void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        rb = player.GetComponent<Rigidbody>();

        Physics.gravity = new Vector3(0, -20f, 0);
        isJumping = false;
    }

    public void HandleInput()
    {
        var crouchState = playerStateMachine.GetState<PlayerCrouching>();
        if (!isJumping && playerStateMachine.JumpPressed && player.IsGrounded())
        {
            if (crouchState != null && crouchState.GetCrouching())
            {
                crouchState.StopCrouch();
            }

            if (crouchState == null || !crouchState.GetCrouching())
            {
                Jump();
                isJumping = true;
            }
        }
    }

    public void Update()
    {
        if (!isJumping) return;

        JumpCorrection();

        if (player.IsGrounded() && rb.linearVelocity.y <= 0.01f)
        {
            isJumping = false;
            playerStateMachine.DeactivateState(this); // プレイヤーに終了通知
        }
    }

    public void Remove()
    {
        isJumping = false;
    }

    private void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }

    private void JumpCorrection()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            if (playerStateMachine.JumpReleased)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f, rb.linearVelocity.z);
            }
            else
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (ascentMultiplier - 1f) * Time.deltaTime;
            }
        }
    }
}
