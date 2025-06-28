using UnityEngine;

public class PlayerDash : IPlayerState
{
    private Player player;
    private PlayerStateMachine PlayerStateMachine;
    private Rigidbody rb;

    [Header("�_�b�V���ݒ�")]
    public float dashSpeed = 15f;           // �_�b�V���̃X�s�[�h
    public float dashDuration = 0.3f;       // �_�b�V���̌p������
    public float dashCooldown = 0.5f;         // �N�[���_�E������

    private bool isDashing = false;
    private float dashStartTime = 0f;
    private float nextDashTime = 0f;        // ���Ƀ_�b�V���ł��鎞��

        private Vector3 dashDirection;

    //�f�o�b�O
    //private Vector3 dashStartPosition;

    // ������
    public void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.PlayerStateMachine = playerStateMachine;

        rb = player.GetComponent<Rigidbody>();
    }

    // ���͏���
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

                ////�f�o�b�O�p
                //dashStartPosition = player.transform.position;

            }
        }
    }

    // �_�b�V������
    public void Update()
    {
        if (isDashing)
        {
            // �_�b�V�����ԓ��͍����ړ�
            if (Time.time < dashStartTime + dashDuration)
            {
                rb.linearVelocity = dashDirection * dashSpeed;
            }
            else
            {
                // �_�b�V���I���A�ړ���Ԃɖ߂�
                isDashing = false;
                rb.linearVelocity = Vector3.zero;

                ////  �������v���E���O�\���i�f�o�b�O�j
                //float dashDistance = Vector3.Distance(dashStartPosition, player.transform.position);
                //Debug.Log($"[Dash] Distance dashed: {dashDistance:F2} units");


                PlayerStateMachine.ActivateState(new PlayerMoving());  // �_�b�V����Ɉړ���Ԃɖ߂�
            }
        }
    }

    // ��ԏI�����̏���
    public void Remove()
    {
        isDashing = false;
    }
}
