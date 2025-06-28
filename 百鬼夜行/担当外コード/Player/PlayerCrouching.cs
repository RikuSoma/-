using UnityEngine;

/// <summary>
/// �v���C���[�́u���Ⴊ�݁v��Ԃ��Ǘ�����X�e�[�g�B
/// ����ʍs�̏������蔲���鏈�����S������B
/// </summary>
public class PlayerCrouching : IPlayerState
{
    private Player player;
    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;
    private Collider playerCollider; // ���蔲�������ɕK�v

    // ���Ⴊ�݊֘A�̕ϐ�
    private Vector3 playerOriginalScale;
    private float playerOriginalHeightPosition;
    private bool isCurrentlyCrouching; // crouching���疼�O�ύX
    [SerializeField] private Vector3 crouchingScale = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private float crouchSpeed = 10f;

    // ������ PlayerController3D����ړ����Ă����ϐ� ������
    [Header("���蔲���ݒ�")]
    public float fallInitialForce = 10f; // public�ɕύX���Ē����\��
    private int playerOriginalLayer;
    private OneWayPlatform3D currentFallingPlatform = null;
    private const string DEBUG_KEY = "[PlayerCrouching_FallThrough]";
    // ������ PlayerController3D����ړ����Ă����ϐ� ������

    public bool GetCrouching() => isCurrentlyCrouching;

    public void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.rb = player.GetComponent<Rigidbody>();
        this.playerCollider = player.GetComponent<Collider>(); // Collider���擾

        // �I���W�i���̃X�P�[���ƃ��C���[���L��
        this.playerOriginalScale = player.transform.localScale;
        this.playerOriginalLayer = player.gameObject.layer;

        // Null�`�F�b�N
        if (playerCollider == null) Debug.LogError($"{DEBUG_KEY}: Player��Collider���A�^�b�`����Ă��܂���I", player);
    }

    public void HandleInput()
    {
        // ���̃X�e�[�g�ł�Update�œ��͂𒼐ڌ��邽�߁A�����͋��OK
    }

    public void Update()
    {
        // ������ PlayerController3D����ړ����Ă������� ������
        HandleFallThroughPlatform();
        // ������ PlayerController3D����ړ����Ă������� ������

        // ���Ⴊ�ݏ�Ԃ��ǂ������m�F���A��Ԃ�؂�ւ���
        if (playerStateMachine.IsCrouching && player.IsGrounded())
        {
            StartCrouch();
        }
        else
        {
            StopCrouch();
        }

        // �����ڂ̕�ԏ���
        if (isCurrentlyCrouching)
        {
            DoCrouchVisuals();
        }
        else
        {
            DoStandVisuals();
        }
    }

    public void Remove()
    {
        // ���̏�Ԃ����S�ɏI�������Ƃ��́A�����Ɍ��̏�Ԃɖ߂�
        ResetScaleAndPosition();
        RestorePlatformCollision(); // ���蔲�����������ꍇ���l��
        isCurrentlyCrouching = false;
    }

    // --- ���Ⴊ�݊֘A�̃��\�b�h ---

    private void StartCrouch()
    {
        if (isCurrentlyCrouching) return;
        isCurrentlyCrouching = true;
        playerOriginalHeightPosition = player.transform.position.y;
    }

    public void StopCrouch()
    {
        if (isCurrentlyCrouching)
        {
            isCurrentlyCrouching = false;
        }
    }

    private void DoCrouchVisuals()
    {
        if (player.IsGrounded())
        {
            float newScaleY = Mathf.MoveTowards(player.transform.localScale.y, crouchingScale.y, crouchSpeed * Time.deltaTime);
            float heightDifference = (playerOriginalScale.y - newScaleY) * 0.5f;

            player.transform.localScale = new Vector3(playerOriginalScale.x, newScaleY, playerOriginalScale.z);
            player.transform.position = new Vector3(
                player.transform.position.x,
                playerOriginalHeightPosition - heightDifference,
                player.transform.position.z
            );
        }
    }

    private void DoStandVisuals()
    {
        if (player.IsGrounded())
        {
            float newScaleY = Mathf.MoveTowards(player.transform.localScale.y, playerOriginalScale.y, crouchSpeed * Time.deltaTime);
            float heightDifference = (playerOriginalScale.y - newScaleY) * 0.5f;

            player.transform.localScale = new Vector3(playerOriginalScale.x, newScaleY, playerOriginalScale.z);
            player.transform.position = new Vector3(
                player.transform.position.x,
                playerOriginalHeightPosition - heightDifference,
                player.transform.position.z
            );
        }
    }

    private void ResetScaleAndPosition()
    {
        player.transform.localScale = playerOriginalScale;
    }


    // ������ PlayerController3D����ړ����Ă������\�b�h�Q ������

    /// <summary>
    /// ����ʍs�̏��̂��蔲���������Ǘ�����
    /// </summary>
    private void HandleFallThroughPlatform()
    {
        bool isCrouchHeld = playerStateMachine.IsCrouching;

        if (isCrouchHeld)
        {
            // ���łɂ��蔲�����A�܂��͋󒆂ɂ���ꍇ�͉������Ȃ�
            if (currentFallingPlatform != null || !player.IsGrounded()) return;

            // �^����Ray���΂��āA���蔲���\�ȏ������邩�`�F�b�N
            if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 1.0f))
            {
                var platform = hit.collider.GetComponentInParent<OneWayPlatform3D>();
                if (platform != null)
                {
                    currentFallingPlatform = platform;
                    platform.IgnoreCollision(playerCollider);
                    player.gameObject.layer = LayerMask.NameToLayer("FallingPlayer");
                    rb.AddForce(Vector3.down * fallInitialForce, ForceMode.Impulse);
                    Debug.Log($"{DEBUG_KEY}: ���蔲���������J�n���܂��I");
                }
            }
        }
        else
        {
            // ���Ⴊ�݃L�[�������ꂽ��A�Փ˂����ɖ߂�
            RestorePlatformCollision();
        }
    }

    /// <summary>
    /// ���蔲�����̏��Ƃ̏Փ˔�������ɖ߂�
    /// </summary>
    private void RestorePlatformCollision()
    {
        if (currentFallingPlatform != null)
        {
            currentFallingPlatform.RestoreCollision(playerCollider);
            player.gameObject.layer = playerOriginalLayer;
            currentFallingPlatform = null;
            Debug.Log($"{DEBUG_KEY}: ���蔲���������I�����܂����B");
        }
    }
    // ������ PlayerController3D����ړ����Ă������\�b�h�Q ������
}