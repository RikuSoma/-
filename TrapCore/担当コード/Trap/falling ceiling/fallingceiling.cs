using System.Collections;
using UnityEngine;

public class FallingCeiling : TrapBase
{
    [SerializeField] private GameObject DetectionObject;
    [SerializeField] private Vector2 detectionOffset = new Vector2(0, -3.0f);

    private PlayerDetection playerdetection;
    [SerializeField] private float FallingTime = 1.0f;
    [SerializeField] private float endtime = 1.5f;

    private float uptime;
    private float activetime;

    private bool isReturning = false;
    private bool triggeredOnce = false;

    private Rigidbody2D rb;
    private Vector2 Initpos;

    void Start()
    {
        if (DetectionObject != null)
        {
            GameObject obj = Instantiate(DetectionObject, transform.position + (Vector3)detectionOffset, Quaternion.identity);
            DetectionObject = obj;
        }

        playerdetection = DetectionObject?.GetComponent<PlayerDetection>();
        if (playerdetection == null)
        {
            Debug.LogError("playerdetection is null");
        }

        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        Initpos = transform.position;
    }

    void Update()
    {
        // �v���C���[�����m���ꂽ���x�����g���K�[
        if (!triggeredOnce && playerdetection != null && playerdetection.IsPlayer())
        {
            triggeredOnce = true;
        }

        // �g���K�[���ꂽ�痎���܂ł̎��Ԃ��J�E���g
        if (triggeredOnce && !isReturning)
        {
            activetime += Time.deltaTime;

            if (activetime >= FallingTime)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isReturning = true;
            }
        }

        // ��������A��莞�Ԍo�������ɖ߂�
        if (isReturning && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            uptime += Time.deltaTime;

            if (uptime >= endtime)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        // Kinematic �ɖ߂��Ă���Ώ㏸����
        if (isReturning && rb.bodyType == RigidbodyType2D.Kinematic)
        {
            if (transform.position.y < Initpos.y)
            {
                transform.Translate(0, 0.01f, 0);
            }
            else
            {
                // ���S�ɖ߂����烊�Z�b�g
                transform.position = Initpos;
                activetime = 0f;
                uptime = 0f;
                triggeredOnce = false;
                isReturning = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            var health = collision.gameObject.GetComponent<PlayerHealthManager>();
            if (health != null)
            {
                health.TakeDamage(health.GetCurrentHealth()); // ����
            }
        }
    }
}
