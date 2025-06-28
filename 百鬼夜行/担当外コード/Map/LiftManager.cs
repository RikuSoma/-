using UnityEngine;

public class LiftManager : MonoBehaviour
{
    [Header("�ړ��͈́iX���W�j")]
    public float leftX = -5f;     // ���[���W
    public float rightX = 5f;     // �E�[���W
    public float speed = 2f;      // �ړ����x

    private bool movingRight = true;

    void Update()
    {
        // ���E�Ɉړ�����
        Vector3 pos = transform.position;

        if (movingRight)
        {
            pos.x += speed * Time.deltaTime;
            if (pos.x >= rightX)
                movingRight = false;
        }
        else
        {
            pos.x -= speed * Time.deltaTime;
            if (pos.x <= leftX)
                movingRight = true;
        }

        transform.position = pos;
    }

    // �v���C���[���������q�I�u�W�F�N�g�ɂ��Ĉꏏ�Ɉړ�
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    // �v���C���[���~�肽��e�q�֌W������
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
