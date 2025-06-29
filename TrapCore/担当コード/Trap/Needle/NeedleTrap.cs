using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleTrap : TrapBase
{
    [SerializeField] private GameObject DetectionObject;
    private GameObject detectionobject;
    private PlayerDetection playerdetection;
    [SerializeField] private float shootForce = 20f;
    [SerializeField] private bool IsActive;

    private Rigidbody2D rb;
    private bool hasActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        if(IsActive)
        {
            // ���g�̏��Player���m�p�I�u�W�F�N�g��u��
            detectionobject = Instantiate(DetectionObject, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);
            playerdetection = detectionobject.GetComponent<PlayerDetection>();
            if (playerdetection == null)
            {
                Debug.LogError("playerdetection is null");
            }

            rb = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerdetection != null && IsActive && !hasActivated)
        {
            // Player�����m������
            if(playerdetection.IsPlayer())
            {
                Activate();
                hasActivated = true;
                playerdetection.ResetDetection(); // 1��Ń��Z�b�g
            }
        }
    }
    public override void Activate()
    {
        Debug.Log("NeedleTrap �����I");
        rb.linearVelocity = Vector2.up * shootForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealthManager playerhealth = collision.gameObject.GetComponent<PlayerHealthManager>();

        if (playerhealth)
        {
            playerhealth.TakeDamage(Power);
        }
    }

}
