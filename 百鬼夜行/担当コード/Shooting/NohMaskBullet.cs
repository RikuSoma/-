using UnityEngine;

public class NohMaskBullet : Bullet
{
    private Shooting shooting;

    [SerializeField] private float waveAmplitude = 0.1f; // 波の振幅
    [SerializeField] private float waveFrequency = 10f;  // 波の速さ
    [SerializeField] private float bulletSpeed = 5f;     // 弾速

    private Vector3 startPosition;
    private Vector3 direction;
    private Vector3 waveAxis;
    private float startTime;

    void Start()
    {
        shooting = GameObject.Find("Player").GetComponent<Shooting>();

        startPosition = transform.position;
        startTime = Time.time;

        // 親の shootingDirection を使う
        direction = transform.forward.normalized;

        // Y軸にほぼ垂直（上撃ち）の場合は左右に波打つ
        if (Mathf.Abs(direction.y) > 0.9f)
        {
            waveAxis = Vector3.right;
        }
        else
        {
            waveAxis = Vector3.up; // それ以外は上下に波打つ
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

