using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWall : TrapBase
{
    [SerializeField] private GameObject DetectionObject;
    [SerializeField] private float shootForce = 20f;
    private GameObject detectionobject;
    private PlayerDetection playerdetection;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if (playerdetection != null)
        {
            // Player�����m������
            if (playerdetection.IsPlayer())
            {
                Activate();
                playerdetection.ResetDetection(); // 1��Ń��Z�b�g
            }
        }
    }

    public override void Activate()
    {
        Debug.Log("NeedleTrap �����I");
        rb.linearVelocity = Vector2.down * shootForce;
    }
}