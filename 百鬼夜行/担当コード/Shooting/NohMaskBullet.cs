using UnityEngine;

public class NohMaskBullet : Bullet
{
    private Shooting shooting;

    [SerializeField] private float waveAmplitude = 0.1f; // �g�̐U��
    [SerializeField] private float waveFrequency = 10f;  // �g�̑���
    [SerializeField] private float bulletSpeed = 5f;     // �e��

    private Vector3 startPosition;
    private Vector3 direction;
    private Vector3 waveAxis;
    private float startTime;

    void Start()
    {
        shooting = GameObject.Find("Player").GetComponent<Shooting>();

        startPosition = transform.position;
        startTime = Time.time;

        // �e�� shootingDirection ���g��
        direction = transform.forward.normalized;

        // Y���ɂقڐ����i�㌂���j�̏ꍇ�͍��E�ɔg�ł�
        if (Mathf.Abs(direction.y) > 0.9f)
        {
            waveAxis = Vector3.right;
        }
        else
        {
            waveAxis = Vector3.up; // ����ȊO�͏㉺�ɔg�ł�
        }
    }

    void Update()
    {
        float timeElapsed = Time.time - startTime;

        Vector3 forwardMovement = direction * shooting.GetBulletSpeed() * timeElapsed;
        Vector3 waveOffset = waveAxis * Mathf.Sin(timeElapsed * waveFrequency) * waveAmplitude;

        transform.position = startPosition + forwardMovement + waveOffset;
    }
}

