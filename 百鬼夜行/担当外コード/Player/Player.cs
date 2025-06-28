using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    private CharacterHealthManager healthManager;
    private Rigidbody rb;

    [Header("�n�ʃ`�F�b�N�ݒ�")]
    public float groundCheckDistance = 0.6f;
    public LayerMask groundLayer;
    public Vector3 groundCheckOffsetLeft = new Vector3(-0.5f, 0f, 0f);
    public Vector3 groundCheckOffsetRight = new Vector3(0.5f, 0f, 0f);

    [SerializeField, LayerSelector] private int playerLayer;
    [SerializeField, LayerSelector] private int playerinvincibleLayer;
    public event System.Action OnAttackedByEnemy;


    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        healthManager = GetComponent<CharacterHealthManager>();

        playerStateMachine = new PlayerStateMachine();

        if (healthManager != null)
        {
            healthManager.OnDeath += PlayerDeath;
            healthManager.OnDamageTaken += PlayerDamage;
        }

        playerStateMachine.Init(this);
    }

    void Update()
    {
        playerStateMachine.StateSetting();
    }

    public bool IsGrounded()
    {
        if (rb.linearVelocity.y > 0.1f) return false;

        Vector3 leftOrigin = transform.position + groundCheckOffsetLeft;
        Vector3 rightOrigin = transform.position + groundCheckOffsetRight;

        bool leftHit = Physics.Raycast(leftOrigin, Vector3.down, groundCheckDistance, groundLayer);
        bool rightHit = Physics.Raycast(rightOrigin, Vector3.down, groundCheckDistance, groundLayer);

        return leftHit || rightHit;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 leftOrigin = transform.position + groundCheckOffsetLeft;
        Vector3 rightOrigin = transform.position + groundCheckOffsetRight;

        Gizmos.DrawLine(leftOrigin, leftOrigin + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(rightOrigin, rightOrigin + Vector3.down * groundCheckDistance);
    }

    private void PlayerDeath()
    {
        gameObject.SetActive(false);
    }

    private void PlayerDamage()
    {
        // ���G���C���[�ɕύX
        gameObject.layer = playerinvincibleLayer;
        StartCoroutine(ResetLayerAfterInvincibility());
    }

    private IEnumerator ResetLayerAfterInvincibility()
    {
        yield return new WaitForSeconds(healthManager.GetInvincibleDuration());
        // �ʏ탌�C���[�ɕύX
        gameObject.layer = playerLayer;
        Debug.Log("���G��ԏI��");
    }

    private void OnDestroy()
    {
        healthManager.OnDeath -= PlayerDeath;
        healthManager.OnDamageTaken -= PlayerDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            GameManager.Instance.Clear();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �U���Ɣ��肳�����̂Ƃ̐ڐG
        bool isBullet = collision.gameObject.GetComponent<Bullet>() != null;
        bool isEnemy = collision.gameObject.GetComponent<EnemyBase>() != null;

        if (isBullet || isEnemy)
        {
            OnAttackedByEnemy?.Invoke(); // �U�����ꂽ���Ƃ�ʒm
        }
    }
}
