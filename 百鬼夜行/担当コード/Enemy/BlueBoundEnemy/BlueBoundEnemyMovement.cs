using UnityEngine;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;

public class BlueBoundEnemyMovement : MonoBehaviour
{
    // --- Public�ȕϐ���v���p�e�B�i�eState����A�N�Z�X�ł���悤��public�ɂ���j ---
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

    // --- �R���|�[�l���g�ւ̎Q�� ---
    public Rigidbody Rb { get; private set; }
    public GameObject TargetObject { get; private set; }
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Vector3 OriginalScale { get; private set; }


    // --- �X�e�[�g�Ǘ� ---
    private BaseState currentState;
    // �e�X�e�[�g�̃C���X�^���X��ێ�
    public IdleState idleState;
    public PreparingState preparingState;
    public JumpingState jumpingState;
    public StuckRecoveryState stuckRecoveryState;

    void Awake()
    {
        // --- �R���|�[�l���g�̎擾 ---
        Rb = GetComponent<Rigidbody>();
        OriginalScale = transform.localScale;

        // --- �S�ẴX�e�[�g���C���X�^���X�� ---
        idleState = new IdleState(this);
        preparingState = new PreparingState(this);
        jumpingState = new JumpingState(this);
        stuckRecoveryState = new StuckRecoveryState(this);
    }

    void Start()
    {
        // �����X�e�[�g��ݒ�
        currentState = idleState;
        currentState.EnterState();
    }

    void Update()
    {
        if (TargetObject == null) return;

        // ���݂̃X�e�[�g��Update�������Ăяo������
        currentState?.UpdateState();
    }

    // �X�e�[�g��؂�ւ��邽�߂̃��\�b�h
    public void ChangeState(BaseState newState)
    {
        currentState?.ExitState(); // ���݂̃X�e�[�g�̏I���������Ă�
        currentState = newState;
        currentState.EnterState();  // �V�����X�e�[�g�̊J�n�������Ă�
    }

    // ���Ⴊ�ݓ���̃R���[�`���iState����Ăяo�����j
    public IEnumerator CrouchCoroutine()
    {
        transform.localScale = new Vector3(OriginalScale.x, crouchScaleY, OriginalScale.z);
        yield return new WaitForSeconds(crouchDuration);
        transform.localScale = OriginalScale;

        // ���Ⴊ�ݏI�������W�����v���s
        PerformJump();
    }

    // �W�����v���s����
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

    // �n�ʐڒn����
    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
    }

    // BlueBoundEnemyMovement.cs �ɒǋL
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f); // IsGrounded�̔��a�ƍ��킹��
        }
    }

    // --- �O������ݒ肷�邽�߂̃��\�b�h ---
    public void SetTargetObject(GameObject target) { TargetObject = target; }
    public void SetMovementConfig(BlueBoundMovementConfig config) { /* ... */ }
}