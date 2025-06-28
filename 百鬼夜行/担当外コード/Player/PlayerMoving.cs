using UnityEngine;

public class PlayerMoving : IPlayerState
{
    private Player player;
    private PlayerStateMachine playerStateMachine;
    private Rigidbody rb;

    [Header("�ړ��ݒ�")]
    public float moveSpeed = 8f;
    public float airControlMultiplier = 0.7f;  // �󒆐���̌����
    public float groundFriction = 15f;         // �n�ʂł̖��C��
    public float airFriction = 3f;            // �󒆂ł̖��C��

    private float moveInput = 0f;

    // ������
    public void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;

        rb = player.GetComponent<Rigidbody>();
    }

    // ���͂��󂯎��A�����ɔ��f
    public void HandleInput()
    {
        moveInput = playerStateMachine.HorizontalInput;  // �������̓��͂�ۑ�
    }

    // ���t���[���̈ړ�����
    public void Update()
    {
        // ���Ⴊ��ł����Ԃł͈ړ������Ȃ�
        var crouchState = playerStateMachine.GetState<PlayerCrouching>();
        if (crouchState != null && crouchState.GetCrouching()) { return; }

        Move();
    }

    // �K�v�ɉ����ď�ԏI�����ɏ���
    public void Remove()
    {
        // �K�v������ΏI������
    }

    // �ړ��̎��s
    private void Move()
    {
        Vector3 inputDir = new Vector3(moveInput, 0f, 0f).normalized;

        bool grounded = player.IsGrounded(); // �n�ʂɐڂ��Ă��邩�ǂ���
        float controlFactor = grounded ? 1f : airControlMultiplier;  // �󒆂��n�ォ�ɂ�鐧��̌����
        float friction = grounded ? groundFriction : airFriction;  // �n�ʂ��󒆂��ɂ�門�C�̌����

        // ���݂̑��x���擾
        Vector3 currentVel = rb.linearVelocity;
        // �ڕW�̑��x��ݒ�ix�������̈ړ���y�������̈ړ��͂��̂܂܁j
        Vector3 targetVel = new Vector3(inputDir.x * moveSpeed * controlFactor, currentVel.y, currentVel.z);

        // ���݂̑��x��ڕW�̑��x�Ɍ����ăX���[�Y�ɕ��
        rb.linearVelocity = Vector3.Lerp(currentVel, targetVel, friction * Time.deltaTime);
    }
}
