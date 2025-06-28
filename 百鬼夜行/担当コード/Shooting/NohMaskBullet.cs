using UnityEngine;

public class NohMaskBullet : Bullet
{
    private Shooting shooting;

    [SerializeField] private float waveAmplitude = 0.1f; // îgÇÃêUïù
    [SerializeField] private float waveFrequency = 10f;  // îgÇÃë¨Ç≥
    [SerializeField] private float bulletSpeed = 5f;     // íeë¨

    private Vector3 startPosition;
    private Vector3 direction;
    private Vector3 waveAxis;
    private float startTime;

    void Start()
    {
        shooting = GameObject.Find("Player").GetComponent<Shooting>();

        startPosition = transform.position;
        startTime = Time.time;

        // êeÇÃ shootingDirection ÇégÇ§
        direction = transform.forward.normalized;

        // Yé≤Ç…ÇŸÇ⁄êÇíºÅiè„åÇÇøÅjÇÃèÍçáÇÕç∂âEÇ…îgë≈Ç¬
        if (Mathf.Abs(direction.y) > 0.9f)
        {
            waveAxis = Vector3.right;
        }
        else
        {
            waveAxis = Vector3.up; // ÇªÇÍà»äOÇÕè„â∫Ç…îgë≈Ç¬
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

